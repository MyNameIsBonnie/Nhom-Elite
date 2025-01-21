using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent navMeshAgent;
    public Transform player;
    public float detectionRange = 10f;
    public float speed = 3.5f;
    private Vector3 originalPosition;
    private Quaternion originalRotation;

    void Start()
    {
        originalPosition = transform.position;
        originalRotation = transform.rotation;
        navMeshAgent.speed = speed;
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange)
        {
            navMeshAgent.SetDestination(player.position);
            RotateTowards(player.position);
        }
        else
        {
            navMeshAgent.SetDestination(originalPosition);

            if (Vector3.Distance(transform.position, originalPosition) < 0.1f)
            {
                transform.rotation = originalRotation;
            }
        }
    }

    void RotateTowards(Vector3 target)
    {
        Vector3 direction = (target - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }
}