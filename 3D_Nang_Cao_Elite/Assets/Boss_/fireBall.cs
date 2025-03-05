using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fireBall : MonoBehaviour
{


    public float speed = 10f;
    public int damage = 5;

    private Transform target;

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    public void SetDamage(int newDamage)
    {
        damage = newDamage;
    }

    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 direction = (target.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;

        if (Vector3.Distance(transform.position, target.position) < 0.2f)
        {
            HitTarget();
        }
    }

    void HitTarget()
    {
        PlayerMovementVerTwo playerHealth = target.GetComponent<PlayerMovementVerTwo>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(10);
        }
        Destroy(gameObject);
    }
}
