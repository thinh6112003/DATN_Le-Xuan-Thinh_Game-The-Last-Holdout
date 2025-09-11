using AssetKits.ParticleImage;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    public int health;
    public float speed; 
    public HealthEnemy healthEnemy;
    public WaypointMover waypointMover;
    public HashSet<BaseTower> allTowerIn= new HashSet<BaseTower>();
    public bool dead = false;
    public int coinReward;
    public Animator myanimator;
    private void Awake()
    {
        waypointMover.moveSpeed = speed;
        healthEnemy.myBaseEnemy = this;
        EventObserver.AddListener("Lose", LoseHandle);
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
        foreach (BaseTower tower in allTowerIn)
        {
            tower.HandleEnemyDead(this);
        }
        dead = true;
        gameObject.SetActive(false);

    }
    public void RemoveTowerIn(BaseTower tower)
    {
        allTowerIn.Remove(tower);
    }
    public void TakeDamage(int damage)
    {
        healthEnemy.DecHealth(damage);
    }
    public void LoseHandle(object[] args)
    {
        Destroy(waypointMover);
        Destroy(myanimator);
    }
}
public enum EnemyType
{
    tank,
    runner
}
