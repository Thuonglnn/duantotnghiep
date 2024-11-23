using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class Axe_Skill3_Dmg : NetworkBehaviour
{
    public AttributesManager player;

    // thoi gian tre khi gay dmg thieu dot
    public float damageInterval = 0.5f;
    // tung quai vat se co thoi gian gay dmg rieng
    private Dictionary<AttributesManager, float> enemyLastDamageTime = new Dictionary<AttributesManager, float>();

    void Start()
    {
        // if(IsOwner)
        // {
        //     GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        //     if (playerObject != null)
        //     {
        //         player = playerObject.GetComponent<AttributesManager>();
        //     }
        // }

    }

    void OnTriggerStay(Collider other)
    {
        if (IsOwner)
        {
            if (other.gameObject.CompareTag("Player1"))
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

            if (other.gameObject.CompareTag("Enemy"))
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



}
