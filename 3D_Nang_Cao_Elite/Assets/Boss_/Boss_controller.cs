using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Boss_controller : MonoBehaviour
{
    public Transform player;
    public float moveSpeed = 3f;
    public float attackRange = 2f;
    public float rangedAttackRange = 5f;
    public int meleeDamage = 10;
    public int rangedDamage = 5;
    public float attackCooldown = 2f;
    public GameObject projectilePrefab;
    public Transform projectileSpawnPoint;

    [Header("Health")]
    public Slider healthSlider;
    private float maxHealth = 300f;
    public float health;

    private Animator animator;
    private float lastAttackTime;


    [Header("Sound Effects")]
    private AudioSource audioSource;
    public AudioClip hurtSound;
    public AudioClip rangedAttackSound;
    public AudioClip deathSound;
    
    void Start()
    {
        health = maxHealth;
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        lastAttackTime = -attackCooldown;
        if (player == null)
        {
            FindPlayer();
        }
        
    }

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange)
        {
            if (Time.time >= lastAttackTime + attackCooldown)
            {
                animator.SetTrigger("MeleeAttack");
                
                lastAttackTime = Time.time;
            }
        }
        else if (distanceToPlayer <= rangedAttackRange)
        {
            if (Time.time >= lastAttackTime + attackCooldown)
            {
                animator.SetTrigger("RangedAttack");
                
                lastAttackTime = Time.time;
            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
            animator.SetBool("IsMoving", true);
        }

        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    void FindPlayer()
    {
        GameObject foundPlayer = GameObject.FindWithTag("Player");
        if (foundPlayer != null)
        {
            player = foundPlayer.transform;
        }
        else
        {
            Debug.LogError("Không tìm thấy Player trong Scene! Hãy chắc chắn Player có tag 'Player'.");
        }
    }

    public void MeleeAttack()
    {
        if (Vector3.Distance(transform.position, player.position) <= attackRange)
        {
            PlayerMovementVerTwo playerHealth = player.GetComponent<PlayerMovementVerTwo>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(meleeDamage);
            }
        }
    }

    public void RangedAttack()
    {
        PlaySound(rangedAttackSound);
        if (projectileSpawnPoint == null)
        {
            Debug.LogError("ProjectileSpawnPoint chưa được gán!");
            return;
        }

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

        PlaySound(hurtSound);

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        PlaySound(deathSound);
        Destroy(gameObject, 3.5f);
        animator.SetTrigger("Die");

        Invoke(nameof(LoadEndScene), 3f);
        Debug.Log("cho load scene");
    }
    void LoadEndScene()
    {
        SceneManager.LoadScene(3); // Thay "EndScene" bằng tên scene bạn muốn
    }
    

    void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
   
}
