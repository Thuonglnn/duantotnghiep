using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using UnityEngine;

public class DamageSystem : MonoBehaviour
{
    public float damageAmount = 20f;
    public float CritDamage = 120f;
    public float CritRate = 20f;


    void Start()
    {
        CritDamage = CritDamage / 100;
        CritRate = CritRate / 100;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "SpiderQueen":
                other.GetComponent<SpiderQueenAI>().TakeDamage(damageAmount, CritRate, CritDamage);
                Destroy(gameObject);
                break;
            case "SpiderGreen":
                other.GetComponent<SpiderGreenAI>().TakeDamage(damageAmount, CritRate, CritDamage);
                Destroy(gameObject);
                break;
            case "SpiderFlower":
                other.GetComponent<SpiderFlowerAI>().TakeDamage(damageAmount, CritRate, CritDamage);
                Destroy(gameObject);
                break;
            case "GhoulZombie":
                other.GetComponent<GhoulZombieAI>().TakeDamage(damageAmount, CritRate, CritDamage);
                Destroy(gameObject);
                break;
            case "Goblin":
                other.GetComponent<GoblinAI>().TakeDamage(damageAmount, CritRate, CritDamage);
                Destroy(gameObject);
                break;
            case "CaveTroll":
                other.GetComponent<CaveTrollAI>().TakeDamage(damageAmount, CritRate, CritDamage);
                Destroy(gameObject);
                break;
            default:
                Destroy(gameObject, 10f);
                break;
        }
    }
}
