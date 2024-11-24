using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillQ : MonoBehaviour
{

    [SerializeField] private Transform vfxHitGreen;
    [SerializeField] private Transform vfxHitRed;

    private Rigidbody SkillQ1;

    private void Awake()
    {
        SkillQ1 = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        float speed = 500f;
        SkillQ1.velocity = transform.forward * speed * Time.deltaTime;
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
        Destroy(gameObject, 5.0f);
    }

}