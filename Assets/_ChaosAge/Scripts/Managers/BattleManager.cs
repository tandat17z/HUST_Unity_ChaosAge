using System;
using System.Collections.Generic;
using System.Linq;
using AILibraryForNPC.Core;
using AStarPathfinding;
using ChaosAge.Battle;
using ChaosAge.Config;
using ChaosAge.Data;
using ChaosAge.input;
using DatSystem.UI;
using DatSystem.utils;
using Sirenix.OdinInspector;
using UnityEngine;
using Grid = AStarPathfinding.Grid;

namespace ChaosAge.manager
{
    public class BattleManager : Singleton<BattleManager>
    {
        protected override void OnAwake() { }

        public long id = 0;
        public DateTime baseTime;
        public int frameCount = 0;
        public long defender = 0;
        public long attacker = 0;
        public List<BattleBuilding> _buildings = new List<BattleBuilding>();
        public List<BattleUnit> _units = new List<BattleUnit>();
        public List<UnitToAdd> _unitsToAdd = new List<UnitToAdd>();
        public Grid grid = null;
        private Grid unlimitedGrid = null;
        private AStarSearch search = null;
        private AStarSearch unlimitedSearch = null;
        public List<Tile> blockedTiles = new List<Tile>();
        private List<BattleProjectile> projectiles = new List<BattleProjectile>();
        public float percentage = 0;

        private bool _isBattling = false;
        private float _preTimer = 0;
        private float _currentTimer = 0;

        //public delegate void UnitSpawned(long id);
        //public delegate void AttackCallback(long index, BattleVector2 target);
        //public delegate void IndexCallback(long index);
        //public delegate void FloatCallback(long index, float value);


        public List<BattleBuilding> Buildings => _buildings;
        public List<BattleUnit> Units => _units;
        public List<BattleProjectile> Projectiles => projectiles;

        public void LoadLevel(int level)
        {
            BuildingManager.Instance.Clear();

            var opponentData = PlayerData.LoadFromFile($"Assets/Levels/{level}.json");
            Initialize(opponentData.buildings);
        }

        public void Initialize(List<BuildingData> buildings)
        {
            var battleBuildings = new List<BattleBuilding>();
            foreach (var data in buildings)
            {
                var b = FactoryManager.Instance.SpawnBuilding(data.type);
                b.SetInfo(data.id, data.level);
                b.PlacedOnGrid(data.x, data.y);

                var battleBuilding = b.GetComponent<BattleBuilding>();
                battleBuilding.SetInfo(data);
                battleBuildings.Add(battleBuilding);
            }

            Initialize(battleBuildings);
        }

        [Button("Init")]
        public void Initialize(List<BattleBuilding> buildings) // ok
        {
            _preTimer = 0;
            _currentTimer = 0;

            baseTime = DateTime.Now;
            frameCount = 0;
            percentage = 0;
            _buildings = buildings;

            // Thuật toán tìm đường
            grid = new Grid(ConfigData.gridSize, ConfigData.gridSize);
            unlimitedGrid = new Grid(ConfigData.gridSize, ConfigData.gridSize);
            search = new AStarSearch(grid);
            unlimitedSearch = new AStarSearch(unlimitedGrid);

            for (int i = 0; i < _buildings.Count; i++)
            {
                _buildings[i].Initialize();
                _buildings[i].worldCenterPosition = new BattleVector2(
                    (
                        _buildings[i].battleBuidlingConfig.x
                        + (_buildings[i].battleBuidlingConfig.columns / 2f)
                    ) * ConfigData.gridCellSize,
                    (
                        _buildings[i].battleBuidlingConfig.y
                        + (_buildings[i].battleBuidlingConfig.rows / 2f)
                    ) * ConfigData.gridCellSize
                );

                // blockTiles: Vẫn đi được ở rìa công trình
                int startX = _buildings[i].battleBuidlingConfig.x;
                int endX =
                    _buildings[i].battleBuidlingConfig.x
                    + _buildings[i].battleBuidlingConfig.columns;

                int startY = _buildings[i].battleBuidlingConfig.y;
                int endY =
                    _buildings[i].battleBuidlingConfig.y + _buildings[i].battleBuidlingConfig.rows;

                if (
                    _buildings[i].battleBuidlingConfig.type != EBuildingType.wall
                    && _buildings[i].battleBuidlingConfig.columns > 1
                    && _buildings[i].battleBuidlingConfig.rows > 1
                )
                {
                    startX++;
                    startY++;
                    endX--;
                    endY--;
                    if (endX <= startX || endY <= startY)
                    {
                        continue;
                    }
                }

                for (int x = startX; x < endX; x++)
                {
                    for (int y = startY; y < endY; y++)
                    {
                        grid[x, y].Blocked = true;
                        blockedTiles.Add(
                            new Tile(
                                _buildings[i].battleBuidlingConfig.type,
                                new BattleVector2Int(x, y),
                                i
                            )
                        );
                    }
                }
            }

            _isBattling = true;
        }

