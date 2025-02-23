using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Import SceneManager

public class MainMenuController : MonoBehaviour
{
    public GameObject playButton; // Nút Play
    public GameObject storyButton;
    public GameObject settingsButton; // Nút Setting
    public GameObject quitButton;
    public GameObject imageToShow; // Hình ảnh sẽ hiển thị
    public GameObject settingsPanel; // Panel cài đặt
    public GameObject storyPanel; // Panel cot truyen


    public void OnPlayButtonClicked()
    {
        SceneManager.LoadScene("1"); 
    }
    void Start()
    {
        playButton.SetActive(true);
        settingsButton.SetActive(true);
        imageToShow.SetActive(true);
        settingsPanel.SetActive(false); // Ẩn panel cài đặt khi bắt đầu
        storyPanel.SetActive(false);
        storyButton.SetActive(true);
        quitButton.SetActive(true); // Hiển thị nút thoát game
    }

    public void OnSettingsButtonClicked()
    {
        settingsPanel.SetActive(true); // Hiển thị panel cài đặt
        playButton.SetActive(false);
        settingsButton.SetActive(false);
        imageToShow.SetActive(false);
        storyButton.SetActive(false);
        quitButton.SetActive(false); // Ẩn nút thoát game
    }

    public void OnCloseSettingsButtonClicked()
    {
        settingsPanel.SetActive(false); // Ẩn panel cài đặt khi đóng
        playButton.SetActive(true);
        settingsButton.SetActive(true);
        imageToShow.SetActive(true);
        storyButton.SetActive(true);
        quitButton.SetActive(true); // Hiển thị nút thoát game
    }

    public void OnStory()//hien story
    {
        storyPanel.SetActive(true);
        playButton.SetActive(false);
        settingsButton.SetActive(false);
        imageToShow.SetActive(false);
        settingsPanel.SetActive(false);
        storyButton.SetActive(false);
        quitButton.SetActive(false); // Ẩn nút thoát game
    }

    public void CloseStory()
    {
        storyPanel.SetActive(false);
        playButton.SetActive(true);
        settingsButton.SetActive(true);
        imageToShow.SetActive(true);
        settingsPanel.SetActive(false);
        storyButton.SetActive(true);
        quitButton.SetActive(true); // Hiển thị nút thoát game
    }

    public void OnQuitButtonClicked()
    {
        // Thoát game
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif
    }
}


