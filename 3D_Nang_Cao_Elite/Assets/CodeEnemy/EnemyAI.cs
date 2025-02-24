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
    private bool isAttacking = false; // Kiểm soát trạng thái tấn công

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {

        float distanceToPlayer = Vector3.Distance(player.position, transform.position);

        if (distanceToPlayer <= detectionRange && !isAttacking)
        {
            navMeshAgent.SetDestination(player.position); // Di chuyển về phía người chơi
            animator.SetFloat("Speed", navMeshAgent.velocity.magnitude); // Cập nhật Speed cho animator

            // Xoay hướng quái vật theo hướng di chuyển
            if (navMeshAgent.velocity.magnitude > 0.1f)
            {
                Quaternion lookRotation = Quaternion.LookRotation(navMeshAgent.velocity.normalized);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
            }

            // Kiểm tra nếu có thể tấn công
            if (distanceToPlayer <= attackRange && Time.time >= lastAttackTime + attackCooldown)
            {
                StartAttack();
            }
        }
        else
        {
            navMeshAgent.SetDestination(transform.position); // Dừng lại nếu ngoài phạm vi
            animator.SetFloat("Speed", 0f); // Đặt Speed về 0 khi đứng yên
        }
    }

    void StartAttack()
    {
        isAttacking = true;
        animator.SetTrigger("Attack");
        navMeshAgent.isStopped = true; // Ngừng di chuyển khi tấn công
    }

    // **Gọi từ Animation Event khi đòn đánh thực sự xảy ra**
    public void PerformAttack()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(player.position, transform.position);
        if (distanceToPlayer <= attackRange)
        {
            PlayerMovementVerTwo playerHealth = player.GetComponent<PlayerMovementVerTwo>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage((int)attackDamage);
            }
        }
    }

    // **Gọi từ Animation Event khi đòn đánh kết thúc**
    public void EndAttack()
    {
        isAttacking = false;
        navMeshAgent.isStopped = false; // Tiếp tục di chuyển sau khi tấn công
        lastAttackTime = Time.time; // Cập nhật thời gian tấn công cuối cùng
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
