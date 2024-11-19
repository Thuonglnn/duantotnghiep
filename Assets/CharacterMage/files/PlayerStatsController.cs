using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PlayerStatsController : MonoBehaviour
{
    // public Slider healthSlider;
    // public Slider manaSlider;

    // public int maxHealth = 100;
    // public int maxMana = 100;

    // private int currentHealth;
    // private int currentMana;

    // Enum đại diện cho các kỹ năng
    public enum Skill { Q, E, R }

    // Thời gian cooldown của từng kỹ năng
    private Dictionary<Skill, float> skillCooldownTimes = new Dictionary<Skill, float>()
    {
        { Skill.Q, 5f },    // Fireball có cooldown là 5 giây
        { Skill.E, 8f },    // IceBlast có cooldown là 8 giây
        { Skill.R, 30f } // ThunderStrike có cooldown là 10 giây
    };

    // Kiểm tra trạng thái cooldown cho mỗi kỹ năng
    private Dictionary<Skill, bool> skillCooldownStatus = new Dictionary<Skill, bool>()
    {
        { Skill.Q, false },
        { Skill.E, false },
        { Skill.R, false }
    };

    // Phương thức để kiểm tra trạng thái cooldown của một kỹ năng
    public bool IsSkillOnCooldown(Skill skill)
    {
        return skillCooldownStatus.ContainsKey(skill) && skillCooldownStatus[skill];
    }

    // Tham chiếu đến Slider UI cho cooldown của từng kỹ năng
    public Slider qCooldownSlider;
    public Slider eCooldownSlider;
    public Slider rCooldownSlider;

    void Start()
    {
        // currentHealth = maxHealth;
        // currentMana = maxMana;

        // healthSlider.maxValue = maxHealth;
        // manaSlider.maxValue = maxMana;

        // healthSlider.value = currentHealth;
        // manaSlider.value = currentMana;

        //StartCoroutine(RegenerateManaOverTime());

        // Khởi tạo giá trị maxValue cho các Slider cooldown
        qCooldownSlider.maxValue = skillCooldownTimes[Skill.Q];
        eCooldownSlider.maxValue = skillCooldownTimes[Skill.E];
        rCooldownSlider.maxValue = skillCooldownTimes[Skill.R];

        // Ẩn các Slider cooldown khi bắt đầu game
        qCooldownSlider.gameObject.SetActive(false);
        eCooldownSlider.gameObject.SetActive(false);
        rCooldownSlider.gameObject.SetActive(false);
    }

    // public void TakeDamage(int damage)
    // {
    //     currentHealth -= damage;
    //     currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
    //     healthSlider.value = currentHealth;
    // }

    // public void Heal(int amount)
    // {
    //     currentHealth += amount;
    //     currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
    //     healthSlider.value = currentHealth;
    // }

    public void UseSkill(Skill skill, int manaCost)
    {
        if (skillCooldownStatus[skill]) //|| currentMana < manaCost
        {
            Debug.Log("Skill is on cooldown or not enough mana.");
            return;
        }

        // Giảm mana và bắt đầu cooldown cho kỹ năng
        // currentMana -= manaCost;
        // currentMana = Mathf.Clamp(currentMana, 0, maxMana);
        // manaSlider.value = currentMana;

        StartCoroutine(SkillCooldownRoutine(skill));
    }

    // private IEnumerator RegenerateManaOverTime()
    // {
    //     while (true)
    //     {
    //         yield return new WaitForSeconds(1f);
    //         RegenerateMana(1);
    //     }
    // }

    // public void RegenerateMana(int amount)
    // {
    //     currentMana += amount;
    //     currentMana = Mathf.Clamp(currentMana, 0, maxMana);
    //     manaSlider.value = currentMana;
    // }

    // Coroutine để xử lý cooldown của kỹ năng
    private IEnumerator SkillCooldownRoutine(Skill skill)
    {
        skillCooldownStatus[skill] = true;

        Slider cooldownSlider = null;
        float cooldownTime = skillCooldownTimes[skill];

        // Chọn Slider tương ứng với kỹ năng
        switch (skill)
        {
            case Skill.Q:
                cooldownSlider = qCooldownSlider;
                break;
            case Skill.E:
                cooldownSlider = eCooldownSlider;
                break;
            case Skill.R:
                cooldownSlider = rCooldownSlider;
                break;
        }

        if (cooldownSlider != null)
        {
            cooldownSlider.gameObject.SetActive(true);
            cooldownSlider.value = cooldownTime;

            // Đếm ngược thời gian cooldown trên Slider
            while (cooldownSlider.value > 0)
            {
                cooldownSlider.value -= Time.deltaTime;
                yield return null;
            }

            cooldownSlider.gameObject.SetActive(false); // Ẩn Slider khi hết cooldown
        }

        skillCooldownStatus[skill] = false;
    }
}

