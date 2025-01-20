using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{
    public Slider volumeSlider; // Slider để điều chỉnh âm lượng

    void Start()
    {
        // Tải giá trị âm lượng đã lưu nếu có, nếu không sẽ mặc định là 1
        volumeSlider.value = PlayerPrefs.GetFloat("Volume", 1f);
        // Đặt giá trị âm lượng cho AudioListener
        AudioListener.volume = volumeSlider.value;

        volumeSlider.onValueChanged.AddListener(delegate { OnVolumeChange(); });
    }

    public void OnVolumeChange()
    {
        // Cập nhật giá trị âm lượng cho AudioListener
        AudioListener.volume = volumeSlider.value;
        // Lưu giá trị âm lượng
        PlayerPrefs.SetFloat("Volume", volumeSlider.value);
        PlayerPrefs.Save();
    }
}

