using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hpCanvas : MonoBehaviour
{
    public static hpCanvas Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
