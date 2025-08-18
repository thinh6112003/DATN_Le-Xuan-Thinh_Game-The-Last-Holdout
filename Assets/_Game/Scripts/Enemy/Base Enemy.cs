using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    public int health;
    public HealthEnemy healthEnemy;

    private void Awake()
    {
        healthEnemy.myBaseEnemy = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            Bullet bullet = other.GetComponent<Bullet>();
            healthEnemy.DecHealth(bullet.damage);
            Destroy(bullet.gameObject,0.05f);
        }
    }
    public void HandleDead()
    {
        Destroy(gameObject);
    }
}
