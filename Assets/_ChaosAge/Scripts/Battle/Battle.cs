//using System;
//using System.Collections.Generic;
//using System.Linq;
//using AStarPathfinding;
//using ChaosAge.Config;
//using ChaosAge.Data;
//using UnityEngine;

//namespace ChaosAge.Battle
//{
//    public class Battle : MonoBehaviour
//    {
//        public long id = 0;
//        //public Data.BattleReport report = new Data.BattleReport();
//        public DateTime baseTime = DateTime.Now;
//        public int frameCount = 0;
//        public long defender = 0;
//        public long attacker = 0;
//        public List<BattleBuilding> _buildings = new List<BattleBuilding>();
//        public List<BattleUnitData> _units = new List<BattleUnitData>();
//        public List<UnitToAdd> _unitsToAdd = new List<UnitToAdd>();
//        private AStarPathfinding.Grid grid = null;
//        private AStarPathfinding.Grid unlimitedGrid = null;
//        private AStarSearch search = null;
//        private AStarSearch unlimitedSearch = null;
//        private List<Tile> blockedTiles = new List<Tile>();
//        private List<Projectile> projectiles = new List<Projectile>();
//        public float percentage = 0;

//        public delegate void UnitSpawned(long id);
//        public delegate void AttackCallback(long index, BattleVector2 target);
//        public delegate void IndexCallback(long index);
//        public delegate void FloatCallback(long index, float value);


//        public class Tile
//        {
//            public Tile(EBuildingType id, BattleVector2Int position, int index = -1)
//            {
//                this.id = id;
//                this.position = position;
//                this.index = index;
//            }
//            public EBuildingType id;
//            public BattleVector2Int position;
//            public int index = -1;
//        }

//        public class UnitToAdd
//        {
//            public BattleUnitData unit = null;
//            public int x;
//            public int y;
//            public UnitSpawned callback = null;
//            public AttackCallback attackCallback = null;
//            public IndexCallback dieCallback = null;
//            public FloatCallback damageCallback = null;
//            public FloatCallback healCallback = null;
//        }

//        //-========================================================
//        public void Initialize(List<BattleBuilding> buildings, DateTime time, AttackCallback attackCallback = null, FloatCallback destroyCallback = null, FloatCallback damageCallback = null) // ok
//        {
//            baseTime = time;
//            frameCount = 0;
//            percentage = 0;
//            _buildings = buildings;

//            // Thuật toán tìm đường
//            grid = new AStarPathfinding.Grid(ConfigData.gridSize, ConfigData.gridSize);
//            unlimitedGrid = new AStarPathfinding.Grid(ConfigData.gridSize, ConfigData.gridSize);
//            search = new AStarSearch(grid);
//            unlimitedSearch = new AStarSearch(unlimitedGrid);

//            for (int i = 0; i < _buildings.Count; i++)
//            {
//                _buildings[i].attackCallback = attackCallback;
//                _buildings[i].destroyCallback = destroyCallback;
//                _buildings[i].damageCallback = damageCallback;

//                _buildings[i].Initialize();
//                _buildings[i].worldCenterPosition = new BattleVector2((_buildings[i].building.x + (_buildings[i].building.columns / 2f)) * ConfigData.gridCellSize, (_buildings[i].building.y + (_buildings[i].building.rows / 2f)) * ConfigData.gridCellSize);


//                // blockTiles: Vẫn đi được ở rìa công trình
//                int startX = _buildings[i].building.x;
//                int endX = _buildings[i].building.x + _buildings[i].building.columns;

//                int startY = _buildings[i].building.y;
//                int endY = _buildings[i].building.y + _buildings[i].building.rows;

//                if (_buildings[i].building.type != EBuildingType.wall && _buildings[i].building.columns > 1 && _buildings[i].building.rows > 1)
//                {
//                    startX++;
//                    startY++;
//                    endX--;
//                    endY--;
//                    if (endX <= startX || endY <= startY)
//                    {
//                        continue;
//                    }
//                }

//                for (int x = startX; x < endX; x++)
//                {
//                    for (int y = startY; y < endY; y++)
//                    {
//                        grid[x, y].Blocked = true;
//                        blockedTiles.Add(new Tile(_buildings[i].building.type, new BattleVector2Int(x, y), i));
//                    }
//                }
//            }
//        }


//        public void AddUnit(UnitData unit, int x, int y, UnitSpawned callback = null, AttackCallback attackCallback = null, IndexCallback dieCallback = null, FloatCallback damageCallback = null, FloatCallback healCallback = null) // ok
//        {
//            UnitToAdd unitToAdd = new UnitToAdd();
//            unitToAdd.callback = callback;
//            var battleUnit = new BattleUnitData();
//            battleUnit.attackCallback = attackCallback;
//            battleUnit.dieCallback = dieCallback;
//            battleUnit.damageCallback = damageCallback;
//            battleUnit.healCallback = healCallback;
//            battleUnit.unit = unit;
//            battleUnit.Initialize(x, y);
//            battleUnit.health = unit.health;
//            unitToAdd.unit = battleUnit;
//            unitToAdd.x = x;
//            unitToAdd.y = y;
//            _unitsToAdd.Add(unitToAdd);
//            /*
//            if(time > updateTime)
//            {
//                updateTime = time;
//            }
//            */
//        }


