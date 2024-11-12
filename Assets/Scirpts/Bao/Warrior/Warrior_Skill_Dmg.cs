using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Warrior_Skill_Dmg : NetworkBehaviour
{
    AttributesManager player;


    public float damageInterval = 1f;
    private float lastDamageTime = 0f; 

    void Start()
    {
        if(IsOwner)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
            {
                player = playerObject.GetComponent<AttributesManager>();
            }
        }
        
        DestroyAfterTime();
    }

    void OnTriggerStay (Collider other)
    {
        if(IsOwner)
        {
            if(other.CompareTag("Player"))
            {
                AttributesManager enemy = other.GetComponent<AttributesManager>();
                if (enemy != null && Time.time >= lastDamageTime + damageInterval)
                {
                    player.DealDmg(enemy.gameObject, player.atk-3);

                    lastDamageTime = Time.time;
                }
            }
        }
        
        
    }

    void DestroyAfterTime()
    {
        Destroy(gameObject, 3f);
    }


}
