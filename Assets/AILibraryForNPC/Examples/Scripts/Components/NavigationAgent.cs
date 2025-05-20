using System.Collections.Generic;
using System.Linq;
using AILibraryForNPC.Algorithms;
using ChaosAge.AI.battle;
using UnityEngine;

namespace ChaosAge.Components
{
    public class NavigationAgent : MonoBehaviour
    {
        public bool isStopped = false;
        private List<NavigationNode> _nodes = new List<NavigationNode>();
        public float moveSpeed = 5f;
        public float rotationSpeed = 5f;
        public float nodeReachedThreshold = 0.1f;
        private int _currentNodeIndex = 0;

        public void SetDestination(Vector3 position)
        {
            // TODO: Implement movement logic
            var startNode = new NavigationNode(transform.position);
            var targetNode = new NavigationNode(position);

            var path = AStar.FindPath(startNode, targetNode);
            _nodes.Clear();
            _nodes = path.Cast<NavigationNode>().ToList();
            _currentNodeIndex = 0;
        }

        void Update()
        {
            if (isStopped || _nodes.Count == 0)
            {
                return;
            }

            // Check if we've reached the end of the path
            if (_currentNodeIndex >= _nodes.Count)
            {
                isStopped = true;
                return;
            }

            // Get current target node
            NavigationNode currentTargetNode = _nodes[_currentNodeIndex];
            Vector3 targetPosition = currentTargetNode.position;

            // Move towards the target
            Vector3 direction = (targetPosition - transform.position).normalized;
            transform.position += direction * moveSpeed * Time.deltaTime;

            // Rotate towards movement direction
            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    targetRotation,
                    rotationSpeed * Time.deltaTime
                );
            }

            // Check if we've reached the current node
            float distanceToNode = Vector3.Distance(transform.position, targetPosition);
            if (distanceToNode < nodeReachedThreshold)
            {
                _currentNodeIndex++;
            }
        }
    }

    public class NavigationNode : INode
    {
        public Vector3 position;
        public int x;
        public int y;

        public NavigationNode(Vector3 position)
        {
            this.position = position;
            var cell = AIBattleManager.Instance.GetCell(position);
            this.x = (int)cell.x;
            this.y = (int)cell.y;
        }

        public NavigationNode(int x, int y)
        {
            this.x = x;
            this.y = y;
            this.position = AIBattleManager.Instance.GetWorldPosition(new Vector2(x, y));
        }

        public bool CheckIfGoalReached(INode current)
        {
            var currentNode = (NavigationNode)current;
            return x == currentNode.x && y == currentNode.y;
        }

        public bool Equals(INode other)
        {
            var otherNode = (NavigationNode)other;
            return x == otherNode.x && y == otherNode.y;
        }

        public float GetCost()
        {
            return 1;
        }

        public float GetHeuristic(INode goal)
        {
            var goalNode = (NavigationNode)goal;
            return Mathf.Sqrt(Mathf.Pow(x - goalNode.x, 2) + Mathf.Pow(y - goalNode.y, 2));
        }

        public List<INode> GetNeighbors()
        {
            var neighbors = new List<INode>();
            if (
                x + 1 < AIBattleManager.MaxCell
                && AIBattleManager.Instance.CheckCanMoveOnCell(new Vector2(x + 1, y))
            )
            {
                neighbors.Add(new NavigationNode(x + 1, y));
            }
            if (x - 1 >= 0 && AIBattleManager.Instance.CheckCanMoveOnCell(new Vector2(x - 1, y)))
            {
                neighbors.Add(new NavigationNode(x - 1, y));
            }
            if (
                y + 1 < AIBattleManager.MaxCell
                && AIBattleManager.Instance.CheckCanMoveOnCell(new Vector2(x, y + 1))
            )
            {
                neighbors.Add(new NavigationNode(x, y + 1));
            }
            if (y - 1 >= 0 && AIBattleManager.Instance.CheckCanMoveOnCell(new Vector2(x, y - 1)))
            {
                neighbors.Add(new NavigationNode(x, y - 1));
            }
            return neighbors;
        }
    }
}