//        public void ExecuteFrame()
//        {
//            int addIndex = _units.Count;
//            for (int i = _unitsToAdd.Count - 1; i >= 0; i--)
//            {
//                if (CanAddUnit(_unitsToAdd[i].x, _unitsToAdd[i].y))
//                {
//                    _units.Insert(addIndex, _unitsToAdd[i].unit);
//                    if (_unitsToAdd[i].callback != null)
//                    {
//                        _unitsToAdd[i].callback.Invoke((long)_unitsToAdd[i].unit.unit.type);
//                    }
//                }
//                _unitsToAdd.RemoveAt(i);
//            }

//            for (int i = 0; i < _buildings.Count; i++)
//            {
//                if (_buildings[i].building.targetType != Data.BuildingTargetType.none && _buildings[i].health > 0)
//                {
//                    HandleBuilding(i, ConfigData.battleFrameRate);
//                }
//            }

//            for (int i = 0; i < _units.Count; i++)
//            {
//                if (_units[i].health > 0)
//                {
//                    HandleUnit(i, ConfigData.battleFrameRate);
//                }
//            }

//            if (projectiles.Count > 0)
//            {
//                for (int i = projectiles.Count - 1; i >= 0; i--)
//                {
//                    projectiles[i].timer -= ConfigData.battleFrameRate;
//                    if (projectiles[i].timer <= 0)
//                    {
//                        // hồi máu or gây damage
//                        if (projectiles[i].type == TargetType.unit)
//                        {
//                            // hồi máu
//                            if (projectiles[i].heal)
//                            {
//                                _units[projectiles[i].target].Heal(projectiles[i].damage);

//                                // hồi máu trong phạm vi nổ (splash)
//                                // Không hồi màu cho đối tượng bay
//                                for (int j = 0; j < _units.Count; j++)
//                                {
//                                    if (_units[j].health <= 0 || j == projectiles[i].target || _units[j].unit.movement == UnitMoveType.fly)
//                                    {
//                                        continue;
//                                    }
//                                    float distance = BattleVector2.Distance(_units[j].position, _units[projectiles[i].target].position);
//                                    if (distance < projectiles[i].splash * ConfigData.gridCellSize)
//                                    {
//                                        _units[j].Heal(projectiles[i].damage * (1f - (distance / projectiles[i].splash * ConfigData.gridCellSize)));
//                                    }
//                                }
//                            }
//                            // GÂy damage, tương tự
//                            else
//                            {
//                                _units[projectiles[i].target].TakeDamage(projectiles[i].damage);
//                                if (projectiles[i].splash > 0)
//                                {
//                                    for (int j = 0; j < _units.Count; j++)
//                                    {
//                                        if (j != projectiles[i].target)
//                                        {
//                                            float distance = BattleVector2.Distance(_units[j].position, _units[projectiles[i].target].position);
//                                            if (distance < projectiles[i].splash * ConfigData.gridSize)
//                                            {
//                                                _units[j].TakeDamage(projectiles[i].damage * (1f - (distance / projectiles[i].splash * ConfigData.gridCellSize)));
//                                            }
//                                        }
//                                    }
//                                }
//                            }
//                        }
//                        else
//                        {
//                            _buildings[projectiles[i].target].TakeDamage(projectiles[i].damage, ref grid, ref blockedTiles, ref percentage);
//                        }
//                        projectiles.RemoveAt(i);
//                    }
//                }
//            }

//            frameCount++;
//        }


//        public bool CanAddUnit(int x, int y) // ok
//        {
//            for (int i = 0; i < _buildings.Count; i++)
//            {
//                if (_buildings[i].health <= 0)
//                {
//                    continue;
//                }

//                int startX = _buildings[i].building.x;
//                int endX = _buildings[i].building.x + _buildings[i].building.columns;

//                int startY = _buildings[i].building.y;
//                int endY = _buildings[i].building.y + _buildings[i].building.rows;

//                for (int x2 = startX; x2 < endX; x2++)
//                {
//                    for (int y2 = startY; y2 < endY; y2++)
//                    {
//                        if (x == x2 && y == y2)
//                        {
//                            return false;
//                        }
//                    }
//                }
//            }
//            return true;
//        }

//        #region Handle building

