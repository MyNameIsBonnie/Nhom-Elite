using UnityEngine;
using UnityEngine.UI;

public class AudioSettingsManager : MonoBehaviour
{
    public GameObject settingsPanel;      // Bảng cài đặt âm thanh
    public Slider volumeSlider;           // Thanh chỉnh âm lượng
    public AudioSource backgroundMusic;   // Nhạc nền

    private const string VolumeKey = "musicVolume"; // Key lưu âm lượng

    private void Start()
    {
        LoadVolume();  // Tải âm lượng khi khởi động game
        settingsPanel.SetActive(false);  // Ẩn bảng cài đặt ban đầu
    }

    // Mở bảng cài đặt
    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
    }

    // Đóng bảng cài đặt
    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
    }

    // Khi người chơi chỉnh âm lượng
    public void OnVolumeChanged()
    {
        float volume = Mathf.Clamp(volumeSlider.value, 0f, 1f); // Đảm bảo trong khoảng 0-1
        backgroundMusic.volume = volume;
        PlayerPrefs.SetFloat(VolumeKey, volume);
        PlayerPrefs.Save();
    }

    // Tải giá trị âm lượng đã lưu
    private void LoadVolume()
    {
        float savedVolume = PlayerPrefs.GetFloat(VolumeKey, 1.0f); // Mặc định là 100%
        volumeSlider.value = savedVolume;
        backgroundMusic.volume = savedVolume;
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
