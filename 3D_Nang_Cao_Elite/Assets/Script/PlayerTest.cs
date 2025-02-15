using UnityEngine;
using Cinemachine;

public class PlayerTest : MonoBehaviour
{
    public float speed = 6f;
    public CinemachineFreeLook freeLookCamera; // Gán FreeLook Camera vào đây từ Inspector
    private CharacterController characterController;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Lấy các giá trị đầu vào từ bàn phím
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // Lấy hướng từ FreeLook Camera
        Vector3 forward = freeLookCamera.transform.forward;
        Vector3 right = freeLookCamera.transform.right;

        // Đảm bảo chỉ di chuyển trên mặt phẳng (X-Z)
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        // Tạo vector di chuyển
        Vector3 move = forward * moveVertical + right * moveHorizontal;

        // Di chuyển player
        characterController.Move(move * speed * Time.deltaTime);
    }
}
