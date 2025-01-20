using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusicController : MonoBehaviour
{
    public AudioSource backgroundMusic; // AudioSource cho nhạc nền

    void Start()
    {
        // Tải giá trị âm lượng đã lưu nếu có, nếu không sẽ mặc định là 1
        backgroundMusic.volume = PlayerPrefs.GetFloat("MusicVolume", 1f);
    }

    void Update()
    {
        // Cập nhật giá trị âm lượng của nhạc nền
        backgroundMusic.volume = PlayerPrefs.GetFloat("MusicVolume", 1f);
    }
}
