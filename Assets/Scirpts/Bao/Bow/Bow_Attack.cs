using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
  
    public float[] cooldownTimes = { 10f, 8f, 12f }; 
    float[] cooldownTimers = { 0f, 0f, 0f };  
    public static bool[] isCooldowns = { false, false, false }; 
    public TextMeshProUGUI [] tmpCooldownTimers;

    float TimeSkill_1 ;
    bool Skill1 = false;

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
        
        if(Input.GetKey(KeyCode.Q) && !isCooldowns[0])
        {
            Skill1 = true;
            TimeSkill_1 = Time.time;
            SkillAttack_1.Play();
            CastSkill(0);
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

        if(Input.GetKey(KeyCode.E) && !Bow_CTRL.isAiming && !isCooldowns[1])
        {
            animator.SetBool("Skill2",true);
            CastSkill(1);
        }
        if(Input.GetKeyDown(KeyCode.R) && Bow_CTRL.isAiming && !isCooldowns[2])
        {
            animator.SetBool("isAttacking", true);
            for (int i = 0; i < 1; i++)
            {          
                Instantiate(BigIceArrow, transformArrow.position, transformArrow.rotation);
            }
            CastSkill(2);
        }
        
        for (int i = 0; i < 3; i++)
        {
            if (isCooldowns[i])
            {
                cooldownTimers[i] -= Time.deltaTime;
                tmpCooldownTimers[i].text ="" + Mathf.Ceil(cooldownTimers[i]);

                if (cooldownTimers[i] <= 0)
                {
                    isCooldowns[i] = false;
                    tmpCooldownTimers[i].text = "";
                }
            }
        }
        
    }
    

    void CastSkill(int skillIndex)
    {
        isCooldowns[skillIndex] = true;
        cooldownTimers[skillIndex] = cooldownTimes[skillIndex];
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
