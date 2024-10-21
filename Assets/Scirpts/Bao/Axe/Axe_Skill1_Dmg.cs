using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe_Skill1_Dmg : MonoBehaviour
{
    AttributesManager player;

    // thoi gian tre khi gay dmg thieu dot
    public float damageInterval = 1f;
    // tung quai vat se co thoi gian gay dmg rieng
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
        if(other.gameObject.CompareTag("Enemy") )
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

    void DestroyAfterTime()
    {
        Destroy(gameObject, 3f);
    }


}
