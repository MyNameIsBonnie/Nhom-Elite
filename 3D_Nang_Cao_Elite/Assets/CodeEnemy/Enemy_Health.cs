using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy_Health : MonoBehaviour
{
    [SerializeField] public float maxHP = 100f;
    [SerializeField] public float currentHP;
    public Animator animator;
    public Image healthBarFill; // Dùng Image thay vì Slider

    private void Start()
    {
        currentHP = maxHP;
        UpdateHealthUI();
    }

    public virtual void TakeDamage(float damage)
    {
        currentHP -= damage;
        currentHP = Mathf.Max(0, currentHP);

        UpdateHealthUI();

        if (currentHP == 0)
        {
            Die();
        }
    }

    void UpdateHealthUI()
    {
        if (healthBarFill != null)
        {
            healthBarFill.fillAmount = currentHP / maxHP; // Cập nhật thanh máu
        }
    }

    private void Die()
    {
        if (animator != null)
        {
            animator.SetTrigger("Die");
        }

        if (healthBarFill != null)
        {
            healthBarFill.transform.parent.gameObject.SetActive(false); // Ẩn thanh máu khi chết
        }

        Destroy(gameObject, 2f);
    }
}
