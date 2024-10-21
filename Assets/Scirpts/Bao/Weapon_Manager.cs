using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Manager : MonoBehaviour
{
    public GameObject weapon;
    public GameObject Skill3;
    void Start()
    {
        weapon.GetComponent<Collider>().enabled = false;
    }

    void OnCollider ()
    {
        weapon.GetComponent<Collider>().enabled = true;
    }
    void OffCollider ()
    {
        weapon.GetComponent<Collider>().enabled = false;
    }

    void OnColliderSkill3 ()
    {
        Skill3.GetComponent<Collider>().enabled = true;
    }
    void OffColliderSkill3 ()
    {
        Skill3.GetComponent<Collider>().enabled = false;
    }
}
