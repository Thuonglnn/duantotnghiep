using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warrior_Attack : MonoBehaviour
{
    Animator animator;

    public float comboResetTime = 0.5f;
    public float resetAttackTime = 0.3f;
    float comboAttackTime;

    
    public static bool isWalk;
    void Start()
    {
        animator = GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {
        isWalk = true;
        animator.SetBool("Skill1", false);
        animator.SetBool("Skill2", false);
        animator.SetBool("Skill3", false);
        animator.SetBool("isAttacking",false);

        
        
       if (Input.GetMouseButtonDown(0) && Time.time - comboAttackTime > resetAttackTime)
        {
           animator.SetBool("isAttacking",true);
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

    
}