        private void FixedUpdate()
        {
            if (_isBattling)
            {
                _currentTimer += Time.deltaTime;
                if (_currentTimer - _preTimer > ConfigData.battleFrameRate)
                {
                    Debug.Log("ExecuteFrame");
                    ExecuteFrame();
                    _preTimer += ConfigData.battleFrameRate;
                }
            }
        }

        public void AddUnit(EUnitType unitType, int x, int y) // ok
        {
            UnitToAdd unitToAdd = new UnitToAdd();

            var battleUnit = FactoryManager.Instance.SpawnUnit(unitType);
            battleUnit.Initialize(x, y);
            unitToAdd.unit = battleUnit;
            unitToAdd.x = x;
            unitToAdd.y = y;
            _unitsToAdd.Add(unitToAdd);
            /*
            if(time > updateTime)
            {
                updateTime = time;
            }
            */
        }

        public void ExecuteFrame()
        {
            CheckEnd();
            int addIndex = _units.Count;
            for (int i = _unitsToAdd.Count - 1; i >= 0; i--)
            {
                if (CanAddUnit(_unitsToAdd[i].x, _unitsToAdd[i].y))
                {
                    _units.Insert(addIndex, _unitsToAdd[i].unit);
                }
                _unitsToAdd.RemoveAt(i);
            }

            for (int i = 0; i < _buildings.Count; i++)
            {
                if (
                    _buildings[i].battleBuidlingConfig.targetType != Data.BuildingTargetType.none
                    && _buildings[i].health > 0
                )
                {
                    _buildings[i].HandleBuilding(i, ConfigData.battleFrameRate);
                }
            }

            for (int i = 0; i < _units.Count; i++)
            {
                if (_units[i].health > 0)
                {
                    _units[i].HandleUnit(i, ConfigData.battleFrameRate);
                }
            }

            if (projectiles.Count > 0)
            {
                for (int i = projectiles.Count - 1; i >= 0; i--)
                {
                    projectiles[i].HandleProjectile(ConfigData.battleFrameRate);
                }
            }

            frameCount++;
        }

        private void CheckEnd()
        {
            if (CheckEndBuilding())
            {
                _isBattling = false;
                PanelManager.Instance.OpenPanel<PopupWin>();
            }

            //if (_units.Count == 0)
            //{
            //    _isBattling = false;
            //    PanelManager.Instance.OpenPanel<PopupLoss>();
            //}
        }

        private bool CheckEndBuilding()
        {
            foreach (var b in _buildings)
            {
                if (b.health > 0)
                    return false;
            }
            return true;
        }

        public static BattleVector2 GridToWorldPosition(BattleVector2Int position) // ok
        {
            return new BattleVector2(
                position.x * ConfigData.gridCellSize + ConfigData.gridCellSize / 2f,
                position.y * ConfigData.gridCellSize + ConfigData.gridCellSize / 2f
            );
        }

