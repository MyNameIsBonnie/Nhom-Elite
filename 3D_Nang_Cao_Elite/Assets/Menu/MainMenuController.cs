using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement; // Import SceneManager

public class MainMenuController : MonoBehaviour
{
    public GameObject playButton; // Nút Play
    public GameObject settingsButton; // Nút Setting
    public TextMeshProUGUI startText; // Văn bản "Nhấn bất kỳ đâu để bắt đầu"
    public GameObject imageToShow; // Hình ảnh sẽ hiển thị
    public GameObject settingsPanel; // Panel cài đặt

    void Start()
    {
        playButton.SetActive(false);
        settingsButton.SetActive(false);
        imageToShow.SetActive(false);
        settingsPanel.SetActive(false); // Ẩn panel cài đặt khi bắt đầu
    }

    void Update()
    {
        if (Input.anyKeyDown)
        {
            ShowUIElements();
        }
    }

    void ShowUIElements()
    {
        playButton.SetActive(true);
        settingsButton.SetActive(true);
        startText.gameObject.SetActive(false);
        imageToShow.SetActive(true);
    }

    public void OnPlayButtonClicked()
    {
        SceneManager.LoadScene("YourGameSceneName"); // Thay thế "YourGameSceneName" bằng tên scene của bạn
    }

    public void OnSettingsButtonClicked()
    {
        settingsPanel.SetActive(true); // Hiển thị panel cài đặt
    }

    public void OnCloseSettingsButtonClicked()
    {
        settingsPanel.SetActive(false); // Ẩn panel cài đặt khi đóng
    }
}


