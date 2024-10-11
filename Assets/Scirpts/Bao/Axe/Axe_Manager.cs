using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe_Manager : MonoBehaviour
{
    public GameObject weapon;
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
}