//        private void HandleBuilding(int index, double deltaTime)
//        {
//            var idxUnit = _buildings[index].target;
//            if (idxUnit >= 0)
//            {
//                // Nếu the building's target is dead  or Không nằm trong phạm vi bắn
//                if (_units[idxUnit].health <= 0 || !IsUnitInRange(idxUnit, index) || (_units[idxUnit].unit.movement == UnitMoveType.underground && _units[idxUnit].path != null))
//                {
//                    // If the building's target is dead or not in range then remove it as target
//                    idxUnit = -1;
//                }
//                else
//                {
//                    // Building has a target
//                    _buildings[index].attackTimer += deltaTime;
//                    int attacksCount = (int)Math.Floor(_buildings[index].attackTimer / _buildings[index].building.speed);
//                    if (attacksCount > 0)
//                    {
//                        _buildings[index].attackTimer -= (attacksCount * _buildings[index].building.speed);
//                        for (int i = 1; i <= attacksCount; i++)
//                        {
//                            if (_buildings[index].building.radius > 0 && _buildings[index].building.rangedSpeed > 0)
//                            {
//                                float distance = BattleVector2.Distance(_units[idxUnit].position, _buildings[index].worldCenterPosition);
//                                Projectile projectile = new Projectile();
//                                projectile.type = TargetType.unit;
//                                projectile.target = idxUnit;
//                                projectile.timer = distance / _buildings[index].building.rangedSpeed;
//                                projectile.damage = _buildings[index].building.damage;
//                                projectile.splash = _buildings[index].building.splashRange;
//                                projectiles.Add(projectile);
//                            }
//                            else
//                            {
//                                _units[idxUnit].TakeDamage(_buildings[index].building.damage);
//                                if (_buildings[index].building.splashRange > 0)
//                                {
//                                    for (int j = 0; j < _units.Count; j++)
//                                    {
//                                        if (j != idxUnit)
//                                        {
//                                            float distance = BattleVector2.Distance(_units[j].position, _units[idxUnit].position);
//                                            if (distance < _buildings[index].building.splashRange * ConfigData.gridCellSize)
//                                            {
//                                                _units[j].TakeDamage(_buildings[index].building.damage * (1f - (distance / _buildings[index].building.splashRange * ConfigData.gridCellSize)));
//                                            }
//                                        }
//                                    }
//                                }
//                            }
//                            if (_buildings[index].attackCallback != null)
//                            {
//                                _buildings[index].attackCallback.Invoke((long)_buildings[index].building.type, _units[_buildings[index].target].position);
//                            }
//                        }
//                    }
//                }
//            }
//            if (idxUnit < 0)
//            {
//                // Find a new target for this building
//                if (FindTargetForBuilding(index))
//                {
//                    HandleBuilding(index, deltaTime);
//                }
//            }
//        }
//        private bool FindTargetForBuilding(int index) // ok
//        {
//            for (int i = 0; i < _units.Count; i++)
//            {
//                if (_units[i].health <= 0 || _units[i].unit.movement == Data.UnitMoveType.underground && _units[i].path != null)
//                {
//                    continue;
//                }

//                if (_buildings[index].building.targetType == Data.BuildingTargetType.ground && _units[i].unit.movement == Data.UnitMoveType.fly)
//                {
//                    continue;
//                }

//                if (_buildings[index].building.targetType == Data.BuildingTargetType.air && _units[i].unit.movement != Data.UnitMoveType.fly)
//                {
//                    continue;
//                }

//                if (IsUnitInRange(i, index))
//                {
//                    _buildings[index].attackTimer = 0;
//                    _buildings[index].target = i;
//                    return true;
//                }
//            }
//            return false;
//        }

//        private bool IsUnitInRange(int unitIndex, int buildingIndex) // ok
//        {
//            float distance = BattleVector2.Distance(_buildings[buildingIndex].worldCenterPosition, _units[unitIndex].position);
//            if (distance <= _buildings[buildingIndex].building.radius)
//            {
//                if (_buildings[buildingIndex].building.blindRange > 0 && distance <= _buildings[buildingIndex].building.blindRange)
//                {
//                    return false;
//                }
//                return true;
//            }
//            return false;
//        }
//        #endregion

//        private void HandleUnit(int index, double deltaTime) // ok
//        {
//            if (_units[index].unit.type == EUnitType.healer)
//            {
//                //if (_units[index].target >= 0 && _units[_units[index].target].health <= 0 || _units[_units[index].target].health >= _units[_units[index].target].unit.health)
//                //{
//                //    _units[index].target = -1;
//                //}
//                //if (_units[index].target < 0)
//                //{
//                //    FindHealerTargets(index);
//                //}
//                //if (_units[index].target >= 0)
//                //{
//                //    float distance = BattleVector2.Distance(_units[index].position, _units[_units[index].target].position);
//                //    if (distance + ConfigData.gridSize <= _units[index].unit.attackRange)
//                //    {
//                //        if (_units[index].unit.attackRange > 0 && _units[index].unit.rangedSpeed > 0)
//                //        {
//                //            Projectile projectile = new Projectile();
//                //            projectile.type = TargetType.unit;
//                //            projectile.target = _units[index].target;
//                //            projectile.timer = distance / _units[index].unit.rangedSpeed;
//                //            projectile.damage = _units[index].unit.damage;
//                //            projectile.heal = true;
//                //            projectiles.Add(projectile);
//                //        }
//                //        else
//                //        {
//                //            _units[_units[index].target].Heal(_units[index].unit.damage);
//                //            for (int i = 0; i < _units.Count; i++)
//                //            {
//                //                if (_units[i].health <= 0 || i == index || i == _units[index].target)
//                //                {
//                //                    continue;
//                //                }
//                //                float d = BattleVector2.Distance(_units[i].position, _units[_units[index].target].position);
//                //                if (d < _units[i].unit.splashRange * ConfigData.gridSize)
//                //                {
//                //                    float amount = _units[index].unit.damage * (1f - (d / _units[i].unit.splashRange * ConfigData.gridSize));
//                //                    _units[i].Heal(amount);
//                //                }
//                //            }
//                //        }
//                //    }
//                //    else
//                //    {
//                //        // Move the healer
//                //        float d = (float)deltaTime * _units[index].unit.moveSpeed * ConfigData.gridSize;
//                //        _units[index].position = BattleVector2.LerpUnclamped(_units[index].position, _units[_units[index].target].position, d / distance);
//                //        return;
//                //    }
//                //}
//            }
//            else
//            {
//                if (_units[index].path != null)
//                {

