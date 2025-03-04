using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBoss : MonoBehaviour
{

    public PlayerMovementVerTwo playerVerTwo;

    // Mensaje de Unity | 0 referencias
    void OnTriggerEnter(Collider coll)
    {
        if (coll.CompareTag("Player"))
        {
            coll.GetComponent<PlayerMovementVerTwo>().health -= playerVerTwo.attackDamage; // Personaje3D -> Character3D (or Player3D)
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
