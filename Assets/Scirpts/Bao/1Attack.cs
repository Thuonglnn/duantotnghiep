using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    Animator animator;
    
    int attackCount;

    public float comboResetTime = 0.5f;
    public float resetAttackTime = 0.3f;
    float comboAttackTime;
    
    void Start()
    {
        animator = GetComponent<Animator>();

        attackCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - comboAttackTime > comboResetTime)
        {
            attackCount = 0;
            animator.SetInteger("AttackCount", 0); 
        }
        
       if (Input.GetMouseButtonDown(0) && Time.time - comboAttackTime > resetAttackTime)
        {
            attackCount++; 

            if (attackCount > 2)
            {
                attackCount = 0; 
            }

            comboAttackTime = Time.time;

            animator.SetInteger("AttackCount", attackCount);
        }
        
        
    }
}
