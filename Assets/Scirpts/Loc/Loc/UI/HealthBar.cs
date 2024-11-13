using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Slider easeHealthSlider;
    [SerializeField] private float maxHealth = 1000f;
    [SerializeField] private float health;
    private float lerpSpeed = 0.05f;

    private void Start()
    {
        health = maxHealth;
    }

    private void Update()
    {
        if(healthSlider.value != health) healthSlider.value = health;

        if (Input.GetKeyDown(KeyCode.Z)) TakeDamage(100);

        if(healthSlider.value != easeHealthSlider.value) easeHealthSlider.value = Mathf.Lerp(easeHealthSlider.value, health, lerpSpeed);
    }

    void TakeDamage(float _damage)
    {
        health -= _damage;
    }
}
