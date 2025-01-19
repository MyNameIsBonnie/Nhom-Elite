using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Người chơi bị trừ: " + damage + " máu");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Người chơi đã chết!");
        // Thêm các xử lý khi người chơi chết, ví dụ như reload scene, show game over screen, v.v.
    }
}
