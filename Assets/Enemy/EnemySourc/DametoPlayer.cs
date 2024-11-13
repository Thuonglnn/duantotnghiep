using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DametoPlayer : MonoBehaviour
{
    public int damageAmount = 20;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Destroy(gameObject, 2f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerStatsController>().TakeDamage(damageAmount);
            Destroy(gameObject);
        }

    }
}
