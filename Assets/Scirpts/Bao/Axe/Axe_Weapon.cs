using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe_Weapon : MonoBehaviour
{
    public AttributesManager player;
    //public AttributesManager enemy;

    void OnTriggerEnter(Collider other)
    {
        AttributesManager enemy = other.GetComponent<AttributesManager>();

        if (enemy != null) 
        {
            player.DealDmg(enemy.gameObject);
        }
    }


}
