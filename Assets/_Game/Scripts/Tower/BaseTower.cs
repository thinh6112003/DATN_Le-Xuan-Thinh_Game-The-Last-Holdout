using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BaseTower : MonoBehaviour
{
    public Bullet bullet;
    public Transform enemy;
    public Transform gunTransform;
    public int health;
    public int bulletDamage;
    public float bulletPerSecond;
    public float attackRadius;
    public Transform attackRadiusDisplay;
    public HashSet<BaseEnemy> allEnemyInRange= new HashSet<BaseEnemy>();
    public BuidingSlot myBuildingSlot;
    public int currentLevel = 1;
    public List<GameObject> modelActive = new List<GameObject>();
    public List<GameObject> modelCons = new List<GameObject>();

    public virtual void Start()
    { 
        StartCoroutine(EnemyDetecting());
    }
    public virtual IEnumerator Fire()
    {
        while (enemy != null)
        {
            if (enemy == null) break;
            Bullet newBullet =  Instantiate(bullet);
            newBullet.damage = bulletDamage;
            newBullet.myTower = this;
            newBullet.transform.position = gunTransform.position;
            newBullet.SetTarget(enemy);
            yield return new WaitForSeconds(1f/ bulletPerSecond);
        }
        StartCoroutine(EnemyDetecting());
        yield return null;
    }
    public virtual IEnumerator EnemyDetecting()
    {
        while (enemy == null)
        {
            if(allEnemyInRange.Count > 0)
            {
                BaseEnemy enemyNearest = allEnemyInRange.First();
                if (enemyNearest.dead)
                {
                    allEnemyInRange.Remove(enemyNearest);
                    Debug.LogWarning("bug bug bug bug");
                    continue;
                }
                float minDistance = Vector3.Distance(enemyNearest.transform.position, transform.position);
                foreach (BaseEnemy enemyTmp in allEnemyInRange)
                {
                    if(enemyTmp != null && enemyNearest != null && enemyTmp != enemyNearest)
                    {
                        float distanse = Vector3.Distance(transform.position, enemyTmp.transform.position);
                        if (distanse< minDistance)
                        {
                            enemyNearest = enemyTmp;
                            minDistance = distanse;
                        }
                    }
                }
                enemy = enemyNearest?.transform;
            }
            yield return null;
        }
        StartCoroutine(Fire());
        yield return null;
    }
    public virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            BaseEnemy newEnemy = other.gameObject.GetComponent<BaseEnemy>();
            if (!allEnemyInRange.Contains(newEnemy))
            {
                newEnemy.allTowerIn.Add(this);
                allEnemyInRange.Add(newEnemy);
            }
        }
    }
    public virtual void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            BaseEnemy outEnemy = other.gameObject.GetComponent<BaseEnemy>();
            allEnemyInRange.Remove(outEnemy);
            outEnemy.RemoveTowerIn(this);
            if (enemy == outEnemy.transform)
            {
                enemy = null;
            }
        }
    }
    public virtual void HandleEnemyDead(BaseEnemy _enemy)
    {
        allEnemyInRange.Remove(_enemy);
        if (enemy == _enemy.transform)
        {
            enemy = null;
        }
    }
    public virtual void UpdateLevel()
    {
        modelActive[currentLevel-1].SetActive(false);
        currentLevel++;
        modelActive[currentLevel-1].SetActive(true);
    }
}