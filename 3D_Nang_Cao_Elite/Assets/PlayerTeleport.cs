using System.Collections;
using UnityEngine;

public class PlayerTeleport : MonoBehaviour
{
    public Transform[] waypoints; // Danh sách các waypoints
    private int currentWaypointIndex = 0; // Chỉ số của waypoint hiện tại

    void Update()
    {
        // Kiểm tra nếu nhấn phím F
        if (Input.GetKeyDown(KeyCode.F))
        {
           
            TeleportToNextWaypoint();
        }
    }

    void TeleportToNextWaypoint()
    {
        // Kiểm tra nếu có waypoints
        if (waypoints.Length == 0)
        {
            Debug.LogWarning("Không có waypoints được thiết lập!");
            return;
        }

        // Tạm thời vô hiệu hóa CharacterController (nếu có)
        CharacterController controller = GetComponent<CharacterController>();
        if (controller != null)
        {
            controller.enabled = false;
        }

        // Di chuyển player đến waypoint tiếp theo
        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        transform.position = waypoints[currentWaypointIndex].position;

        // Kích hoạt lại CharacterController
        if (controller != null)
        {
            controller.enabled = true;
        }

        
        
    }
}