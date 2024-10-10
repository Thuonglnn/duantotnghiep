using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bow_Attack : MonoBehaviour
{
    Animator animator;

    public GameObject arrowModel;
    public GameObject Arrow;
    public GameObject ArrowIce;
    public GameObject BigIceArrow;


    public Transform transformArrow;

    public ParticleSystem SkillAttack_1;


    bool Skill1 = false;
    float TimeSkill_1 ;
    void Start()
    {
        animator = GetComponent<Animator>();
        SkillAttack_1.Stop();
        arrowModel.SetActive(false);
        TimeSkill_1 = 0;
    }

    void Update()
    {
        
        animator.SetBool("isAttacking", false);
        animator.SetBool("Skill2",false);
        animator.SetBool("Skill3",false);

        if(Input.GetMouseButtonDown(1))
        {
            animator.SetBool("DrawArrow",true);
        }
        if(Input.GetMouseButtonUp(1))
        {
            animator.SetBool("DrawArrow",false);
            arrowModel.SetActive(false);

        }

        if (Input.GetMouseButtonDown(0))
        {
            animator.SetBool("isAttacking", true);
           
        }

        
        if(Input.GetKey(KeyCode.Q))
        {
            Skill1 = true;
            TimeSkill_1 = Time.time;
            SkillAttack_1.Play();
        }
        
        if(Skill1)
        {
            if(Time.time - TimeSkill_1 > 4f)
            {
                Skill1 = false;
                TimeSkill_1 = 0;
                SkillAttack_1.Stop();
            }
        }

        if(Input.GetKey(KeyCode.E) && !Bow_CTRL.isAiming)
        {
            animator.SetBool("Skill2",true);
        }
        if(Input.GetKeyDown(KeyCode.R) && Bow_CTRL.isAiming)
        {
            animator.SetBool("isAttacking", true);
            for (int i = 0; i < 1; i++)
            {          
                Instantiate(BigIceArrow, transformArrow.position, transformArrow.rotation);
            }
        }
        
        
    }
    

    void SetActiveArrowTrue ()
    {
        arrowModel.SetActive(true);

        if(Skill1)
        {
            SkillAttack_1.Play();
        }
        else
        {
            SkillAttack_1.Stop();
        }
    }
    void SetActiveArrowfalse ()
    {
        arrowModel.SetActive(false);
        if(Skill1)
        {
            SkillAttack_1.Play();
        }
        else
        {
            SkillAttack_1.Stop();
        }
    }

    void Attacking ()
    {
        for (int i = 0; i < 1; i++)
        {   
            //Instantiate(Arrow, arrowModel.transform.position, arrowModel.transform.rotation);

            if(Skill1)
            {
                Instantiate(ArrowIce, transformArrow.position, transformArrow.rotation);
            }
            else
            {
                Instantiate(Arrow, transformArrow.position, transformArrow.rotation);
            }
            

        }
    }

}