//                    if (_units[index].target < 0 || (_units[index].target >= 0 && _buildings[_units[index].target].health <= 0))
//                    {
//                        _units[index].path = null;
//                        _units[index].target = -1;
//                    }
//                    else
//                    {
//                        double remainedTime = _units[index].pathTime - _units[index].pathTraveledTime;

//                        if (remainedTime >= deltaTime)
//                        {
//                            _units[index].pathTraveledTime += deltaTime;
//                            deltaTime = 0;
//                        }
//                        else
//                        {
//                            _units[index].pathTraveledTime = _units[index].pathTime;
//                            deltaTime -= remainedTime;
//                        }

//                        // Update unit's position based on path
//                        _units[index].position = GetPathPosition(_units[index].path.points, (float)(_units[index].pathTraveledTime / _units[index].pathTime));

//                        // Check if target is in range
//                        if (_units[index].unit.attackRange > 0 && IsBuildingInRange(index, _units[index].target))
//                        {
//                            _units[index].path = null;
//                        }
//                        else
//                        {
//                            // check if unit reached the end of the path
//                            BattleVector2 targetPosition = GridToWorldPosition(new BattleVector2Int(_units[index].path.points.Last().Location.X, _units[index].path.points.Last().Location.Y));
//                            float distance = BattleVector2.Distance(_units[index].position, targetPosition);
//                            if (distance <= ConfigData.gridSize * 0.05f)
//                            {
//                                _units[index].position = targetPosition;
//                                _units[index].path = null;
//                            }
//                        }
//                    }
//                }

//                if (_units[index].target >= 0)
//                {
//                    if (_buildings[_units[index].target].health > 0)
//                    {
//                        if (_buildings[_units[index].target].building.type == EBuildingType.wall && _units[index].mainTarget >= 0 && _buildings[_units[index].mainTarget].health <= 0)
//                        {
//                            _units[index].target = -1;
//                        }
//                        else
//                        {
//                            if (_units[index].path == null)
//                            {
//                                // Attack the target
//                                float multiplier = 1;
//                                if (_units[index].unit.priority != TargetPriority.all || _units[index].unit.priority != TargetPriority.none)
//                                {
//                                    switch (_buildings[_units[index].target].building.type)
//                                    {
//                                        case EBuildingType.townhall:
//                                        case EBuildingType.goldmine:
//                                        case EBuildingType.goldstorage:
//                                        case EBuildingType.elixirmine:
//                                        case EBuildingType.elixirstorage:
//                                        case EBuildingType.darkelixirmine:
//                                        case EBuildingType.darkelixirstorage:
//                                            if (_units[index].unit.priority != Data.TargetPriority.resources)
//                                            {
//                                                multiplier = _units[index].unit.priorityMultiplier;
//                                            }
//                                            break;
//                                        case EBuildingType.wall:
//                                            if (_units[index].unit.priority != Data.TargetPriority.walls)
//                                            {
//                                                multiplier = _units[index].unit.priorityMultiplier;
//                                            }
//                                            break;
//                                        case EBuildingType.cannon:
//                                        case EBuildingType.archertower:
//                                        case EBuildingType.mortor:
//                                        case EBuildingType.airdefense:
//                                        case EBuildingType.wizardtower:
//                                        case EBuildingType.hiddentesla:
//                                        case EBuildingType.bombtower:
//                                        case EBuildingType.xbow:
//                                        case EBuildingType.infernotower:
//                                            if (_units[index].unit.priority != TargetPriority.defenses)
//                                            {
//                                                multiplier = _units[index].unit.priorityMultiplier;
//                                            }
//                                            break;
//                                    }
//                                }
//                                _units[index].attackTimer += deltaTime;
//                                if (_units[index].attackTimer >= _units[index].unit.attackSpeed)
//                                {
//                                    float distance = BattleVector2.Distance(_units[index].position, _buildings[_units[index].target].worldCenterPosition);
//                                    if (_units[index].unit.attackRange > 0 && _units[index].unit.rangedSpeed > 0)
//                                    {
//                                        Projectile projectile = new Projectile();
//                                        projectile.type = TargetType.building;
//                                        projectile.target = _units[index].target;
//                                        projectile.timer = distance / _units[index].unit.rangedSpeed;
//                                        projectile.damage = _units[index].unit.damage * multiplier;
//                                        projectiles.Add(projectile);
//                                    }
//                                    else
//                                    {
//                                        _buildings[_units[index].target].TakeDamage(_units[index].unit.damage * multiplier, ref grid, ref blockedTiles, ref percentage);
//                                    }
//                                    _units[index].attackTimer -= _units[index].unit.attackSpeed;
//                                    if (_units[index].attackCallback != null)
//                                    {
//                                        _units[index].attackCallback.Invoke(index, _buildings[_units[index].target].worldCenterPosition);
//                                    }
//                                    if (_units[index].unit.type == EUnitType.wallbreaker)
//                                    {
//                                        _units[index].TakeDamage(_units[index].health);
//                                    }
//                                }
//                            }
//                        }
//                    }
//                    else
//                    {
//                        _units[index].target = -1;
//                    }
//                }

