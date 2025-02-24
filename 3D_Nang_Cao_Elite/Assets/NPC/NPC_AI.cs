using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class NPC_AI : MonoBehaviour
{
    public float moveRadius = 10f;  // Bán kính di chuyển ngẫu nhiên
    public float idleTime = 3f;     // Thời gian chờ giữa các lần di chuyển

    private NavMeshAgent agent;
    private Animator animator;
    private bool isMoving = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        StartCoroutine(Wander());
    }

    IEnumerator Wander()
    {
        while (true)
        {
            // Dừng di chuyển, chuyển sang trạng thái Idle
            agent.isStopped = true;
            isMoving = false;
            animator.SetBool("isWalking", false);

            // Chờ thời gian Idle
            yield return new WaitForSeconds(idleTime);

            // Chọn vị trí mới
            Vector3 newTarget = GetRandomPoint(transform.position, moveRadius);

            // Bắt đầu di chuyển
            agent.isStopped = false;
            agent.SetDestination(newTarget);
            isMoving = true;
            animator.SetBool("isWalking", true);

            // Chờ cho đến khi NPC đến nơi hoặc bị kẹt
            yield return new WaitUntil(() => !agent.pathPending && (agent.remainingDistance <= agent.stoppingDistance || !agent.hasPath));
        }
    }

    Vector3 GetRandomPoint(Vector3 origin, float radius)
    {
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += origin;
        randomDirection.y = transform.position.y;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, radius, NavMesh.AllAreas))
        {
            return hit.position;
        }
        return origin;
    }
}
