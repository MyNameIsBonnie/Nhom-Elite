using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Import SceneManager

public class MainMenuController : MonoBehaviour
{
    public GameObject playButton; // Nút Play
    public GameObject settingsButton; // Nút Setting
    public GameObject storyButton;
    public TextMeshProUGUI startText; // Văn bản "Nhấn bất kỳ đâu để bắt đầu"
    public GameObject imageToShow; // Hình ảnh sẽ hiển thị
    public GameObject settingsPanel; // Panel cài đặt
    public GameObject storyPanel; // Panel cot truyen

    void Start()
    {
        playButton.SetActive(true);
        settingsButton.SetActive(true);
        imageToShow.SetActive(true);
        settingsPanel.SetActive(false); // Ẩn panel cài đặt khi bắt đầu
        storyPanel.SetActive(false);
        storyButton.SetActive(true);

    }

    void Update()
    {

    }


    public void OnPlayButtonClicked()
    {
        SceneManager.LoadScene("1"); // Thay thế "YourGameSceneName" bằng tên scene của bạn
    }

    public void OnSettingsButtonClicked()
    {
        settingsPanel.SetActive(true); // Hiển thị panel cài đặt
        playButton.SetActive(false);
        settingsButton.SetActive(false);
        imageToShow.SetActive(false);
        storyButton.SetActive(false);


    }

    public void OnCloseSettingsButtonClicked()
    {
        settingsPanel.SetActive(false); // Ẩn panel cài đặt khi đóng
        playButton.SetActive(true);
        settingsButton.SetActive(true);
        imageToShow.SetActive(true);
        storyButton.SetActive(true);

    }

    public void OnStory()//hien story
    {
        storyPanel.SetActive(true);
        playButton.SetActive(false);
        settingsButton.SetActive(false);
        imageToShow.SetActive(false);
        settingsPanel.SetActive(false);
        storyButton.SetActive(false);
    }
    public void CloseStory()
    {
        storyPanel.SetActive(false);
        playButton.SetActive(true);
        settingsButton.SetActive(true);
        imageToShow.SetActive(true);
        settingsPanel.SetActive(false);
        storyButton.SetActive(true);
    }
}


