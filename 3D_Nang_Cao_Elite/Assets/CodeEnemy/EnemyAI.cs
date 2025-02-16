using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform player; // Biến lưu người chơi
    public float detectionRange = 10f; // Phạm vi phát hiện
    public float attackRange = 2f; // Phạm vi tấn công
    public float attackDamage = 10f; // Sát thương mỗi lần tấn công
    public float attackCooldown = 2f; // Thời gian giữa các lần tấn công

    private NavMeshAgent navMeshAgent;
    private Animator animator;
    private float lastAttackTime = -Mathf.Infinity; // Thời điểm lần tấn công cuối cùng
    private Collider damageZone; // Vùng gây sát thương

    private Health health; // Thêm biến Health

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        damageZone = GetComponent<Collider>();
        health = GetComponent<Health>(); // Khởi tạo biến Health

        if (damageZone != null)
        {
            damageZone.isTrigger = true; // Đảm bảo Box Collider là trigger
        }

        if (health != null)
        {
            health.animator = animator; // Liên kết Animator với Health
        }
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(player.position, transform.position);

        if (distanceToPlayer <= detectionRange)
        {
            navMeshAgent.SetDestination(player.position); // Di chuyển về phía người chơi
            animator.SetFloat("Speed", navMeshAgent.velocity.magnitude); // Cập nhật Speed cho animator

            // Xoay hướng quái vật theo hướng di chuyển
            if (navMeshAgent.velocity != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(navMeshAgent.velocity.normalized);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
            }

            if (distanceToPlayer <= attackRange && Time.time >= lastAttackTime + attackCooldown)
            {
                Attack(); // Gọi hàm tấn công khi trong phạm vi tấn công và hết thời gian chờ
                lastAttackTime = Time.time; // Cập nhật thời điểm tấn công cuối cùng
            }
        }
        else
        {
            navMeshAgent.SetDestination(transform.position); // Dừng lại nếu ngoài phạm vi
            animator.SetFloat("Speed", 0f); // Đặt Speed về 0 khi đứng yên
        }
    }

    void OnDrawGizmosSelected()
    {
        // Hiển thị phạm vi phát hiện trong chế độ Scene
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        // Hiển thị phạm vi tấn công trong chế độ Scene
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    void Attack()
    {
        animator.SetTrigger("Attack");

        // Gây sát thương cho người chơi nếu trong phạm vi tấn công
        float distanceToPlayer = Vector3.Distance(player.position, transform.position);
        if (distanceToPlayer <= attackRange)
        {
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(attackDamage); // Gây sát thương cho người chơi
            }
        }
    }

    // Hàm xử lý va chạm với Damage Zone
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(attackDamage); // Gây sát thương cho người chơi khi vào vùng gây damage
            }
        }
    }
}


