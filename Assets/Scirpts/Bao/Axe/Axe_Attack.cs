using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Axe_Attack : MonoBehaviour
{
    Animator animator;
    public static int attackCount;

    public float comboResetTime = 0.5f;
    public float resetAttackTime = 0.3f;
    float comboAttackTime;

    public float[] cooldownTimes = { 10f, 8f, 12f }; 
    float[] cooldownTimers = { 0f, 0f, 0f };  
    public static bool[] isCooldowns = { false, false, false }; 
    public TextMeshProUGUI [] tmpCooldownTimers;
    
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
        
        if(Input.GetKey(KeyCode.Q) && !isCooldowns[0])
        {
            animator.SetBool("Skill1", true);
            CastSkill(0);
        }
        if(Input.GetKey(KeyCode.E) && !isCooldowns[1])
        {
            animator.SetBool("Skill2", true);
            CastSkill(1);
        }
        if(Input.GetKey(KeyCode.R) && !isCooldowns[2])
        {
            animator.SetBool("Skill3", true);
            CastSkill(2);
        }
        if(Input.GetKeyDown(KeyCode.F))
        {
            animator.SetTrigger("Blocking");
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

    public static Axe_Attack instance;

    void Awake()
    {
        instance = this;
    }
}
