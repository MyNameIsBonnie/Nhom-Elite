using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuestManager : MonoBehaviour
{
    private int enemyKillCount = 0;
    public static QuestManager instance;
    public TextMeshProUGUI KillCountText;
    public TextMeshProUGUI CountdownText; // Thêm text hiển thị đếm ngược

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void EnemyKilled()
    {
        enemyKillCount++;
        KillCountText.text = "Enemy killed: " + enemyKillCount + "/4";

        if (enemyKillCount >= 4)
        {
            KillCountText.color = Color.green;
            StartCoroutine(CountdownToBoss(7)); // Gọi hàm đếm ngược
        }
    }

    private IEnumerator CountdownToBoss(int countdownTime)
    {
        while (countdownTime > 0)
        {
            CountdownText.text = "BOSS XUAT HIEN SAU: " + countdownTime + "s";
            yield return new WaitForSeconds(1f);
            countdownTime--;
        }

        SceneManager.LoadScene("End"); // Chuyển scene sau khi đếm ngược xong
        
    }
}