//                if (_units[index].target < 0)
//                {
//                    // Find a target and path
//                    FindTargets(index, _units[index].unit.priority);
//                    if (deltaTime > 0 && _units[index].target >= 0)
//                    {
//                        HandleUnit(index, deltaTime);
//                    }
//                }
//            }
//        }



//        public static BattleVector2 GridToWorldPosition(BattleVector2Int position) // ok
//        {
//            return new BattleVector2(position.x * ConfigData.gridSize + ConfigData.gridSize / 2f, position.y * ConfigData.gridSize + ConfigData.gridSize / 2f);
//        }

//        private static BattleVector2 GetPathPosition(IList<Cell> path, float t) // ok
//        {
//            if (t < 0) { t = 0; }
//            if (t > 1) { t = 1; }
//            float totalLength = GetPathLength(path);
//            float length = 0;
//            if (path != null && path.Count > 1)
//            {
//                for (int i = 1; i < path.Count; i++)
//                {
//                    BattleVector2Int a = new BattleVector2Int(path[i - 1].Location.X, path[i - 1].Location.Y);
//                    BattleVector2Int b = new BattleVector2Int(path[i].Location.X, path[i].Location.Y);
//                    float l = BattleVector2.Distance(a, b) * ConfigData.gridSize;
//                    float p = (length + l) / totalLength;
//                    if (p >= t)
//                    {
//                        t = (t - (length / totalLength)) / (p - (length / totalLength));
//                        return BattleVector2.LerpUnclamped(GridToWorldPosition(a), GridToWorldPosition(b), t);
//                    }
//                    length += l;
//                }
//            }
//            return GridToWorldPosition(new BattleVector2Int(path[0].Location.X, path[0].Location.Y));
//        }

//        private static BattleVector2Int WorldToGridPosition(BattleVector2 position) // ok
//        {
//            return new BattleVector2Int((int)Math.Floor(position.x / ConfigData.gridSize), (int)Math.Floor(position.y / ConfigData.gridSize));
//        }

//        private void FindHealerTargets(int index) // ok
//        {
//            int target = -1;
//            float distance = 99999;
//            // int unitsCover = 0;
//            for (int i = 0; i < _units.Count; i++)
//            {
//                if (_units[i].health <= 0 || i == index || _units[i].health >= _units[i].unit.health || _units[i].unit.movement == Data.UnitMoveType.fly)
//                {
//                    continue;
//                }
//                float d = BattleVector2.Distance(_units[i].position, _units[index].position);
//                if (d < distance)
//                {
//                    target = i;
//                    distance = d;
//                }
//            }
//            if (target >= 0)
//            {
//                _units[index].AssignHealerTarget(target, distance + ConfigData.gridSize);
//            }
//        }

//        private void ListUnitTargets(int index, Data.TargetPriority priority) // ok
//        {
//            _units[index].resourceTargets.Clear();
//            _units[index].defenceTargets.Clear();
//            _units[index].otherTargets.Clear();
//            if (priority == Data.TargetPriority.walls)
//            {
//                priority = Data.TargetPriority.all;
//            }
//            for (int i = 0; i < _buildings.Count; i++)
//            {
//                if (_buildings[i].health <= 0 || priority != _units[index].unit.priority || !IsBuildingCanBeAttacked(_buildings[i].building.type))
//                {
//                    continue;
//                }
//                float distance = BattleVector2.Distance(_buildings[i].worldCenterPosition, _units[index].position);
//                switch (_buildings[i].building.type)
//                {
//                    case EBuildingType.townhall:
//                    case EBuildingType.elixirmine:
//                    case EBuildingType.elixirstorage:
//                    case EBuildingType.darkelixirmine:
//                    case EBuildingType.darkelixirstorage:
//                    case EBuildingType.goldmine:
//                    case EBuildingType.goldstorage:
//                        _units[index].resourceTargets.Add(i, distance);
//                        break;
//                    case EBuildingType.cannon:
//                    case EBuildingType.archertower:
//                    case EBuildingType.mortor:
//                    case EBuildingType.airdefense:
//                    case EBuildingType.wizardtower:
//                    case EBuildingType.hiddentesla:
//                    case EBuildingType.bombtower:
//                    case EBuildingType.xbow:
//                    case EBuildingType.infernotower:
//                        _units[index].defenceTargets.Add(i, distance);
//                        break;
//                    case EBuildingType.wall:
//                        // Don't include
//                        break;
//                    default:
//                        _units[index].otherTargets.Add(i, distance);
//                        break;
//                }
//            }
//        }

