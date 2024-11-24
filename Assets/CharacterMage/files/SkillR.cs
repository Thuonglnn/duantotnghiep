using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillR : MonoBehaviour
{

    [SerializeField] private Transform vfxHitGreen;
    [SerializeField] private Transform vfxHitRed;

    private Rigidbody SkillR1;

    private void Awake()
    {
        SkillR1 = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        // float speed = 50f;
        // SkillR1.velocity = transform.forward * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<BulletTarget>() != null)
        {
            // Hit target
            Instantiate(vfxHitGreen, transform.position, Quaternion.identity);
        }
        else
        {
            // Hit something else
            Instantiate(vfxHitRed, transform.position, Quaternion.identity);
        }
        Destroy(gameObject, 20.0f);
    }

}