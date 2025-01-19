using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemiAttack : MonoBehaviour
{
    public float attackRange = 2.0f; // Khoảng cách tấn công
    public float attackRate = 1.0f;  // Tần suất tấn công
    public int damageAmount = 10;    // Lượng sát thương
    private float nextAttackTime = 0f;
    private NavMeshAgent navMeshAgent;

    // Tham chiếu tới người chơi
    private Transform player;
    private PlayerHealth playerHealth; // Thêm biến này để truy cập sức khỏe của người chơi

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerHealth = player.GetComponent<PlayerHealth>(); // Khởi tạo biến PlayerHealth
    }

    void Update()
    {
        if (player != null)
        {
            navMeshAgent.SetDestination(player.position);

            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer <= attackRange && Time.time >= nextAttackTime)
            {
                Attack();
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }
    }

    void Attack()
    {
        // Trừ máu người chơi
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damageAmount); // Gọi hàm TakeDamage trong PlayerHealth
        }

        // Gọi hàm tấn công ở đây
        Debug.Log("Quái vật tấn công người chơi!");

        // Có thể thêm các phương thức như hiệu ứng tấn công, âm thanh, vv...
    }
}
