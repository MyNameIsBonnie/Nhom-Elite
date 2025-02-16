using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageZone : MonoBehaviour
{
    public Collider damageCollider;
    public int damageAmount = 20;

    public string targetTag; // Tag của mục tiêu
    public List<Collider> colliderTargets = new List<Collider>();

    public AudioSource audioSource;  // Thành phần AudioSource để phát âm thanh
    public AudioClip hitSound;      // Âm thanh khi bị đánh

    void Start()
    {
        damageCollider.enabled = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag(targetTag) && !colliderTargets.Contains(other))
        {
            colliderTargets.Add(other);
            var go = other.GetComponent<Health>();
            if (go != null)
            {
                go.TakeDamage(damageAmount);
                Debug.Log($"{other.gameObject.name} bị tấn công, máu hiện tại: {go.currentHP}");
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(targetTag) && !colliderTargets.Contains(other))
        {
            colliderTargets.Add(other);
            var go = other.GetComponent<Health>();
            if (go != null)
            {
                go.TakeDamage(damageAmount);
                Debug.Log($"{other.gameObject.name} bị tấn công, máu hiện tại: {go.currentHP}");
            }
        }
    }

    public void BeginAttack()
    {
        colliderTargets.Clear();
        damageCollider.enabled = true;
    }

    public void EndAttack()
    {
        damageCollider.enabled = false;
        colliderTargets.Clear();
    }

    public void IncreaseDamage(int amount)
    {
        damageAmount += amount;
    }

    public void TakeDamage()
    {
        if (hitSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(hitSound);  // Phát âm thanh bị đánh
        }

        // Thêm logic xử lý mất máu hoặc hiệu ứng tại đây
        Debug.Log("Nhân vật bị đánh!");
    }
}



