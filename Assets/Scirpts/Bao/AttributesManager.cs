using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttributesManager : MonoBehaviour
{
    public int hp = 100;
    public int def = 100;
    public int atk = 10;
    public float critRate = 0.5f; //50%
    public float critDamage = 2f; //200%

    public Slider healthBar;
    void Start ()
    {
        if(healthBar != null)
        {
            healthBar.maxValue = hp;
            healthBar.minValue = 0;
        }
    }
    void Update ()
    {   
        if(healthBar != null)
        {
            healthBar.value = hp;
        }
        
    }

    public void TakeDmg(int amount,bool isCrit)
    {
        hp -= amount-def;
        if(isCrit == false)
        {
            DmgPopUpGerenator.current.CreaterPopUp(transform.position, amount-def + "", Color.red);
        }
        else
        {
            DmgPopUpGerenator.current.CreaterPopUpCrit(transform.position, amount-def + "", Color.yellow);
        }
        
    }

    public void DealDmg(GameObject target, int attack )
    {
        var atm = target.GetComponent<AttributesManager>();

        if(atm != null)
        {
            bool isCrit = false;
            int finalDamage = attack;

            if (Random.value <= critRate)
            {
                finalDamage = Mathf.RoundToInt(attack * critDamage);
                isCrit = true;
            }
            
            atm.TakeDmg(finalDamage,isCrit);
        }
    }
}
