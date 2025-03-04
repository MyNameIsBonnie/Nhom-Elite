using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRange : MonoBehaviour
{
    public Animator ani;
    public BossMovement boss;
    public int melee;

    // Mensaje de Unity | 0 referencias
    void OnTriggerEnter(Collider coll)
    {
        if (coll.CompareTag("PJ"))
        {
            melee = Random.Range(0, 4);
            switch (melee)
            {
                case 0:
                    //// Strike 1 //// // Golpe1 -> Strike 1
                    ani.SetFloat("skills", 0);
                    boss.hitSelect = 0; // hit_Select -> hitSelect
                    break;
                case 1:
                    //// Strike 2 //// // Golpe2 -> Strike 2
                    ani.SetFloat("skills", 0);
                    boss.hitSelect = 1; // hit_Select -> hitSelect
                    break;
                case 2:
                    //// Jump //// // 3ump -> Jump
                    ani.SetFloat("skills", 0);
                    boss.hitSelect = 2; // hit_Select -> hitSelect
                    break;
                case 3:
                    //// Fire Ball ////
                    if (boss.phase == 2) // fase -> phase
                    {
                        ani.SetFloat("skills", 0);
                    }
                    else
                    {
                        melee = 0;
                    }
                    break;
            }
            ani.SetBool("walk", false);
            ani.SetBool("run", false);
            ani.SetBool("attack", true);
            boss.attacking = true; // atacando -> attacking
            GetComponent<CapsuleCollider>().enabled = false;
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
