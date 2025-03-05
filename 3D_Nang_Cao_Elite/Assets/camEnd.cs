using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camEnd : MonoBehaviour
{
    public Transform target; // Đối tượng trung tâm để camera quay quanh
    public float rotationSpeed = 20f; // Tốc độ xoay

    void Start()
    {
        RemoveDontDestroyOnLoadObjects();
    }
    void Update()
    {
        if (target == null) return;
        
        transform.LookAt(target); // Nhìn vào trung tâm
        transform.RotateAround(target.position, Vector3.up, rotationSpeed * Time.deltaTime);
    }
    void RemoveDontDestroyOnLoadObjects()
    {
        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            if (obj.scene.name == null || obj.scene.name == "DontDestroyOnLoad")
            {
                Destroy(obj);
            }
        }
    }
}
