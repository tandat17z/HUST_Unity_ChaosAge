using System;
using System.Collections.Generic;
using AStarPathfinding;
using ChaosAge.Battle;
using ChaosAge.Config;
using ChaosAge.Data;
using DatSystem.utils;
using Sirenix.OdinInspector;
using UnityEngine;
using Grid = AStarPathfinding.Grid;

namespace ChaosAge.manager
{
    public class BattleManager : Singleton<BattleManager>
    {
        protected override void OnAwake()
        {

        }

        public long id = 0;
        public DateTime baseTime;
        public int frameCount = 0;
        public long defender = 0;
        public long attacker = 0;
        public List<BattleBuilding> _buildings = new List<BattleBuilding>();
        public List<BattleUnit> _units = new List<BattleUnit>();
        public List<UnitToAdd> _unitsToAdd = new List<UnitToAdd>();
        private Grid grid = null;
        private Grid unlimitedGrid = null;
        private AStarSearch search = null;
        private AStarSearch unlimitedSearch = null;
        private List<Tile> blockedTiles = new List<Tile>();
        private List<Projectile> projectiles = new List<Projectile>();
        public float percentage = 0;

        private bool _isBattling = false;
        private float _preTimer = 0;
        private float _currentTimer = 0;
        //public delegate void UnitSpawned(long id);
        //public delegate void AttackCallback(long index, BattleVector2 target);
        //public delegate void IndexCallback(long index);
        //public delegate void FloatCallback(long index, float value);

        [Button("Init")]
        public void Initialize(List<BattleBuilding> buildings) // ok
        {
            _isBattling = true;
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
                _buildings[i].worldCenterPosition = new BattleVector2((_buildings[i].building.x + (_buildings[i].building.columns / 2f)) * ConfigData.gridCellSize, (_buildings[i].building.y + (_buildings[i].building.rows / 2f)) * ConfigData.gridCellSize);


                // blockTiles: Vẫn đi được ở rìa công trình
                int startX = _buildings[i].building.x;
                int endX = _buildings[i].building.x + _buildings[i].building.columns;

                int startY = _buildings[i].building.y;
                int endY = _buildings[i].building.y + _buildings[i].building.rows;

                if (_buildings[i].building.type != EBuildingType.wall && _buildings[i].building.columns > 1 && _buildings[i].building.rows > 1)
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
                        blockedTiles.Add(new Tile(_buildings[i].building.type, new BattleVector2Int(x, y), i));
                    }
                }
            }
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
                if (_buildings[i].building.targetType != Data.BuildingTargetType.none && _buildings[i].health > 0)
                {
                    HandleBuilding(i, ConfigData.battleFrameRate);
                }
            }

            for (int i = 0; i < _units.Count; i++)
            {
                if (_units[i].health > 0)
                {
                    HandleUnit(i, ConfigData.battleFrameRate);
                }
            }

            if (projectiles.Count > 0)
            {
                for (int i = projectiles.Count - 1; i >= 0; i--)
                {
                    projectiles[i].timer -= ConfigData.battleFrameRate;
                    if (projectiles[i].timer <= 0)
                    {
                        // hồi máu or gây damage
                        if (projectiles[i].type == TargetType.unit)
                        {
                            // hồi máu
                            if (projectiles[i].heal)
                            {
                                //_units[projectiles[i].target].Heal(projectiles[i].damage);

                                //// hồi máu trong phạm vi nổ (splash)
                                //// Không hồi màu cho đối tượng bay
                                //for (int j = 0; j < _units.Count; j++)
                                //{
                                //    if (_units[j].health <= 0 || j == projectiles[i].target || _units[j].unit.movement == UnitMoveType.fly)
                                //    {
                                //        continue;
                                //    }
                                //    float distance = BattleVector2.Distance(_units[j].position, _units[projectiles[i].target].position);
                                //    if (distance < projectiles[i].splash * ConfigData.gridCellSize)
                                //    {
                                //        _units[j].Heal(projectiles[i].damage * (1f - (distance / projectiles[i].splash * ConfigData.gridCellSize)));
                                //    }
                                //}
                            }
                            // GÂy damage, tương tự
                            else
                            {
                                _units[projectiles[i].target].TakeDamage(projectiles[i].damage);
                                if (projectiles[i].splash > 0)
                                {
                                    for (int j = 0; j < _units.Count; j++)
                                    {
                                        if (j != projectiles[i].target)
                                        {
                                            float distance = BattleVector2.Distance(_units[j].position, _units[projectiles[i].target].position);
                                            if (distance < projectiles[i].splash * ConfigData.gridSize)
                                            {
                                                _units[j].TakeDamage(projectiles[i].damage * (1f - (distance / projectiles[i].splash * ConfigData.gridCellSize)));
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            _buildings[projectiles[i].target].TakeDamage(projectiles[i].damage, ref grid, ref blockedTiles, ref percentage);
                        }
                        projectiles.RemoveAt(i);
                    }
                }
            }

            frameCount++;
        }

        private void HandleUnit(int i, float battleFrameRate)
        {
            Debug.Log("HandleUnit");
        }

        private void HandleBuilding(int i, float battleFrameRate)
        {
            Debug.Log("HandleBuilding");
        }

        public bool CanAddUnit(int x, int y) // ok
        {
            for (int i = 0; i < _buildings.Count; i++)
            {
                if (_buildings[i].health <= 0)
                {
                    continue;
                }

                int startX = _buildings[i].building.x;
                int endX = _buildings[i].building.x + _buildings[i].building.columns;

                int startY = _buildings[i].building.y;
                int endY = _buildings[i].building.y + _buildings[i].building.rows;

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



        public class UnitToAdd
        {
            public BattleUnit unit = null;
            public int x;
            public int y;
        }
    }
}

