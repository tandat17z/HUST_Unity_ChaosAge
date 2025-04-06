using System;
using System.Collections;
using System.Collections.Generic;
using ChaosAge.building;
using ChaosAge.input;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ChaosAge.editor
{
    public class BuildGrid : MonoBehaviour
    {
        [SerializeField] private int _rows;
        [SerializeField] private int _columns;
        [SerializeField] private int _cellSize;
        public int Row { get => _rows; }
        public int Column { get => _columns; }
        public int CellSize { get => _cellSize; }

        public Vector3 GetStartPosition(int x, int y)
        {
            Vector3 position = transform.position;
            position += (transform.right.normalized * x * _cellSize) + (transform.forward.normalized * y * _cellSize);
            return position;
        }

        public Vector3 GetCenterPosition(int x, int y, int rows, int columns)
        {
            Vector3 position = GetStartPosition(x, y);
            position += (transform.right.normalized * columns * _cellSize / 2) + (transform.forward.normalized * rows * _cellSize / 2);
            return position;
        }

        public Vector3 GetEndPosition(int x, int y, int rows, int columns)
        {
            Vector3 position = GetStartPosition(x, y);
            position += (transform.right.normalized * columns * _cellSize) + (transform.forward.normalized * rows * _cellSize);
            return position;
        }

        public Vector3 GetEndPosition(Building building)
        {
            return GetEndPosition(building.CurrentX, building.CurrentY, building.Rows, building.Columns);
        }

        public bool IsWorldPositionIsOnPlane(Vector3 position, Building building)
        {
            position = transform.InverseTransformPoint(position);
            Rect rect = new Rect(building.CurrentX, building.CurrentY, building.Columns, building.Rows);

            if (rect.Contains(new Vector2(position.x, position.z)))
            {
                return true;
            }

            return false;
        }

#if UNITY_EDITOR

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.white;
            for (int i = 0; i <= _rows; i++)
            {
                Vector3 point = transform.position + transform.forward.normalized * _cellSize * (float)i;
                Gizmos.DrawLine(point, point + transform.right.normalized * _cellSize * (float)_columns);
            }

            for (int i = 0; i <= _columns; i++)
            {
                Vector3 point = transform.position + transform.right.normalized * _cellSize * (float)i;
                Gizmos.DrawLine(point, point + transform.forward.normalized * _cellSize * (float)_rows);
            }
        }

        public Vector2 ConvertGridPos(Vector3 pos)
        {
            var local = transform.InverseTransformPoint(pos);
            return Vector2.right * Mathf.FloorToInt(local.x / _cellSize) + Vector2.up * Mathf.FloorToInt(local.z / _cellSize);
        }
#endif

        private void OnMouseDown()
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                // Nếu đang click vào UI, thì không làm gì cả
                return;
            }
            Debug.Log("BuildingGrid onmousedown");
            InputHandler.Instance.TouchStarted();
        }

        private void OnMouseUp()
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                // Nếu đang click vào UI, thì không làm gì cả
                return;
            }
            InputHandler.Instance.TouchCanceled();
        }
    }

}
