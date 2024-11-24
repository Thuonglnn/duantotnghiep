using UnityEngine;
using UnityEngine.UI;

public class TimerScript : MonoBehaviour
{
    public float startTime = 60f; // thời gian bắt đầu tính (tính bằng giây)
    private float timeRemaining;
    public Text timerText; // kéo Text UI vào đây trong Inspector

    void Start()
    {
        timeRemaining = startTime;
    }

    void Update()
    {
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            UpdateTimerUI();
        }
        else
        {
            timeRemaining = 0;
            UpdateTimerUI();
        }
    }

    void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);
        timerText.text = $"{minutes:00}:{seconds:00}";
    }
}
