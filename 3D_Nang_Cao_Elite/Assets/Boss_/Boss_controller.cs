using UnityEngine;
using UnityEngine.UI;

public class Boss_controller : MonoBehaviour
{
    public Transform player; // Tham chiếu đến player
    public float moveSpeed = 3f; // Tốc độ di chuyển của boss
    public float attackRange = 2f; // Tầm tấn công tầm gần
    public float rangedAttackRange = 5f; // Tầm tấn công tầm xa
    public int meleeDamage = 10; // Sát thương tấn công tầm gần
    public int rangedDamage = 5; // Sát thương tấn công tầm xa
    public float attackCooldown = 2f; // Thời gian chờ giữa các đợt tấn công
    public GameObject projectilePrefab; // Prefab cho đạn tấn công tầm xa
    public Transform projectileSpawnPoint; // Vị trí spawn đạn

    [Header("Health")]
    public Slider healthSlider;
    public float maxHealth = 200f;
    public float health;

    private Animator animator;
    private float lastAttackTime;

    void Start()
    {
        animator = GetComponent<Animator>();
        lastAttackTime = -attackCooldown; // Đảm bảo boss có thể tấn công ngay lập tức
    }

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange)
        {
            // Tấn công tầm gần
            if (Time.time >= lastAttackTime + attackCooldown)
            {
                animator.SetTrigger("MeleeAttack");
                lastAttackTime = Time.time;
            }
        }
        else if (distanceToPlayer <= rangedAttackRange)
        {
            // Tấn công tầm xa
            if (Time.time >= lastAttackTime + attackCooldown)
            {
                animator.SetTrigger("RangedAttack");
                lastAttackTime = Time.time;
            }
        }
        else
        {
            // Di chuyển đến player
            transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
            animator.SetBool("IsMoving", true);
        }

        // Xoay boss hướng về player
        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    // Hàm này sẽ được gọi từ animation event khi tấn công tầm gần
    public void MeleeAttack()
    {
        if (Vector3.Distance(transform.position, player.position) <= attackRange)
        {
            // Gây sát thương cho player
            PlayerMovementVerTwo playerHealth = player.GetComponent<PlayerMovementVerTwo>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(10);
            }
        }
    }

    // Hàm này sẽ được gọi từ animation event khi tấn công tầm xa
    public void RangedAttack()
    {
        if (projectileSpawnPoint == null)
        {
            Debug.LogError("ProjectileSpawnPoint chưa được gán!");
            return;
        }

        // Tạo đạn tại vị trí spawn
        GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
        fireBall projectileScript = projectile.GetComponent<fireBall>();
        if (projectileScript != null)
        {
            projectileScript.SetTarget(player);
            projectileScript.SetDamage(rangedDamage);
        }
    }
    public void TakeDamage(int damage)
    {
        health = Mathf.Max(0, health - damage);
        healthSlider.value = health;

        /*if (hurtSound != null)
        {
            audioSource.PlayOneShot(hurtSound);
        }*/

        if (health <= 0)
        {
            Die();
        }
    }
    void Die()
    {
        Destroy(gameObject, 3.5f);
        animator.SetTrigger("Die");
    }
}