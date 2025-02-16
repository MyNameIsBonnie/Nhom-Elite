using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NPCNameDisplay : MonoBehaviour
{
    private Transform trans;
    private Vector3 offset = new Vector3(0, 180, 0);

    private void Start()
    {
        trans = GameObject.Find("FreeLook Camera").GetComponent<Transform>();
    }

    private void Update()
    {
        transform.LookAt(trans);
        transform.Rotate(offset);
    }
}

