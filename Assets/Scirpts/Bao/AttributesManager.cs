using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttributesManager : MonoBehaviour
{
    public int hp = 100;
    public int def = 100;
    public int atk = 10;

    Slider healthBar;
    void Start ()
    {
        GameObject healthBarObject = GameObject.FindWithTag("HealthBar");
        if (healthBarObject != null)
        {
            healthBar = healthBarObject.GetComponent<Slider>();
            healthBar.maxValue = hp;
            healthBar.minValue = 0;
        }
    }
    void Update ()
    {
        healthBar.value = hp;
    }

    public void TakeDmg(int amount)
    {
        hp -= amount-def;
    }

    public void DealDmg(GameObject target)
    {
        var atm = target.GetComponent<AttributesManager>();

        if(atm != null)
        {
            atm.TakeDmg(atk);
        }
    }
}
