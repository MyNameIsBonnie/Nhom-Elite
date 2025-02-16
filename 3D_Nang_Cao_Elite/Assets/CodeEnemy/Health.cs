using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float maxHP;
    public float currentHP;
    public RectTransform healthBar; // Thêm RectTransform cho RawImage
    public Animator animator; // Thêm biến Animator

    private void Start()
    {
        currentHP = maxHP;
        UpdateHealthBar();
    }

    public virtual void TakeDamage(float damage)
    {
        currentHP -= damage;
        currentHP = Mathf.Max(0, currentHP);
        UpdateHealthBar();

        if (currentHP == 0)
        {
            Die();
        }
    }

    private void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            float healthPercentage = currentHP / maxHP;
            healthBar.localScale = new Vector3(healthPercentage, 1, 1); // Cập nhật kích thước của thanh máu
        }
    }

    private void Die()
    {
        // Kích hoạt trigger "Die"
        if (animator != null)
        {
            animator.SetTrigger("Die");
        }

        // Logic cho việc chết của đối tượng, ví dụ như phá hủy đối tượng sau một khoảng thời gian
        Destroy(gameObject, 3f); // Phá hủy đối tượng sau 3 giây để hoàn tất hoạt ảnh
    }
}

