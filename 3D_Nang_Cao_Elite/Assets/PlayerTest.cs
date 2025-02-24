using UnityEngine;
using Cinemachine;

public class PlayerTest : MonoBehaviour
{
    public float speed = 6f;
    public CinemachineFreeLook freeLookCamera; // Camera theo dõi
    private CharacterController characterController;

    public float attackRange = 2f; // Phạm vi tấn công
    public LayerMask enemyLayer; // Chọn layer của Enemy trong Inspector

    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Lấy input từ bàn phím
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // Lấy hướng từ FreeLook Camera
        Vector3 forward = freeLookCamera.transform.forward;
        Vector3 right = freeLookCamera.transform.right;

        // Chỉ di chuyển trên trục X-Z
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        // Vector di chuyển
        Vector3 move = forward * moveVertical + right * moveHorizontal;

        // Di chuyển player
        characterController.Move(move * speed * Time.deltaTime);

        // Kiểm tra tấn công
        if (Input.GetKeyDown(KeyCode.F))
        {
            Attack();
        }
    }

    void Attack()
    {
        // Tìm kẻ địch trong phạm vi tấn công
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, attackRange, enemyLayer);

        foreach (Collider enemy in hitEnemies)
        {
            Enemy_Health enemyHealth = enemy.GetComponent<Enemy_Health>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(50); // Gây sát thương
                Debug.Log("Tấn công: " + enemy.name);
            }
        }
    }

    // Hiển thị phạm vi tấn công trong Scene
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
