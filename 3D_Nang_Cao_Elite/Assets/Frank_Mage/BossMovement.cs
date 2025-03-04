using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class BossMovement : MonoBehaviour
{
    //Boss base script behavior
    public int routine;
    public float timer;
    public float routineTime;
    public Animator ani;
    public Quaternion angle;
    public float degree;
    public GameObject target;
    public bool attacking;
    public BossRange range;
    public float speed;
    public GameObject[] hit;
    public int hitSelect;

    //Flamethrower
    public bool flamethrower;
    public List<GameObject> pool = new List<GameObject>();
    public GameObject fire;
    public GameObject head;
    private float timer2;

    //Jump Attack
    public float jumpDistance;
    public bool directionSkill;

    //fire ball
    public GameObject fireBall;
    public GameObject point;
    public List<GameObject> pool2 = new List<GameObject>();

    public int phase = 1;
    public float HP_Min;
    public float HP_Max;
    public Image bar;
    public AudioSource music;
    public bool dead;

    void Start()
    {
        ani = GetComponent<Animator>(); // ani -> anim
        target = GameObject.Find("Link");
    }

    public void BossBehavior() // Comportamiento_Boss -> BossBehavior
    {
        if (Vector3.Distance(transform.position, target.transform.position) < 15)
        {
            var lookPos = target.transform.position - transform.position;
            lookPos.y = 0;
            var rotation = Quaternion.LookRotation(lookPos);
            point.transform.LookAt(target.transform.position);
            music.enabled = true;

            if (Vector3.Distance(transform.position, target.transform.position) > 1 && !attacking)
            {
                switch (routine)
                {
                    case 0:
                        ////// Walk //////
                        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, 2);
                        ani.SetBool("walk", true);
                        ani.SetBool("run", false);
                        if (transform.rotation == rotation)
                        {
                            transform.Translate(Vector3.forward * speed * Time.deltaTime);
                        }
                        ani.SetBool("attack", false);
                        timer += 1 * Time.deltaTime;
                        if (timer > routineTime) 
                        {
                            routine = Random.Range(0, 5);
                            timer = 0;
                        }
                        break;
                    case 1:
                        ////// Run //////
                        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, 2);
                        ani.SetBool("walk", false);
                        ani.SetBool("run", true);
                        if (transform.rotation == rotation)
                        {
                            transform.Translate(Vector3.forward * speed * 2 * Time.deltaTime);
                        }
                        ani.SetBool("attack", false);
                        break;

                    case 2:
                        ////// Flamethrower //////
                        ani.SetBool("walk", false);
                        ani.SetBool("run", false);
                        ani.SetBool("attack", true);
                        ani.SetFloat("skills", 0);
                        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, 2);
                        range.GetComponent<CapsuleCollider>().enabled = false;
                        break;

                    case 3:
                        ////// Jump Attack //////
                        if (phase == 2) // fase -> phase
                        {
                            jumpDistance += 1 * Time.deltaTime; // jump_distance -> jumpDistance
                            ani.SetBool("walk", false); // ani -> anim
                            ani.SetBool("run", false); // ani -> anim
                            ani.SetBool("attack", true); // ani -> anim
                            ani.SetFloat("skills", 0); // ani -> anim
                            hitSelect = 3; // hit_Select -> hitSelect
                            range.GetComponent<CapsuleCollider>().enabled = false; // rango -> range

                            if (directionSkill) // direction_Skill -> directionSkill
                            {
                                if (jumpDistance < 1f) // jump_distance -> jumpDistance
                                {
                                    transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, 2);
                                }
                                transform.Translate(Vector3.forward * 8 * Time.deltaTime);
                            }
                        }
                        else
                        {
                            routine = 0;
                            timer = 0; // cronometro -> timer
                        }
                        break;

                    case 4:
                        ////// Fire ball //////
                        if (phase == 2) // fase -> phase
                        {
                            ani.SetBool("walk", false); // ani -> anim
                            ani.SetBool("run", false); // ani -> anim
                            ani.SetBool("attack", true); // ani -> anim
                            ani.SetFloat("skills", 0); // ani -> anim
                            range.GetComponent<CapsuleCollider>().enabled = false; // rango -> range
                            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, 0.5f);
                        }
                        else
                        {
                            routine = 0;
                            timer = 0; // cronometro -> timer
                        }
                        break;
                }
            }
        }
    }
    public void FinalAni() // Final_Ani -> FinalAni (camelCase)
    {
        routine = 0;
        ani.SetBool("attack", false); // ani -> anim
        attacking = false;
        range.GetComponent<CapsuleCollider>().enabled = true; // rango -> range
        flamethrower = false; // lanza_llamas -> flamethrower
        jumpDistance = 0; // jump_distance -> jumpDistance
        directionSkill = false; // direction_Skill -> directionSkill
    }

    public void DirectionAttackStart() // Direction_Attack_Start -> DirectionAttackStart (camelCase)
    {
        directionSkill = true; // direction_Skill -> directionSkill
    }

    public void DirectionAttackFinal() // Direction_Attack_Final -> DirectionAttackFinal (camelCase)
    {
        directionSkill = false; // direction_Skill -> directionSkill
    }

    /////// - Melee - ///////

    public void ColliderWeaponTrue() // ColliderWeapon True -> ColliderWeaponTrue (camelCase)
    {
        hit[hitSelect].GetComponent<SphereCollider>().enabled = true; // hit_Select -> hitSelect
    }

    public void ColliderWeaponFalse() // ColliderWeaponFalse -> ColliderWeaponFalse (camelCase)
    {
        hit[hitSelect].GetComponent<SphereCollider>().enabled = false; // hit_Select -> hitSelect
    }

    ////// Flamethrower ////// // Lanzallamas -> Flamethrower

    public GameObject GetBullet() // GetBala -> GetBullet
    {
        for (int i = 0; i < pool.Count; i++)
        {
            if (!pool[i].activeInHierarchy)
            {
                pool[i].SetActive(true);
                return pool[i];
            }
        }
        GameObject obj = Instantiate(fire, head.transform.position, head.transform.rotation) as GameObject; // cabeza -> head
        pool.Add(obj);
        return obj;
    }


    public void FlamethrowerSkill() // LanzaLlamas_Skill -> FlamethrowerSkill
    {
        timer2 += 1 * Time.deltaTime; // cronometro2 -> timer2
        if (timer2 > 0.1f) // cronometro2 -> timer2
        {
            GameObject obj = GetBullet();
            obj.transform.position = head.transform.position; // cabeza -> head
            obj.transform.rotation = head.transform.rotation; // cabeza -> head
            timer2 = 0; // cronometro2 -> timer2
        }
    }

    public void StartFire() // Start_Fire -> StartFire
    {
        flamethrower = true; // lanza_llamas -> flamethrower
    }

    public void StopFire() // Stop_Fire -> StopFire
    {
        flamethrower = false; // lanza_llamas -> flamethrower
    }

    ////// Fire Ball //////

    public GameObject GetFireBall() // Get_Fire_Ball -> GetFireBall (camelCase)
    {
        for (int i = 0; i < pool2.Count; i++)
        {
            if (!pool2[i].activeInHierarchy)
            {
                pool2[i].SetActive(true);
                return pool2[i];
            }
        }
        GameObject obj = Instantiate(fireBall, point.transform.position, point.transform.rotation) as GameObject; // fire_ball -> fireBall (camelCase)
        pool2.Add(obj);
        return obj;
    }

    ////// //////

    public void FireBallSkill() // Fire_Ball_Skill -> FireBallSkill (camelCase)
    {
        GameObject obj = GetFireBall(); // Get_Fire_Ball -> GetFireBall (camelCase)
        obj.transform.position = point.transform.position;
        obj.transform.rotation = point.transform.rotation;
    }

    public void Alive() // Vivo -> Alive
    {
        if (HP_Min < 500)
        {
            phase = 2; // fase -> phase
            routineTime = 1; // time_rutinas -> routineTime
        }

        BossBehavior(); // Comportamiento_Boss -> BossBehavior

        if (flamethrower) // lanza_llamas -> flamethrower
        {
            FlamethrowerSkill(); // Lanzallamas_Skill -> FlamethrowerSkill
        }
    }
    void Update()
    {
        bar.fillAmount = HP_Min / HP_Max; // barra -> bar

        if (HP_Min > 0)
        {
            Alive(); // Vivo -> Alive
        }
        else
        {
            if (!dead) // muerto -> dead
            {
                ani.SetTrigger("dead"); // ani -> anim
                music.enabled = false; // musica -> music
                dead = true; // muerto -> dead
            }
        }
    }
}
