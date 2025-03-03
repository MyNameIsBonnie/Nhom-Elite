using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

public class BossMovement : MonoBehaviour
{
    public Animator animator;
    public float timeBetweenSpells = 3f; // Time before casting another spell
    public GameObject spell1Prefab; // Prefabs for your spells
    public GameObject spell2Prefab;
    public GameObject spell3Prefab;
    public GameObject spell4Prefab;
    public GameObject spell5Prefab;
    public Transform spellSpawnPoint; // Where the spells originate
    private BossDetection detection;
    private float nextSpellTime;

    void Start()
    {
        animator = GetComponent<Animator>();
        nextSpellTime = Time.time + timeBetweenSpells;
        detection = new BossDetection();
    }

    void Update()
    {
        if (detection.playerInRange == true)
        {
            if (Time.time >= nextSpellTime)
            {
                CastRandomSpell();
                nextSpellTime = Time.time + timeBetweenSpells;
            }
        }
        else
        {
            animator.SetTrigger("Idle");
        }
    }

    void CastRandomSpell()
    {
        int randomSpell = Random.Range(1, 6); // Generate a random number between 1 and 5

        switch (randomSpell)
        {
            case 1:
                CastSpell(1, "CastSpell1", spell1Prefab);
                break;
            //case 2:
            //    CastSpell(2, "CastSpell2", spell2Prefab);
            //    break;
            //case 3:
            //    CastSpell(3, "CastSpell3", spell3Prefab);
            //    break;
            //case 4:
            //    CastSpell(4, "CastSpell4", spell4Prefab);
            //    break;
            //case 5:
            //    CastSpell(5, "CastSpell5", spell5Prefab);
            //    break;
        }
    }

    void CastSpell(int spellNumber, string triggerName, GameObject spellPrefab)
    {
        animator.SetTrigger(triggerName);

        // Instantiate the spell prefab at the spawn point.
        if (spellPrefab != null && spellSpawnPoint != null)
        {
            Instantiate(spellPrefab, spellSpawnPoint.position, spellSpawnPoint.rotation);
        }

        Debug.Log("Casting Spell " + spellNumber);

        // Add any spell-specific logic here (e.g., damage, effects).
    }
}
