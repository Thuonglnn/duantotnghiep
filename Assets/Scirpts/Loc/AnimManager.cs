using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AnimManager : MonoBehaviour
{
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void AnimateCharacter(float _horizontal, float _vertical, bool _sprinting)
    {
        anim.SetFloat("Horizontal", _horizontal);
        anim.SetFloat("Vertical", _vertical);
        anim.SetBool("Sprint", _sprinting);
    }

    public void CharacterAim(bool _isAiming)
    {
        anim.SetBool("Aim", _isAiming);
    }

    public void CharacterPullString(bool _pull)
    {
        anim.SetBool("PullString", _pull);
    }

    public void CharacterFireArrow()
    {
        anim.SetTrigger("Fire");
    }
}
