using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResetSkill : MonoBehaviour
{
    public float[] cooldownTimes = { 10f, 8f, 12f }; 
    float[] cooldownTimers = { 0f, 0f, 0f };  
    public static bool[] isCooldowns = { false, false, false }; 

    public TextMeshProUGUI [] tmpCooldownTimers;

    
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Q) && !isCooldowns[0])
        {
            CastSkill(0); 
        }
        if (Input.GetKeyDown(KeyCode.E) && !isCooldowns[1])
        {
            CastSkill(1); 
        }
        if (Input.GetKeyDown(KeyCode.R) && !isCooldowns[2])
        {
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
                }
            }
        }
    }

    
    void CastSkill(int skillIndex)
    {
        isCooldowns[skillIndex] = true;
        cooldownTimers[skillIndex] = cooldownTimes[skillIndex];
    }

    public static ResetSkill instance;
    void Awake()
    {
        instance = this;
    }
}
