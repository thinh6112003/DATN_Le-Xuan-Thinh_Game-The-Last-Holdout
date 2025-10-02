using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthLinh : MonoBehaviour
{
    public Linh linh;
    public Slider healthSlider;
    float maxhealth { get => linh.maxHealth; }
    float currentHealth { get => linh.health; set => linh.health = (int)value; }

    public void Start()
    {
        Init();
    }
    public void Init()
    {
        healthSlider.value = 1;
    }
    public void UpdateHealthSlider()
    {
        healthSlider.value = currentHealth / maxhealth;
    }
    [Button]
    public void IncHealth(int healthNumber)
    {
        currentHealth += healthNumber;
        if (currentHealth > maxhealth) currentHealth = maxhealth;
        UpdateHealthSlider();
    }
}