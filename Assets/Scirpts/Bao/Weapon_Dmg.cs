using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Dmg : MonoBehaviour
{
    AttributesManager player;
    

    void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.GetComponent<AttributesManager>();
        }

    }

    void OnTriggerEnter(Collider other)
    {

        if(other.CompareTag("Enemy"))
        {
            AttributesManager  enemy = other.GetComponent<AttributesManager>();
            player.DealDmg(enemy.gameObject,player.atk);
        }
        
    }


}