//        public static bool IsBuildingCanBeAttacked(EBuildingType id)
//        {
//            switch (id)
//            {
//                case EBuildingType.obstacle:
//                case EBuildingType.decoration:
//                case EBuildingType.boomb:
//                case EBuildingType.springtrap:
//                case EBuildingType.airbomb:
//                case EBuildingType.giantbomb:
//                case EBuildingType.seekingairmine:
//                case EBuildingType.skeletontrap:
//                    return false;
//            }
//            return true;
//        }

//        private void FindTargets(int index, Data.TargetPriority priority) // ok
//        {
//            ListUnitTargets(index, priority);
//            if (priority == Data.TargetPriority.defenses)
//            {
//                if (_units[index].defenceTargets.Count > 0)
//                {
//                    AssignTarget(index, ref _units[index].defenceTargets);
//                }
//                else
//                {
//                    FindTargets(index, Data.TargetPriority.all);
//                    return;
//                }
//            }
//            else if (priority == Data.TargetPriority.resources)
//            {
//                if (_units[index].resourceTargets.Count > 0)
//                {
//                    AssignTarget(index, ref _units[index].resourceTargets);
//                }
//                else
//                {
//                    FindTargets(index, Data.TargetPriority.all);
//                    return;
//                }
//            }
//            else if (priority == Data.TargetPriority.all || priority == Data.TargetPriority.walls)
//            {
//                Dictionary<int, float> temp = _units[index].GetAllTargets();
//                if (temp.Count > 0)
//                {
//                    AssignTarget(index, ref temp, priority == Data.TargetPriority.walls);
//                }
//                else
//                {
//                    return;
//                }
//            }
//        }

//        private void AssignTarget(int index, ref Dictionary<int, float> targets, bool wallsPriority = false) // ok
//        {
//            if (wallsPriority)
//            {
//                var wallPath = GetPathToWall(index, ref targets);
//                if (wallPath.Item1 >= 0)
//                {
//                    _units[index].AssignTarget(wallPath.Item1, wallPath.Item2);
//                    return;
//                }
//            }

//            int min = targets.Aggregate((a, b) => a.Value < b.Value ? a : b).Key;
//            var path = GetPathToBuilding(min, index);
//            if (path.Item1 >= 0)
//            {
//                _units[index].AssignTarget(path.Item1, path.Item2);
//            }
//        }

//        private (int, Path) GetPathToWall(int unitIndex, ref Dictionary<int, float> targets) // ok
//        {
//            BattleVector2Int unitGridPosition = WorldToGridPosition(_units[unitIndex].position);
//            List<Path> tiles = new List<Path>();
//            foreach (var target in (targets.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value)))
//            {
//                List<Cell> points = search.Find(new AStarPathfinding.Vector2Int(_buildings[target.Key].building.x, _buildings[target.Key].building.y), new AStarPathfinding.Vector2Int(unitGridPosition.x, unitGridPosition.y)).ToList();
//                if (Path.IsValid(ref points, new AStarPathfinding.Vector2Int(_buildings[target.Key].building.x, _buildings[target.Key].building.y), new AStarPathfinding.Vector2Int(unitGridPosition.x, unitGridPosition.y)))
//                {
//                    continue;
//                }
//                else
//                {
//                    for (int i = 0; i < _units.Count; i++)
//                    {
//                        if (_units[i].health <= 0 || _units[i].unit.movement != Data.UnitMoveType.ground || i != unitIndex || _units[i].target < 0 || _units[i].mainTarget != target.Key || _units[i].mainTarget < 0 || _buildings[_units[i].mainTarget].building.type != EBuildingType.wall || _buildings[_units[i].mainTarget].health <= 0)
//                        {
//                            continue;
//                        }
//                        BattleVector2Int pos = WorldToGridPosition(_units[i].position);
//                        List<Cell> pts = search.Find(new AStarPathfinding.Vector2Int(pos.x, pos.y), new AStarPathfinding.Vector2Int(unitGridPosition.x, unitGridPosition.y)).ToList();
//                        if (Path.IsValid(ref pts, new AStarPathfinding.Vector2Int(pos.x, pos.y), new AStarPathfinding.Vector2Int(unitGridPosition.x, unitGridPosition.y)))
//                        {
//                            float dis = GetPathLength(pts, false);
//                            if (id <= ConfigData.battleGroupWallAttackRadius)
//                            {
//                                AStarPathfinding.Vector2Int end = _units[i].path.points.Last().Location;
//                                Path p = new Path();
//                                if (p.Create(ref search, pos, new BattleVector2Int(end.X, end.Y)))
//                                {
//                                    _units[unitIndex].mainTarget = target.Key;
//                                    p.blocks = _units[i].path.blocks;
//                                    p.length = GetPathLength(p.points);
//                                    return (_units[i].target, p);
//                                }
//                            }
//                        }
//                    }
//                    Path path = new Path();
//                    if (path.Create(ref unlimitedSearch, unitGridPosition, new BattleVector2Int(_buildings[target.Key].building.x, _buildings[target.Key].building.y)))
//                    {
//                        path.length = GetPathLength(path.points);
//                        for (int i = 0; i < path.points.Count; i++)
//                        {
//                            for (int j = 0; j < blockedTiles.Count; j++)
//                            {
//                                if (blockedTiles[j].position.x == path.points[i].Location.X && blockedTiles[j].position.y == path.points[i].Location.Y)
//                                {
//                                    if (blockedTiles[j].id == EBuildingType.wall && _buildings[blockedTiles[j].index].health > 0)
//                                    {
//                                        int t = blockedTiles[j].index;
//                                        for (int k = path.points.Count - 1; k >= j; k--)
//                                        {
//                                            path.points.RemoveAt(k);
//                                        }
//                                        path.length = GetPathLength(path.points);
//                                        return (t, path);
//                                    }
//                                    break;
//                                }
//                            }
//                        }
//                        break;
//                    }
//                }
//            }
//            return (-1, null);
//        }

