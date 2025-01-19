using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Transform player;  // Tham chiếu đến đối tượng người chơi
    public float moveSpeed = 3f;  // Tốc độ di chuyển của quái vật
    public float attackRange = 2f;  // Phạm vi tấn công của quái vật
    public float attackCooldown = 1f;  // Thời gian chờ giữa các lần tấn công

    private float lastAttackTime;  // Thời điểm tấn công cuối cùng

    void Start()
    {
        // Tìm đối tượng người chơi trong game
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer <= attackRange)
            {
                if (Time.time > lastAttackTime + attackCooldown)
                {
                    AttackPlayer();
                    lastAttackTime = Time.time;
                }
            }
            else
            {
                MoveTowardsPlayer();
            }
        }
    }

    void MoveTowardsPlayer()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
        transform.LookAt(player);
    }

    void AttackPlayer()
    {
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            int damage = 10;  // Số sát thương mà quái vật gây ra
            playerHealth.TakeDamage(damage);
            Debug.Log("Quái vật tấn công! Người chơi nhận " + damage + " sát thương.");
        }
    }
}
