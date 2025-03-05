using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public float maxHP = 100f;
    public float currentHP;

    private void Start()
    {
        currentHP = maxHP;
    }

    public void TakeDamage(float damage)
    {
        currentHP -= damage;
        currentHP = Mathf.Max(0, currentHP);

        if (currentHP == 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // Logic cho việc chết của người chơi, ví dụ: kết thúc trò chơi
        Debug.Log("Player Died");
        SceneManager.LoadScene(2);
    }
}