//        private (int, Path) GetPathToBuilding(int buildingIndex, int unitIndex) // ok
//        {
//            if (_buildings[buildingIndex].building.type == EBuildingType.wall || _buildings[buildingIndex].building.type == EBuildingType.decoration || _buildings[buildingIndex].building.type == EBuildingType.obstacle)
//            {
//                return (-1, null);
//            }

//            BattleVector2Int unitGridPosition = WorldToGridPosition(_units[unitIndex].position);

//            // Get the x and y list of the building's surrounding tiles
//            List<int> columns = new List<int>();
//            List<int> rows = new List<int>();
//            int startX = _buildings[buildingIndex].building.x;
//            int endX = _buildings[buildingIndex].building.x + _buildings[buildingIndex].building.columns - 1;
//            int startY = _buildings[buildingIndex].building.y;
//            int endY = _buildings[buildingIndex].building.y + _buildings[buildingIndex].building.rows - 1;
//            if (_units[unitIndex].unit.movement == Data.UnitMoveType.ground && _buildings[buildingIndex].building.type == EBuildingType.wall)
//            {
//                startX--;
//                startY--;
//                endX++;
//                endY++;
//            }
//            columns.Add(startX);
//            columns.Add(endX);
//            rows.Add(startY);
//            rows.Add(endY);

//            // Get the list of building's available surrounding tiles
//            List<Path> tiles = new List<Path>();
//            if (_units[unitIndex].unit.movement == Data.UnitMoveType.ground)
//            {
//                #region With Walls Effect
//                int closest = -1;
//                float distance = 99999;
//                int blocks = 999;
//                for (int x = 0; x < columns.Count; x++)
//                {
//                    for (int y = 0; y < rows.Count; y++)
//                    {
//                        if (x >= 0 && y >= 0 && x < ConfigData.gridSize && y < ConfigData.gridSize)
//                        {
//                            Path path1 = new Path();
//                            Path path2 = new Path();
//                            path1.Create(ref search, new BattleVector2Int(columns[x], rows[y]), unitGridPosition);
//                            path2.Create(ref unlimitedSearch, new BattleVector2Int(columns[x], rows[y]), unitGridPosition);
//                            if (path1.points != null && path1.points.Count > 0)
//                            {
//                                path1.length = GetPathLength(path1.points);
//                                int lengthToBlocks = (int)Math.Floor(path1.length / (ConfigData.battleTilesWorthOfOneWall * ConfigData.gridSize));
//                                if (path1.length < distance && lengthToBlocks <= blocks)
//                                {
//                                    closest = tiles.Count;
//                                    distance = path1.length;
//                                    blocks = lengthToBlocks;
//                                }
//                                tiles.Add(path1);
//                            }
//                            if (path2.points != null && path2.points.Count > 0)
//                            {
//                                path2.length = GetPathLength(path2.points);
//                                for (int i = 0; i < path2.points.Count; i++)
//                                {
//                                    for (int j = 0; j < blockedTiles.Count; j++)
//                                    {
//                                        if (blockedTiles[j].position.x == path2.points[i].Location.X && blockedTiles[j].position.y == path2.points[i].Location.Y)
//                                        {
//                                            if (blockedTiles[j].id == EBuildingType.wall && _buildings[blockedTiles[j].index].health > 0)
//                                            {
//                                                path2.blocks.Add(blockedTiles[j]);
//                                                // path2.blocksHealth += _buildings[blockedTiles[j].index].health;
//                                            }
//                                            break;
//                                        }
//                                    }
//                                }
//                                if (path2.length < distance && path2.blocks.Count <= blocks)
//                                {
//                                    closest = tiles.Count;
//                                    distance = path1.length;
//                                    blocks = path2.blocks.Count;
//                                }
//                                tiles.Add(path2);
//                            }
//                        }
//                    }
//                }
//                tiles[closest].points.Reverse();
//                if (tiles[closest].blocks.Count > 0)
//                {
//                    for (int i = 0; i < _units.Count; i++)
//                    {
//                        if (_units[i].health <= 0 || _units[i].unit.movement != Data.UnitMoveType.ground || i != unitIndex || _units[i].target < 0 || _units[i].mainTarget != buildingIndex || _units[i].mainTarget < 0 || _buildings[_units[i].mainTarget].building.type != EBuildingType.wall || _buildings[_units[i].mainTarget].health <= 0)
//                        {
//                            continue;
//                        }
//                        BattleVector2Int pos = WorldToGridPosition(_units[i].position);
//                        List<Cell> points = search.Find(new AStarPathfinding.Vector2Int(pos.x, pos.y), new AStarPathfinding.Vector2Int(unitGridPosition.x, unitGridPosition.y)).ToList();
//                        if (!Path.IsValid(ref points, new AStarPathfinding.Vector2Int(pos.x, pos.y), new AStarPathfinding.Vector2Int(unitGridPosition.x, unitGridPosition.y)))
//                        {
//                            continue;
//                        }
//                        // float dis = GetPathLength(points, false);
//                        if (id <= ConfigData.battleGroupWallAttackRadius)
//                        {
//                            AStarPathfinding.Vector2Int end = _units[i].path.points.Last().Location;
//                            Path path = new Path();
//                            if (path.Create(ref search, pos, new BattleVector2Int(end.X, end.Y)))
//                            {
//                                _units[unitIndex].mainTarget = buildingIndex;
//                                path.blocks = _units[i].path.blocks;
//                                path.length = GetPathLength(path.points);
//                                return (_units[i].target, path);
//                            }
//                        }
//                    }

