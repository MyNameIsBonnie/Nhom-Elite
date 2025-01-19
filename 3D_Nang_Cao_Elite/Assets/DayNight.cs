using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNight : MonoBehaviour
{
    public Light sun;
    public float daySpeed = 10f;

    void Update()
    {
        sun.transform.Rotate(Vector3.right, daySpeed * Time.deltaTime);
    }
}
