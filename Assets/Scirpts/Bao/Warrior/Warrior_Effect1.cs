using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warrior_Effect : MonoBehaviour
{
    public ParticleSystem NormalAttack_1;
    public ParticleSystem SkillAttack_1_1;
    public ParticleSystem SkillAttack_1_2;

    public ParticleSystem SkillAttack_1_3;

    public ParticleSystem SkillAttack_2;

    public ParticleSystem SkillAttack_3;
    public ParticleSystem SkillAttack_3_1;

    public Transform Character ;


    // public Transform axe ;


    // Start is called before the first frame update
    void Start()
    {
        NormalAttack_1.Stop();
        
        SkillAttack_1_1.Stop();
        SkillAttack_1_2.Stop();
        SkillAttack_1_3.Stop();
        SkillAttack_2.Stop();
        SkillAttack_3.Stop();

    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void normalAttack1 ()
    {
        NormalAttack_1.Play();
    }
    void skillAttack1_1 ()
    {
        SkillAttack_1_1.Play();
    }

    void skillAttack1_2 ()
    {
        SkillAttack_1_2.Play();
    }

    void skillAttack1_3 ()
    {
        SkillAttack_1_3.Play();
    }


     void skillAttack2()
    {
        SkillAttack_2.Play();
    } 
    
    void skillAttack3 ()
    {
        SkillAttack_3.Play();
    }

    void skillAttack3_1 ()
    {
       
        Instantiate(SkillAttack_3_1,  Character.position, Character.rotation);
    }

    void skillAttack3_stop ()
    {
        SkillAttack_3.Stop();
    }
    
}
