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

    public AudioClip hurtSound; // Âm thanh khi bị tấn công
    private AudioSource audioSource;

    public GameObject lootPrefab; // Vật phẩm rơi ra khi quái chết
    private bool isDead = false; // Trạng thái quái vật đã chết


    private void Start()
    {
        currentHP = maxHP;
        UpdateHealthUI();
        audioSource = GetComponent<AudioSource>(); // Lấy AudioSource

    }

     void Update()
    {
        if (isDead) return; // Nếu quái đã chết, không làm gì nữa

    }
    public virtual void TakeDamage(float damage)
    {
        

        currentHP -= damage;

        if (hurtSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(hurtSound); // Phát âm thanh khi bị đánh
        }

        UpdateHealthUI(); // Cập nhật UI thanh máu sau khi bị tấn công

        if (currentHP <= 0)
        {
            Die();
        }
    }

    void DropLoot()
    {
        if (lootPrefab != null)
        {
            Instantiate(lootPrefab, transform.position, Quaternion.identity);
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
        if (isDead) return;
        isDead = true;
        if (animator != null)
        {
            animator.SetTrigger("Die");
        }

        if (healthBarFill != null)
        {
            healthBarFill.transform.parent.gameObject.SetActive(false); // Ẩn thanh máu khi chết
        }

        Invoke(nameof(DropLoot), 2f); // Gọi DropLoot() ngay trước khi bị hủy
        Destroy(gameObject, 2f);

        QuestManager.instance.EnemyKilled(); // Báo nhiệm vụ khi Enemy chết
        

    }
}
