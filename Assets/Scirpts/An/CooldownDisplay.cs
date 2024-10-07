using UnityEngine;
using UnityEngine.UI;

public class CooldownDisplay : MonoBehaviour
{
    public Image skill1Image;
    public Image skill2Image;
    public Image skill3Image;

    public float skill1CooldownTime = 2f;
    public float skill2CooldownTime = 5f;
    public float skill3CooldownTime = 10f;

    private float skill1CooldownTimer = 0f;
    private float skill2CooldownTimer = 0f;
    private float skill3CooldownTimer = 0f;

    void Update()
    {
        UpdateCooldown(ref skill1CooldownTimer, skill1Image, skill1CooldownTime);
        UpdateCooldown(ref skill2CooldownTimer, skill2Image, skill2CooldownTime);
        UpdateCooldown(ref skill3CooldownTimer, skill3Image, skill3CooldownTime);
    }

    void UpdateCooldown(ref float cooldownTimer, Image skillImage, float cooldownTime)
    {
        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
            // Giảm độ mờ (opacity) của hình ảnh khi hồi chiêu
            skillImage.fillAmount = cooldownTimer / cooldownTime; // Giả sử tất cả kỹ năng dùng hình ảnh dạng "fill"
            skillImage.color = new Color(1, 1, 1, 0.5f); // Làm mờ hình ảnh (alpha = 0.5)
        }
        else
        {
            // Đặt lại opacity của hình ảnh khi kỹ năng sẵn sàng
            skillImage.fillAmount = 1;
            skillImage.color = new Color(1, 1, 1, 1f); // Đặt alpha = 1 để hiện rõ lại
        }
    }

    public bool IsSkillReady(int skillNumber)
    {
        switch (skillNumber)
        {
            case 1: return skill1CooldownTimer <= 0;
            case 2: return skill2CooldownTimer <= 0;
            case 3: return skill3CooldownTimer <= 0;
            default: return false;
        }
    }

    public void UseSkill(int skillNumber)
    {
        switch (skillNumber)
        {
            case 1:
                if (IsSkillReady(1))
                {
                    skill1CooldownTimer = skill1CooldownTime;
                    // Gọi hiệu ứng hoặc chức năng của chiêu 1
                    TriggerSkill1Effect();
                }
                break;
            case 2:
                if (IsSkillReady(2))
                {
                    skill2CooldownTimer = skill2CooldownTime;
                    // Gọi hiệu ứng hoặc chức năng của chiêu 2
                    TriggerSkill2Effect();
                }
                break;
            case 3:
                if (IsSkillReady(3))
                {
                    skill3CooldownTimer = skill3CooldownTime;
                    // Gọi hiệu ứng hoặc chức năng của chiêu 3
                    TriggerSkill3Effect();
                }
                break;
        }
    }

    void TriggerSkill1Effect()
    {
        Debug.Log("Skill 1 Activated");
    }

    void TriggerSkill2Effect()
    {
        Debug.Log("Skill 2 Activated");
    }

    void TriggerSkill3Effect()
    {
        Debug.Log("Skill 3 Activated");
    }
}
