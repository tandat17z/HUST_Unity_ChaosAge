using ChaosAge.Data;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleBuilding : MonoBehaviour
{
    [SerializeField] Slider hpSlider;
    [SerializeField] TMP_Text text;

    public BuildingData building = null;
    public float health = 0;
    public int target = -1;
    public double attackTimer = 0;
    public float percentage = 0;
    public BattleVector2 worldCenterPosition;

    private void Start()
    {
        text.gameObject.SetActive(false);
        hpSlider.gameObject.SetActive(false);
    }

    public void Initialize()
    {
        health = building.health;
        percentage = building.percentage;

        text.gameObject.SetActive(true);
        hpSlider.gameObject.SetActive(true);
        text.text = building.type.ToString();
        hpSlider.value = 1;
    }

    public void TakeDamage(float damage, ref AStarPathfinding.Grid grid, ref List<Tile> blockedTiles, ref float percentage)
    {
        if (health <= 0) { return; }
        health -= damage;

        hpSlider.value = health / building.health;
        // die
        if (health < 0) { health = 0; }
        if (health <= 0)
        {
            for (int x = building.x; x < building.x + building.columns; x++)
            {
                for (int y = building.y; y < building.y + building.rows; y++)
                {
                    grid[x, y].Blocked = false;
                    for (int i = 0; i < blockedTiles.Count; i++)
                    {
                        if (blockedTiles[i].position.x == x && blockedTiles[i].position.y == y)
                        {
                            blockedTiles.RemoveAt(i);
                            break;
                        }
                    }
                }
            }
            if (this.percentage > 0)
            {
                percentage += this.percentage;
            }

            Destroy(gameObject);
        }
    }

    public void HandleBuilding(int index, float battleFrameRate)
    {

    }
}