        public bool IsBuildingInRange(int unitIndex, int buildingIndex)
        {
            for (
                int x = _buildings[buildingIndex].battleBuidlingConfig.x;
                x
                    < _buildings[buildingIndex].battleBuidlingConfig.x
                        + _buildings[buildingIndex].battleBuidlingConfig.columns;
                x++
            )
            {
                for (
                    int y = _buildings[buildingIndex].battleBuidlingConfig.y;
                    y
                        < _buildings[buildingIndex].battleBuidlingConfig.y
                            + _buildings[buildingIndex].battleBuidlingConfig.columns;
                    y++
                )
                {
                    float distance = BattleVector2.Distance(
                        GridToWorldPosition(new BattleVector2Int(x, y)),
                        _units[unitIndex].position
                    );
                    if (distance <= _units[unitIndex].unit.attackRange)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static BattleVector2 GetPathPosition(IList<Cell> path, float t) // ok
        {
            if (t < 0)
            {
                t = 0;
            }
            if (t > 1)
            {
                t = 1;
            }
            float totalLength = GetPathLength(path);
            float length = 0;
            if (path != null && path.Count > 1)
            {
                for (int i = 1; i < path.Count; i++)
                {
                    BattleVector2Int a = new BattleVector2Int(
                        path[i - 1].Location.X,
                        path[i - 1].Location.Y
                    );
                    BattleVector2Int b = new BattleVector2Int(
                        path[i].Location.X,
                        path[i].Location.Y
                    );
                    float l = BattleVector2.Distance(a, b) * ConfigData.gridCellSize;
                    float p = (length + l) / totalLength;
                    if (p >= t)
                    {
                        t = (t - (length / totalLength)) / (p - (length / totalLength));
                        return BattleVector2.LerpUnclamped(
                            GridToWorldPosition(a),
                            GridToWorldPosition(b),
                            t
                        );
                    }
                    length += l;
                }
            }
            return GridToWorldPosition(
                new BattleVector2Int(path[0].Location.X, path[0].Location.Y)
            );
        }

        public bool CanAddUnit(int x, int y) // ok
        {
            for (int i = 0; i < _buildings.Count; i++)
            {
                if (_buildings[i].health <= 0)
                {
                    continue;
                }

                int startX = _buildings[i].battleBuidlingConfig.x;
                int endX =
                    _buildings[i].battleBuidlingConfig.x
                    + _buildings[i].battleBuidlingConfig.columns;

                int startY = _buildings[i].battleBuidlingConfig.y;
                int endY =
                    _buildings[i].battleBuidlingConfig.y + _buildings[i].battleBuidlingConfig.rows;

                for (int x2 = startX; x2 < endX; x2++)
                {
                    for (int y2 = startY; y2 < endY; y2++)
                    {
                        if (x == x2 && y == y2)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        public bool FindTargetForBuilding(int index) // ok
        {
            Debug.Log("FindTargetForBuilding");
            for (int i = 0; i < _units.Count; i++)
            {
                if (
                    _units[i].health <= 0
                    || _units[i].unit.movement == Data.UnitMoveType.underground
                        && _units[i].path != null
                )
                {
                    continue;
                }

                if (
                    _buildings[index].battleBuidlingConfig.targetType
                        == Data.BuildingTargetType.ground
                    && _units[i].unit.movement == Data.UnitMoveType.fly
                )
                {
                    continue;
                }

                if (
                    _buildings[index].battleBuidlingConfig.targetType == Data.BuildingTargetType.air
                    && _units[i].unit.movement != Data.UnitMoveType.fly
                )
                {
                    continue;
                }

                if (IsUnitInRange(i, index))
                {
                    _buildings[index].attackTimer = 0;
                    _buildings[index].target = i;
                    return true;
                }
            }
            return false;
        }

        public bool IsUnitInRange(int unitIndex, int buildingIndex) // ok
        {
            float distance = BattleVector2.Distance(
                _buildings[buildingIndex].worldCenterPosition,
                _units[unitIndex].position
            );
            if (distance <= _buildings[buildingIndex].battleBuidlingConfig.radius)
            {
                if (
                    _buildings[buildingIndex].battleBuidlingConfig.blindRange > 0
                    && distance <= _buildings[buildingIndex].battleBuidlingConfig.blindRange
                )
                {
                    return false;
                }
                return true;
            }
            return false;
        }

        private static float GetPathLength(IList<Cell> path, bool includeCellSize = true) // ok
        {
            float length = 0;
            if (path != null && path.Count > 1)
            {
                for (int i = 1; i < path.Count; i++)
                {
                    length += BattleVector2.Distance(
                        new BattleVector2(path[i - 1].Location.X, path[i - 1].Location.Y),
                        new BattleVector2(path[i].Location.X, path[i].Location.Y)
                    );
                }
            }
            if (includeCellSize)
            {
                length *= ConfigData.gridCellSize;
            }
            return length;
        }

        public void FindTargets(int index, TargetPriority priority) // ok
        {
            ListUnitTargets(index, priority);
            if (priority == TargetPriority.defenses)
            {
                if (_units[index].defenceTargets.Count > 0)
                {
                    AssignTarget(index, ref _units[index].defenceTargets);
                }
                else
                {
                    FindTargets(index, Data.TargetPriority.all);
                    return;
                }
            }
            else if (priority == TargetPriority.resources)
            {
                if (_units[index].resourceTargets.Count > 0)
                {
                    AssignTarget(index, ref _units[index].resourceTargets);
                }
                else
                {
                    FindTargets(index, Data.TargetPriority.all);
                    return;
                }
            }
            else if (priority == TargetPriority.all || priority == TargetPriority.walls)
            {
                Dictionary<int, float> temp = _units[index].GetAllTargets();
                if (temp.Count > 0)
                {
                    AssignTarget(index, ref temp, priority == TargetPriority.walls);
                }
                else
                {
                    return;
                }
            }
        }

        /// <summary>
        /// Gán các building hiện tại vào listTarget
        /// </summary>
        /// <param name="unitIdx"></param>
        /// <param name="priority"></param>
        private void ListUnitTargets(int unitIdx, TargetPriority priority) // ok
        {
            _units[unitIdx].resourceTargets.Clear();
            _units[unitIdx].defenceTargets.Clear();
            _units[unitIdx].otherTargets.Clear();
            if (priority == TargetPriority.walls)
            {
                priority = TargetPriority.all;
            }
            for (int i = 0; i < _buildings.Count; i++)
            {
                if (
                    _buildings[i].health <= 0
                    || priority != _units[unitIdx].unit.priority
                    || !IsBuildingCanBeAttacked(_buildings[i].battleBuidlingConfig.type)
                )
                {
                    continue;
                }
                float distance = BattleVector2.Distance(
                    _buildings[i].worldCenterPosition,
                    _units[unitIdx].position
                );
                switch (_buildings[i].battleBuidlingConfig.type)
                {
                    case EBuildingType.townhall:
                    case EBuildingType.elixirmine:
                    case EBuildingType.elixirstorage:
                    //case EBuildingType.darkelixirmine:
                    //case EBuildingType.darkelixirstorage:
                    case EBuildingType.goldmine:
                    case EBuildingType.goldstorage:
                        _units[unitIdx].resourceTargets.Add(i, distance);
                        break;
                    case EBuildingType.cannon:
                    case EBuildingType.archertower:
                        //case EBuildingType.mortor:
                        //case EBuildingType.airdefense:
                        //case EBuildingType.wizardtower:
                        //case EBuildingType.hiddentesla:
                        //case EBuildingType.bombtower:
                        //case EBuildingType.xbow:
                        //case EBuildingType.infernotower:
                        _units[unitIdx].defenceTargets.Add(i, distance);
                        break;
                    case EBuildingType.wall:
                        // Don't include
                        break;
                    default:
                        _units[unitIdx].otherTargets.Add(i, distance);
                        break;
                }
            }
        }

        public static bool IsBuildingCanBeAttacked(EBuildingType id)
        {
            //    switch (id)
            //    {
            //        //case EBuildingType.obstacle:
            //        //case EBuildingType.decoration:
            //        //case EBuildingType.boomb:
            //        //case EBuildingType.springtrap:
            //        //case EBuildingType.airbomb:
            //        //case EBuildingType.giantbomb:
            //        //case EBuildingType.seekingairmine:
            //        //case EBuildingType.skeletontrap:
            //            return false;
            //}
            return true;
        }

        private void AssignTarget(
            int index,
            ref Dictionary<int, float> targets,
            bool wallsPriority = false
        ) // ok
        {
            //if (wallsPriority)
            //{
            //    var wallPath = GetPathToWall(index, ref targets);
            //    if (wallPath.Item1 >= 0)
            //    {
            //        _units[index].AssignTarget(wallPath.Item1, wallPath.Item2);
            //        return;
            //    }
            //}

            // lấy building id có distance nhỏ nhất
            int min = targets.Aggregate((a, b) => a.Value < b.Value ? a : b).Key;
            var path = GetPathToBuilding(min, index);
            if (path.Item1 >= 0)
            {
                _units[index].AssignTarget(path.Item1, path.Item2);
            }
        }

        private (int, Path) GetPathToWall(int unitIndex, ref Dictionary<int, float> targets) // ok
        {
            BattleVector2Int unitGridPosition = WorldToGridPosition(_units[unitIndex].position);
            List<Path> tiles = new List<Path>();
            foreach (
                var target in (targets.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value))
            )
            {
                List<Cell> points = search
                    .Find(
                        new AStarPathfinding.Vector2Int(
                            _buildings[target.Key].battleBuidlingConfig.x,
                            _buildings[target.Key].battleBuidlingConfig.y
                        ),
                        new AStarPathfinding.Vector2Int(unitGridPosition.x, unitGridPosition.y)
                    )
                    .ToList();
                if (
                    Path.IsValid(
                        ref points,
                        new AStarPathfinding.Vector2Int(
                            _buildings[target.Key].battleBuidlingConfig.x,
                            _buildings[target.Key].battleBuidlingConfig.y
                        ),
                        new AStarPathfinding.Vector2Int(unitGridPosition.x, unitGridPosition.y)
                    )
                )
                {
                    continue;
                }
                else
                {
                    for (int i = 0; i < _units.Count; i++)
                    {
                        if (
                            _units[i].health <= 0
                            || _units[i].unit.movement != Data.UnitMoveType.ground
                            || i != unitIndex
                            || _units[i].target < 0
                            || _units[i].mainTarget != target.Key
                            || _units[i].mainTarget < 0
                            || _buildings[_units[i].mainTarget].battleBuidlingConfig.type
                                != EBuildingType.wall
                            || _buildings[_units[i].mainTarget].health <= 0
                        )
                        {
                            continue;
                        }
                        BattleVector2Int pos = WorldToGridPosition(_units[i].position);
                        List<Cell> pts = search
                            .Find(
                                new AStarPathfinding.Vector2Int(pos.x, pos.y),
                                new AStarPathfinding.Vector2Int(
                                    unitGridPosition.x,
                                    unitGridPosition.y
                                )
                            )
                            .ToList();
                        if (
                            Path.IsValid(
                                ref pts,
                                new AStarPathfinding.Vector2Int(pos.x, pos.y),
                                new AStarPathfinding.Vector2Int(
                                    unitGridPosition.x,
                                    unitGridPosition.y
                                )
                            )
                        )
                        {
                            float dis = GetPathLength(pts, false);
                            if (id <= ConfigData.battleGroupWallAttackRadius)
                            {
                                AStarPathfinding.Vector2Int end = _units[i]
                                    .path.points.Last()
                                    .Location;
                                Path p = new Path();
                                if (p.Create(ref search, pos, new BattleVector2Int(end.X, end.Y)))
                                {
                                    _units[unitIndex].mainTarget = target.Key;
                                    p.blocks = _units[i].path.blocks;
                                    p.length = GetPathLength(p.points);
                                    return (_units[i].target, p);
                                }
                            }
                        }
                    }
                    Path path = new Path();
                    if (
                        path.Create(
                            ref unlimitedSearch,
                            unitGridPosition,
                            new BattleVector2Int(
                                _buildings[target.Key].battleBuidlingConfig.x,
                                _buildings[target.Key].battleBuidlingConfig.y
                            )
                        )
                    )
                    {
                        path.length = GetPathLength(path.points);
                        for (int i = 0; i < path.points.Count; i++)
                        {
                            for (int j = 0; j < blockedTiles.Count; j++)
                            {
                                if (
                                    blockedTiles[j].position.x == path.points[i].Location.X
                                    && blockedTiles[j].position.y == path.points[i].Location.Y
                                )
                                {
                                    if (
                                        blockedTiles[j].id == EBuildingType.wall
                                        && _buildings[blockedTiles[j].index].health > 0
                                    )
                                    {
                                        int t = blockedTiles[j].index;
                                        for (int k = path.points.Count - 1; k >= j; k--)
                                        {
                                            path.points.RemoveAt(k);
                                        }
                                        path.length = GetPathLength(path.points);
                                        return (t, path);
                                    }
                                    break;
                                }
                            }
                        }
                        break;
                    }
                }
            }
            return (-1, null);
        }

        private (int, Path) GetPathToBuilding(int buildingIndex, int unitIndex) // ok
        {
            // Nếu là tường, decor, obstacle thì return null
            if (_buildings[buildingIndex].battleBuidlingConfig.type == EBuildingType.wall) // || _buildings[buildingIndex].building.type == EBuildingType.decoration || _buildings[buildingIndex].building.type == EBuildingType.obstacle)
            {
                return (-1, null);
            }

            BattleVector2Int unitGridPosition = WorldToGridPosition(_units[unitIndex].position);

            // Get the x and y list of the building's surrounding tiles
            List<int> columns = new List<int>();
            List<int> rows = new List<int>();
            int startX = _buildings[buildingIndex].battleBuidlingConfig.x;
            int endX =
                _buildings[buildingIndex].battleBuidlingConfig.x
                + _buildings[buildingIndex].battleBuidlingConfig.columns
                - 1;
            int startY = _buildings[buildingIndex].battleBuidlingConfig.y;
            int endY =
                _buildings[buildingIndex].battleBuidlingConfig.y
                + _buildings[buildingIndex].battleBuidlingConfig.rows
                - 1;

            if (
                _units[unitIndex].unit.movement == Data.UnitMoveType.ground
                && _buildings[buildingIndex].battleBuidlingConfig.type == EBuildingType.wall
            )
            {
                startX--;
                startY--;
                endX++;
                endY++;
            }
            columns.Add(startX);
            columns.Add(endX);
            rows.Add(startY);
            rows.Add(endY);

            // Get the list of building's available surrounding tiles
            List<Path> tiles = new List<Path>();
            if (_units[unitIndex].unit.movement == UnitMoveType.ground)
            {
                #region With Walls Effect
                int closest = -1;
                float distance = 99999;
                int blocks = 999;
                for (int x = 0; x < columns.Count; x++)
                {
                    for (int y = 0; y < rows.Count; y++)
                    {
                        if (x >= 0 && y >= 0 && x < ConfigData.gridSize && y < ConfigData.gridSize)
                        {
                            Path path1 = new Path();
                            Path path2 = new Path();
                            path1.Create(
                                ref search,
                                new BattleVector2Int(columns[x], rows[y]),
                                unitGridPosition
                            );
                            path2.Create(
                                ref unlimitedSearch,
                                new BattleVector2Int(columns[x], rows[y]),
                                unitGridPosition
                            );
                            if (path1.points != null && path1.points.Count > 0)
                            {
                                path1.length = GetPathLength(path1.points);
                                int lengthToBlocks = (int)
                                    Math.Floor(
                                        path1.length
                                            / (
                                                ConfigData.battleTilesWorthOfOneWall
                                                * ConfigData.gridCellSize
                                            )
                                    );
                                if (path1.length < distance && lengthToBlocks <= blocks)
                                {
                                    closest = tiles.Count;
                                    distance = path1.length;
                                    blocks = lengthToBlocks;
                                }
                                tiles.Add(path1);
                            }
                            if (path2.points != null && path2.points.Count > 0)
                            {
                                path2.length = GetPathLength(path2.points);
                                for (int i = 0; i < path2.points.Count; i++)
                                {
                                    for (int j = 0; j < blockedTiles.Count; j++)
                                    {
                                        if (
                                            blockedTiles[j].position.x == path2.points[i].Location.X
                                            && blockedTiles[j].position.y
                                                == path2.points[i].Location.Y
                                        )
                                        {
                                            if (
                                                blockedTiles[j].id == EBuildingType.wall
                                                && _buildings[blockedTiles[j].index].health > 0
                                            )
                                            {
                                                path2.blocks.Add(blockedTiles[j]);
                                                // path2.blocksHealth += _buildings[blockedTiles[j].index].health;
                                            }
                                            break;
                                        }
                                    }
                                }
                                if (path2.length < distance && path2.blocks.Count <= blocks)
                                {
                                    closest = tiles.Count;
                                    distance = path1.length;
                                    blocks = path2.blocks.Count;
                                }
                                tiles.Add(path2);
                            }
                        }
                    }
                }
                tiles[closest].points.Reverse();
                if (tiles[closest].blocks.Count > 0)
                {
                    for (int i = 0; i < _units.Count; i++)
                    {
                        if (
                            _units[i].health <= 0
                            || _units[i].unit.movement != Data.UnitMoveType.ground
                            || i != unitIndex
                            || _units[i].target < 0
                            || _units[i].mainTarget != buildingIndex
                            || _units[i].mainTarget < 0
                            || _buildings[_units[i].mainTarget].battleBuidlingConfig.type
                                != EBuildingType.wall
                            || _buildings[_units[i].mainTarget].health <= 0
                        )
                        {
                            continue;
                        }
                        BattleVector2Int pos = WorldToGridPosition(_units[i].position);
                        List<Cell> points = search
                            .Find(
                                new AStarPathfinding.Vector2Int(pos.x, pos.y),
                                new AStarPathfinding.Vector2Int(
                                    unitGridPosition.x,
                                    unitGridPosition.y
                                )
                            )
                            .ToList();
                        if (
                            !Path.IsValid(
                                ref points,
                                new AStarPathfinding.Vector2Int(pos.x, pos.y),
                                new AStarPathfinding.Vector2Int(
                                    unitGridPosition.x,
                                    unitGridPosition.y
                                )
                            )
                        )
                        {
                            continue;
                        }
                        // float dis = GetPathLength(points, false);
                        if (id <= ConfigData.battleGroupWallAttackRadius)
                        {
                            AStarPathfinding.Vector2Int end = _units[i].path.points.Last().Location;
                            Path path = new Path();
                            if (path.Create(ref search, pos, new BattleVector2Int(end.X, end.Y)))
                            {
                                _units[unitIndex].mainTarget = buildingIndex;
                                path.blocks = _units[i].path.blocks;
                                path.length = GetPathLength(path.points);
                                return (_units[i].target, path);
                            }
                        }
                    }

                    Tile last = tiles[closest].blocks.Last();
                    for (int i = tiles[closest].points.Count - 1; i >= 0; i--)
                    {
                        int x = tiles[closest].points[i].Location.X;
                        int y = tiles[closest].points[i].Location.Y;
                        tiles[closest].points.RemoveAt(i);
                        if (x == last.position.x && y == last.position.y)
                        {
                            break;
                        }
                    }
                    _units[unitIndex].mainTarget = buildingIndex;
                    return (last.index, tiles[closest]);
                }
                else
                {
                    return (buildingIndex, tiles[closest]);
                }
                #endregion
            }
            else
            {
                #region Without Walls Effect
                int closest = -1;
                float distance = 99999;
                for (int x = 0; x < columns.Count; x++)
                {
                    for (int y = 0; y < rows.Count; y++)
                    {
                        if (
                            columns[x] >= 0
                            && rows[y] >= 0
                            && columns[x] < ConfigData.gridSize
                            && rows[y] < ConfigData.gridSize
                        )
                        {
                            Path path = new Path();
                            if (
                                path.Create(
                                    ref unlimitedSearch,
                                    new BattleVector2Int(columns[x], rows[y]),
                                    unitGridPosition
                                )
                            )
                            {
                                path.length = GetPathLength(path.points);
                                if (path.length < distance)
                                {
                                    closest = tiles.Count;
                                    distance = path.length;
                                }
                                tiles.Add(path);
                            }
                        }
                    }
                }
                if (closest >= 0)
                {
                    tiles[closest].points.Reverse();
                    return (buildingIndex, tiles[closest]);
                }
                #endregion
            }
            return (-1, null);
        }

        private static BattleVector2Int WorldToGridPosition(BattleVector2 position) // ok
        {
            return new BattleVector2Int(
                (int)Math.Floor(position.x / ConfigData.gridCellSize),
                (int)Math.Floor(position.y / ConfigData.gridCellSize)
            );
        }

        public void DropUnit()
        {
            var type = PanelManager.Instance.GetPanel<PanelBattle>().GetCurrentBuildingType();
            var pos = InputHandler.Instance.GetPointerPositionInMap();
            var posCell = BuildingManager.Instance.Grid.ConvertGridPos(pos);
            BattleManager.Instance.AddUnit(type, (int)posCell.x, (int)posCell.y);
        }

        public void Reset()
        {
            _isBattling = false;
            foreach (var b in _buildings)
            {
                if (b != null)
                {
                    Destroy(b.gameObject);
                }
            }
            _buildings.Clear();

            foreach (var u in _units)
            {
                Destroy(u.gameObject);
            }
            _units.Clear();

            foreach (var p in projectiles)
            {
                Destroy(p.gameObject);
            }
            projectiles.Clear();
        }

        public class UnitToAdd
        {
            public BattleUnit unit = null;
            public int x;
            public int y;
        }
    }
}
