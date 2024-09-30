using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe_Attack : MonoBehaviour
{
    Animator animator;
    public static int attackCount;

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
        animator.SetBool("Skill2", false);
        animator.SetBool("Skill3", false);

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
        if(Input.GetKey(KeyCode.E))
        {
            animator.SetBool("Skill2", true);
        }
        if(Input.GetKey(KeyCode.R))
        {
            animator.SetBool("Skill3", true);
        }
        if(Input.GetKeyDown(KeyCode.F))
        {
            animator.SetTrigger("Blocking");
        }
        
    }

    public static Axe_Attack instance;

    void Awake()
    {
        instance = this;
    }
}
