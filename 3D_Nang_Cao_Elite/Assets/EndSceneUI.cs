using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndSceneUI : MonoBehaviour
{
    public GameObject endScreenUI; // Panel chứa hình ảnh & nút
    public Button quitButton;

    void Start()
    {
        endScreenUI.SetActive(false); // Ẩn UI ban đầu
        Invoke(nameof(ShowEndScreen), 5f); // Hiển thị UI sau 5 giây
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            QuitGame();
        }
    }

    void ShowEndScreen()
    {
        endScreenUI.SetActive(true);
        quitButton.onClick.AddListener(QuitGame);
    }

    public void QuitGame()
    {

        // Thoát game
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                        Application.Quit();
        #endif
    }
}
