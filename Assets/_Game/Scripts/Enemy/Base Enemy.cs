using AssetKits.ParticleImage;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using static UnityEngine.UI.Image;

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
    public bool hasChar= false;
    public FloatingText floatingText;
    public bool isFly = false;
    public Action attackAction;
    public Action HoiMauAction;
    public int damage = 4;
    public int hoimauValue = 0;

    public bool isPhuThuy = false;
    private void Awake()
    {
        waypointMover.moveSpeed = speed;
        healthEnemy.myBaseEnemy = this;
        EventObserver.AddListener("Lose", LoseHandle);
        if (isPhuThuy)
        {
            HoiMauAction = HoiMauHandle;
        }
    }

    // Update is called once per frame
    void Update()
    {
        attackAction?.Invoke();
        HoiMauAction?.Invoke();
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
        if (hasChar)
        {
            TextMeshProUGUI text = WaveSpawner.Instance.GetText();
            floatingText.transform.parent = null;
            floatingText.transform.position = transform.position;
            floatingText.gameObject.SetActive(true);
            floatingText.SetText(text, transform.position);
            WaveSpawner.Instance.ShowNewChar();
            Debug.Log("show show show show show");
        }
        foreach (BaseTower tower in allTowerIn)
        {
            tower.HandleEnemyDead(this);
        }
        dead = true;
        DataManager.Instance.gamePlayData.coin += coinReward;
        EventObserver.Notice("UpdateStatusOfButton");
        GamePlayUI.Instance.UpdateUIInGame();
        // Thông báo enemy ð? b? tiêu di?t
        GamePlayUI.Instance.EnemyDestroyed();
        
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

    public void SetAttack(Linh linh)
    {
        float angleY = Mathf.Atan2(
            linh.transform.position.x - transform.position.x,
            linh.transform.position.z - transform.position.z
        ) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, angleY, 0);
        linhC = linh;
        if (myanimator != null)
        {
            myanimator.SetBool("attack", true);
        }
        waypointMover.isAttack = true;
        attackAction = HandleAttack;
    }
    public void SetOutAttack()
    {
        waypointMover.isAttack = false;
        attackAction = null;    
        if (myanimator != null)
        {
            myanimator.SetBool("attack", false);
        }
        linhC = null;
    }
    Linh linhC;
    float timer = 0;
    float timeAttack = 0.7666f;
    public void HandleAttack()
    {
        
        timer += Time.deltaTime;
        if(timer >= timeAttack)
        {
            timer = 0;
            if (linhC != null && linhC.currentState != LinhState.Idle)
            {
                linhC.TakeDamage(damage);
                if(linhC.health <= 0)
                {
                    SetOutAttack();
                }
            }
        }
    }
    float timerHoiMau;
    public void HoiMauHandle()
    {
        timerHoiMau += Time.deltaTime;
        if(timerHoiMau >= 7f)
        {
            timerHoiMau = 0;
            myanimator.SetTrigger("Phep");
            
            Collider[] listCol =  Physics.OverlapSphere(transform.position, 2f);
            foreach(Collider col in listCol)
            {
                if(col.CompareTag("Enemy"))
                {
                    BaseEnemy enemy = col.GetComponent<BaseEnemy>();
                    if(enemy != null && !enemy.dead)
                    {
                        enemy.healthEnemy.IncHealth(hoimauValue);
                    }
                }
            }

            healthEnemy.IncHealth(30);
        }
    }
}
public enum EnemyType
{
    tank,
    runner
}
