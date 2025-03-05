using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target; // Nhân vật cần theo dõi
    public float distance = 5.0f; // Khoảng cách từ camera đến nhân vật
    public float minDistance = 2.0f; // Khoảng cách zoom gần nhất
    public float maxDistance = 10.0f; // Khoảng cách zoom xa nhất
    public float zoomSpeed = 2.0f; // Tốc độ zoom

    public float rotationSpeed = 3.0f; // Tốc độ xoay camera
    public float minVerticalAngle = -20f; // Giới hạn góc nhìn xuống
    public float maxVerticalAngle = 80f; // Giới hạn góc nhìn lên

    private float currentX = 0f;
    private float currentY = 20f;
    public static CameraController Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Tự động tìm Player trong scene
        if (target == null)
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                target = player.transform;
            }
        }
    }

    void Update()
    {
        if (target == null) return; // Nếu chưa có Player, không thực hiện cập nhật

        // Chỉ xoay camera khi chuột bị khóa (ẩn)
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            currentX += Input.GetAxis("Mouse X") * rotationSpeed;
            currentY -= Input.GetAxis("Mouse Y") * rotationSpeed;
            currentY = Mathf.Clamp(currentY, minVerticalAngle, maxVerticalAngle);
        }

        // Zoom bằng cuộn chuột (cho phép zoom ngay cả khi chuột mở)
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        distance -= scroll * zoomSpeed;
        distance = Mathf.Clamp(distance, minDistance, maxDistance);
    }

    void LateUpdate()
    {
        if (target == null) return;

        // Xác định vị trí mới của camera dựa trên góc quay và khoảng cách
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        Vector3 direction = new Vector3(0, 0, -distance);
        Vector3 position = target.position + rotation * direction;

        // Đặt vị trí và xoay camera về nhân vật
        transform.position = position;
        transform.LookAt(target.position);
    }
}
