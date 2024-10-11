using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe_Effect : MonoBehaviour
{
    public ParticleSystem NormalAttack_1;
    public ParticleSystem NormalAttack_2;
    public ParticleSystem SkillAttack_1;
    public ParticleSystem SkillAttack_2;

    public ParticleSystem SkillAttack_3;
    public Transform axe ;


    // Start is called before the first frame update
    void Start()
    {
        NormalAttack_1.Stop();
        NormalAttack_2.Stop();
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
    void normalAttack2 ()
    {
        NormalAttack_2.Play();
    }

    void skillAttack1 ()
    {
        
        Instantiate(SkillAttack_1, axe.position,Quaternion.identity);
    }
     void skillAttack2()
    {
        SkillAttack_2.Play();
    } void skillAttack3 ()
    {
        SkillAttack_3.Play();
    }
    
}
