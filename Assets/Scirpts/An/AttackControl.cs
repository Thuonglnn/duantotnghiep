using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackControl : MonoBehaviour
{
    private Animator anim;
    public CooldownDisplay cooldownDisplay;
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
        // Reset các trạng thái kỹ năng trong Animator
        ResetSkillAnimators();

        // Kiểm tra input của người chơi
        HandleInput();
        
    }
    private void ResetSkillAnimators()
    {
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);

        if (cooldownDisplay.IsSkillReady(1) && !stateInfo.IsName("Skill1"))
        {
            anim.SetBool("Skill1", false);
            SetSkillActive(1, false); // Reset trạng thái kỹ năng
        }
        if (cooldownDisplay.IsSkillReady(2) && !stateInfo.IsName("Skill2"))
        {
            anim.SetBool("Skill2", false);
            SetSkillActive(2, false); // Reset trạng thái kỹ năng
        }
        if (cooldownDisplay.IsSkillReady(3) && !stateInfo.IsName("Skill3"))
        {
            anim.SetBool("Skill3", false);
            SetSkillActive(3, false); // Reset trạng thái kỹ năng
        }
    }
    private void HandleInput()
    {
        // Kiểm tra tấn công cơ bản (Click chuột trái)
        if (Input.GetKey(KeyCode.Mouse0))
        {
            anim.SetTrigger("Slash");
        }

        // Kiểm tra động tác phòng thủ (phím C)
        if (Input.GetKey(KeyCode.C))
        {
            anim.SetTrigger("Block");
        }

        // Kiểm tra các kỹ năng với hồi chiêu
        if (Input.GetKeyDown(KeyCode.Q) && cooldownDisplay.IsSkillReady(1))
        {
            ActivateSkill(1, "Skill1");
        }
        else if (Input.GetKeyDown(KeyCode.E) && cooldownDisplay.IsSkillReady(2))
        {
            ActivateSkill(2, "Skill2");
        }
        else if (Input.GetKeyDown(KeyCode.R) && cooldownDisplay.IsSkillReady(3))
        {
            ActivateSkill(3, "Skill3");
        }
    }
    private bool isSkill1Active = false;
    private bool isSkill2Active = false;
    private bool isSkill3Active = false;

private void ActivateSkill(int skillNumber, string skillName)
{
    if (cooldownDisplay.IsSkillReady(skillNumber) && !IsSkillActive(skillNumber))
    {
        anim.SetTrigger(skillName);
        AttackWithSkill(skillNumber);
        SetSkillActive(skillNumber, true); // Đặt trạng thái kỹ năng đang hoạt động
    }
}


private void SetSkillActive(int skillNumber, bool isActive)
{
    switch (skillNumber)
    {
        case 1: isSkill1Active = isActive; break;
        case 2: isSkill2Active = isActive; break;
        case 3: isSkill3Active = isActive; break;
    }
}

private bool IsSkillActive(int skillNumber)
{
    switch (skillNumber)
    {
        case 1: return isSkill1Active;
        case 2: return isSkill2Active;
        case 3: return isSkill3Active;
        default: return false;
    }
}

// Khi kỹ năng hoàn thành hoặc hồi chiêu kết thúc, reset trạng thái
private void OnSkillComplete(int skillNumber)
{
    SetSkillActive(skillNumber, false);
}

    void AttackWithSkill(int skillNumber)
    {
    // Thực hiện logic tấn công kỹ năng
    cooldownDisplay.UseSkill(skillNumber); // Kích hoạt hồi chiêu
    }
    public void Skill1Complete()
    {
        OnSkillComplete(1); // Kỹ năng 1 hoàn thành
    }
    public void Skill2Complete()
    {
        OnSkillComplete(2); // Kỹ năng 2 hoàn thành
    }
    public void Skill3Complete()
    {
        OnSkillComplete(3); // Kỹ năng 3 hoàn thành
    }


}

