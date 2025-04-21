using AILibraryForNPC.core;
using UnityEngine;

[CreateAssetMenu(fileName = "MoveActionSO", menuName = "AI/Actions/MoveActionSO")]
public class MoveActionSO : BaseActionSO
{
    [SerializeField]
    private float moveDistance = 1f;

    [SerializeField]
    private float moveSpeed = 3f;

    [SerializeField]
    private Vector3 direction = Vector3.left;
    private Vector3 targetPosition;

    public override void StartExecute(Agent agent, WorldState worldState)
    {
        base.StartExecute(agent, worldState);
        targetPosition = agent.transform.position + direction * moveDistance;
    }

    public override void ExecutePerFrame(Agent agent)
    {
        if (!isExecuting)
            return;

        // Di chuyển từng frame
        agent.transform.position = Vector3.MoveTowards(
            agent.transform.position,
            targetPosition,
            moveSpeed * Time.deltaTime
        );

        // Kiểm tra nếu đã đến đích
        if (Vector3.Distance(agent.transform.position, targetPosition) <= 0.1f)
        {
            StopExecute(agent);
        }
    }
}