//                    Tile last = tiles[closest].blocks.Last();
//                    for (int i = tiles[closest].points.Count - 1; i >= 0; i--)
//                    {
//                        int x = tiles[closest].points[i].Location.X;
//                        int y = tiles[closest].points[i].Location.Y;
//                        tiles[closest].points.RemoveAt(i);
//                        if (x == last.position.x && y == last.position.y)
//                        {
//                            break;
//                        }
//                    }
//                    _units[unitIndex].mainTarget = buildingIndex;
//                    return (last.index, tiles[closest]);
//                }
//                else
//                {
//                    return (buildingIndex, tiles[closest]);
//                }
//                #endregion
//            }
//            else
//            {
//                #region Without Walls Effect
//                int closest = -1;
//                float distance = 99999;
//                for (int x = 0; x < columns.Count; x++)
//                {
//                    for (int y = 0; y < rows.Count; y++)
//                    {
//                        if (columns[x] >= 0 && rows[y] >= 0 && columns[x] < ConfigData.gridSize && rows[y] < ConfigData.gridSize)
//                        {
//                            Path path = new Path();
//                            if (path.Create(ref unlimitedSearch, new BattleVector2Int(columns[x], rows[y]), unitGridPosition))
//                            {
//                                path.length = GetPathLength(path.points);
//                                if (path.length < distance)
//                                {
//                                    closest = tiles.Count;
//                                    distance = path.length;
//                                }
//                                tiles.Add(path);
//                            }
//                        }
//                    }
//                }
//                if (closest >= 0)
//                {
//                    tiles[closest].points.Reverse();
//                    return (buildingIndex, tiles[closest]);
//                }
//                #endregion
//            }
//            return (-1, null);
//        }

//        private bool IsBuildingInRange(int unitIndex, int buildingIndex) // ok
//        {
//            for (int x = _buildings[buildingIndex].building.x; x < _buildings[buildingIndex].building.x + _buildings[buildingIndex].building.columns; x++)
//            {
//                for (int y = _buildings[buildingIndex].building.y; y < _buildings[buildingIndex].building.y + _buildings[buildingIndex].building.columns; y++)
//                {
//                    float distance = BattleVector2.Distance(GridToWorldPosition(new BattleVector2Int(x, y)), _units[unitIndex].position);
//                    if (distance <= _units[unitIndex].unit.attackRange)
//                    {
//                        return true;
//                    }
//                }
//            }
//            return false;
//        }

//        private static float GetPathLength(IList<Cell> path, bool includeCellSize = true) // ok
//        {
//            float length = 0;
//            if (path != null && path.Count > 1)
//            {
//                for (int i = 1; i < path.Count; i++)
//                {
//                    length += BattleVector2.Distance(new BattleVector2(path[i - 1].Location.X, path[i - 1].Location.Y), new BattleVector2(path[i].Location.X, path[i].Location.Y));
//                }
//            }
//            if (includeCellSize)
//            {
//                length *= ConfigData.gridCellSize;
//            }
//            return length;
//        }
//    }
//}
