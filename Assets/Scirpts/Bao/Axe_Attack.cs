using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe_Attack : MonoBehaviour
{
    Animator animator;
    int attackCount;

    public float comboResetTime = 0.5f;
    public float resetAttackTime = 0.3f;
    float comboAttackTime;
    
    public static bool isWalk;
    void Start()
    {
        animator = GetComponent<Animator>();
        attackCount = 0;
        
    }

    // Update is called once per frame
    void Update()
    {
        isWalk = true;
        animator.SetBool("Skill1", false);
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
        
        if(Input.GetKey(KeyCode.Q))
        {
            animator.SetBool("Skill1", true);
            
        }
        
    }

    public static Axe_Attack instance;

    void Awake()
    {
        instance = this;
    }
}
