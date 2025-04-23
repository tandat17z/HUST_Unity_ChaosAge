using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour
{
    private NavMeshAgent agent;

    void Start()
    {
        // Lấy component NavMesh Agent
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        // Khi click chuột trái
        if (Input.GetMouseButtonDown(0))
        {
            // Lấy vị trí click trên màn hình
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Kiểm tra xem ray có chạm vào bề mặt không
            if (Physics.Raycast(ray, out hit))
            {
                // Di chuyển đến vị trí click
                agent.SetDestination(hit.point);
            }
        }
    }
}
