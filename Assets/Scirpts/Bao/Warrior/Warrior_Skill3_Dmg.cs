using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Warrior_Skill3_Dmg : NetworkBehaviour
{
    AttributesManager player;

    public float damageInterval = 0.5f;
    private Dictionary<AttributesManager, float> enemyLastDamageTime = new Dictionary<AttributesManager, float>();

    void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.GetComponent<AttributesManager>();
        }
        DestroyAfterTime();
    }

    void OnTriggerStay (Collider other)
    {
        if(!IsOwner)
        {
            if(other.gameObject.CompareTag("Player") )
            {
                AttributesManager enemy = other.GetComponent<AttributesManager>();
                if (enemy != null)
                {
                    if (!enemyLastDamageTime.ContainsKey(enemy))
                    {
                        enemyLastDamageTime[enemy] = 0f;
                    }

                    if (Time.time >= enemyLastDamageTime[enemy] + damageInterval)
                    {
                        player.DealDmg(enemy.gameObject, player.atk - 4);
                        enemyLastDamageTime[enemy] = Time.time;
                    }
                }
            }
        }
        
    }

    void DestroyAfterTime()
    {
        Destroy(gameObject, 3f);
    }

}
