using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthEnemy : MonoBehaviour
{
    public BaseEnemy myBaseEnemy;
    public Slider healthSlider;
    public float maxhealth;
    public float currentHealth { get => myBaseEnemy.health; set => myBaseEnemy.health = (int)value; }

    public void Start()
    {
        Init();
    }
    public void Init()
    {
        maxhealth= currentHealth;
        healthSlider.value = 1;
    }
    public void UpdateHealthSlider()
    {
        healthSlider.value= currentHealth/maxhealth;
    }
    [Button]
    public void IncHealth(int healthNumber)
    {
        currentHealth += healthNumber;
        if(currentHealth > maxhealth) currentHealth = maxhealth;
        UpdateHealthSlider();
    }
    [Button]
    public void DecHealth(int healthNumber)
    {
        currentHealth -= healthNumber;
        if (currentHealth < 0)
        {
            currentHealth = 0;
            UpdateHealthSlider();
            myBaseEnemy.HandleDead();
            return;
        }
        UpdateHealthSlider();
    }
}
