using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow_Effect : MonoBehaviour
{

    public ParticleSystem SkillAttack_3;

    public Transform bow ;


    void Start()
    {
        SkillAttack_3.Stop();

        
    }

    void Update()
    {
        
    }

    void skillAttack3 ()
    {
        Instantiate(SkillAttack_3, bow.position,bow.rotation);
    }
    
    
}
