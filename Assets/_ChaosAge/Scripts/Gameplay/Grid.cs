namespace ChaosAge
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class Grid : MonoBehaviour
    {
        [SerializeField]
        private int _rows;

        [SerializeField]
        private int _columns;

        [SerializeField]
        private int _cellSize;

        public Vector3 GetStartPosition(int x, int y)
        {
            Vector3 position = transform.position;
            position +=
                (transform.right.normalized * x * _cellSize)
                + (transform.forward.normalized * y * _cellSize);
            return position;
        }

        public Vector3 GetCenterPosition(int x, int y, int rows, int columns)
        {
            Vector3 position = GetStartPosition(x, y);
            position +=
                (transform.right.normalized * columns * _cellSize / 2)
                + (transform.forward.normalized * rows * _cellSize / 2);
            return position;
        }

        public Vector3 GetEndPosition(int x, int y, int rows, int columns)
        {
            Vector3 position = GetStartPosition(x, y);
            position +=
                (transform.right.normalized * columns * _cellSize)
                + (transform.forward.normalized * rows * _cellSize);
            return position;
        }

#if UNITY_EDITOR

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.white;
            for (int i = 0; i <= _rows; i++)
            {
                Vector3 point =
                    transform.position + transform.forward.normalized * _cellSize * (float)i;
                Gizmos.DrawLine(
                    point,
                    point + transform.right.normalized * _cellSize * (float)_columns
                );
            }

            for (int i = 0; i <= _columns; i++)
            {
                Vector3 point =
                    transform.position + transform.right.normalized * _cellSize * (float)i;
                Gizmos.DrawLine(
                    point,
                    point + transform.forward.normalized * _cellSize * (float)_rows
                );
            }
        }

        public Vector2 ConvertGridPos(Vector3 pos)
        {
            var local = transform.InverseTransformPoint(pos);
            return Vector2.right * Mathf.FloorToInt(local.x / _cellSize)
                + Vector2.up * Mathf.FloorToInt(local.z / _cellSize);
        }
#endif
    }
}
