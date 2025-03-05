using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fireTakeDame : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMovementVerTwo playerHealth = GetComponent<PlayerMovementVerTwo>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(30);
            }
        }
    }
}
