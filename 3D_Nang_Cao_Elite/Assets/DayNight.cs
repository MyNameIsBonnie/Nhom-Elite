/*using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNight : MonoBehaviour
{
    public Light directionalLight;

    public float dayDurationInSeconds = 24.0f;
    public int currentHour;
    float currentTimeOfDay = 0.0f;

    public List<SkyboxTimeMapping> timeMappings;

    void Update()
    {
        currentTimeOfDay += Time.deltaTime / dayDurationInSeconds;
        currentTimeOfDay %= 1;

        currentHour = Mathf.FloorToInt(currentTimeOfDay * 24);

        directionalLight.transform.rotation = Quaternion.Euler(new Vector3((currentTimeOfDay * 360) - 90, 170, 0));

        //update the skybox
        UpdateSkyBox();
    }

    private void UpdateSkyBox()
    {
        Material currentSkybox = null;
        foreach (SkyboxTimeMapping mapping in timeMappings)
        {
            if (currentHour == mapping.hour)
            {
                currentSkybox = mapping.skyboxMaterial;
                break;
            }
        }

        if (currentSkybox != null)
        {
            RenderSettings.skybox = currentSkybox;
        }
    }
}
[System.Serializable]
public class SkyboxTimeMapping
{
    public string phaseName;
    public int hour;
    public Material skyboxMaterial;
}
*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNight : MonoBehaviour
{
    public Light directionalLight;
    public float dayDurationInSeconds = 24.0f;
    public int currentHour;
    float currentTimeOfDay = 0.0f;

    public List<SkyboxTimeMapping> timeMappings;

    // Thêm hiệu ứng mưa
    public GameObject rainPrefab;
    private GameObject rainInstance;
    private bool isRaining = false;
    private float rainTimer = 0f;
    private float nextRainTime = 0f;

    // Tùy chỉnh vị trí mưa
    public Vector3 rainPosition = new Vector3(0, 30, 0); // Mặc định ở trên cao

    public AudioSource rainSound; // Gán trong Inspector


    void Start()
    {
        if (rainPrefab != null)
        {
            rainInstance = Instantiate(rainPrefab);
            rainInstance.SetActive(false);
        }

        nextRainTime = UnityEngine.Random.Range(10f, 25f);
    }

    void Update()
    {
        currentTimeOfDay += Time.deltaTime / dayDurationInSeconds;
        currentTimeOfDay %= 1;
        currentHour = Mathf.FloorToInt(currentTimeOfDay * 24);

        directionalLight.transform.rotation = Quaternion.Euler(new Vector3((currentTimeOfDay * 360) - 90, 170, 0));

        UpdateSkyBox();
        HandleRain();
    }

    private void UpdateSkyBox()
    {
        Material currentSkybox = null;
        foreach (SkyboxTimeMapping mapping in timeMappings)
        {
            if (currentHour == mapping.hour)
            {
                currentSkybox = mapping.skyboxMaterial;
                break;
            }
        }

        if (currentSkybox != null)
        {
            RenderSettings.skybox = currentSkybox;
        }
    }

    private void HandleRain()
    {
        if (!isRaining)
        {
            nextRainTime -= Time.deltaTime;

            if (nextRainTime <= 0f)
            {
                StartRain();
            }
        }
        else
        {
            rainTimer -= Time.deltaTime;
            if (rainTimer <= 0f)
            {
                StopRain();
            }
        }

        if (isRaining && rainInstance != null)
        {
            // Cập nhật vị trí mưa theo giá trị người dùng nhập
            rainInstance.transform.position = rainPosition;
        }
    }

    private void StartRain()
    {
        isRaining = true;
        rainInstance.SetActive(true);
        rainTimer = UnityEngine.Random.Range(20f, 60f); // Mưa kéo dài 20-60 giây
        nextRainTime = UnityEngine.Random.Range(10f, 20f); //mưa tiếp theo

        // Bật âm thanh mưa
        if (rainSound != null && !rainSound.isPlaying)
        {
            rainSound.Play();
        }
    }

    private void StopRain()
    {
        isRaining = false;
        rainInstance.SetActive(false);

        // Tắt âm thanh mưa
        if (rainSound != null && rainSound.isPlaying)
        {
            rainSound.Stop();
        }
    }
}

[System.Serializable]
public class SkyboxTimeMapping
{
    public string phaseName;
    public int hour;
    public Material skyboxMaterial;
}
