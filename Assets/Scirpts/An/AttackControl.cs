using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackControl : MonoBehaviour
{
    private Animator anim;
    private void Awake()
    {
        anim = GetComponent<Animator>(); 
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetBool("Skill1", false);
        anim.SetBool("Skill2", false);
        anim.SetBool("Skill3", false);
        if (Input.GetKey(KeyCode.Mouse0))
        {
            anim.SetTrigger("Slash");
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            anim.SetTrigger("Block");
        }
        if (Input.GetKey(KeyCode.Q))
        {
            anim.SetBool("Skill1",true);
        }
        if (Input.GetKey(KeyCode.E))
        {
            anim.SetBool("Skill2", true);
        }
        if (Input.GetKey(KeyCode.R))
        {
            anim.SetBool("Skill3", true);
        }
    }
}
