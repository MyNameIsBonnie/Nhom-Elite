using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;  // Sức khỏe tối đa của người chơi
    private int currentHealth;  // Sức khỏe hiện tại của người chơi

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Người chơi bị tấn công! Sức khỏe còn lại: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Người chơi đã chết!");
        // Thêm các hành động khi người chơi chết tại đây
    }
}
