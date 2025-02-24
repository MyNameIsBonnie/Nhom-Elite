using UnityEngine;
using System.Collections;


namespace TMPro.Examples
{

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

        void Update()
        {
            // Camera xoay tự do theo chuột (không cần nhấn chuột phải)
            currentX += Input.GetAxis("Mouse X") * rotationSpeed;
            currentY -= Input.GetAxis("Mouse Y") * rotationSpeed;
            currentY = Mathf.Clamp(currentY, minVerticalAngle, maxVerticalAngle);

            // Zoom bằng cuộn chuột
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
}