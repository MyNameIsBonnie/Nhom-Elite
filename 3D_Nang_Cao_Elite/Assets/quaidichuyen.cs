using UnityEngine;

public class quaidichuyen : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float wanderRadius = 5f;
    private Vector3 initialPosition;
    private Vector3 targetPosition;
    private Animator animator;

    private void Start()
    {
        initialPosition = transform.position;
        targetPosition = GetRandomPosition();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            targetPosition = GetRandomPosition();
        }
        else
        {
            Vector3 direction = (targetPosition - transform.position).normalized;
            if (direction != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * moveSpeed);
            }
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }

        // Tính tốc độ
        float speed = (targetPosition - transform.position).magnitude;

        // Cập nhật giá trị Speed cho Animator
        animator.SetFloat("Speed", speed);
    }

    private Vector3 GetRandomPosition()
    {
        Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;
        randomDirection += initialPosition;
        randomDirection.y = transform.position.y; // Đảm bảo quái không thay đổi vị trí Y
        return randomDirection;
    }
}

