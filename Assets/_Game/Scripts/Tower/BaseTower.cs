using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BaseTower : MonoBehaviour
{
    public int health;
    public int bulletDamage;
    public float bulletPerSecond;
    public float attackRadius;
    public Bullet bullet;
    public Transform enemy;
    public Transform gunTransform;
    public Transform attackRadiusDisplay;
    public HashSet<BaseEnemy> allEnemyInRange= new HashSet<BaseEnemy>();
    public BuidingSlot myBuildingSlot;
    public int currentLevel = 1;
    public List<GameObject> modelActive = new List<GameObject>();
    public List<GameObject> modelCons = new List<GameObject>();
    public List<int> costUpgrade = new List<int>();
    public bool isTraiLinh = false;
    public Linh linh1;
    public Transform tf1;
    public bool attackFly = false;

    public virtual void Start()
    {
        if (isTraiLinh)
        {
            tf1.position = myBuildingSlot.tf1.position;
            linh1.transform.position = tf1.position;
        }
        else
            StartCoroutine(EnemyDetecting());
    }
    public virtual IEnumerator Fire()
    {
        while (enemy != null)
        {
            if (enemy == null || enemy.GetComponent<BaseEnemy>().dead)
            {
                yield return null;
                break;
            }
            if (enemy.GetComponent<BaseEnemy>().dead)
            {
                enemy = null;
                break;
            }
            Bullet newBullet =  Instantiate(bullet, transform);
            newBullet.damage = bulletDamage;
            newBullet.myTower = this;
            newBullet.transform.position = gunTransform.position;
            newBullet.SetTarget(enemy,gunTransform.position);
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
                    Debug.LogWarning("enemy dead here");
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
            if (newEnemy.isFly && !attackFly)
            {
                return;
            }
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
            if(outEnemy.isFly && !attackFly)
            {
                return;
            }
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
        Debug.Log("bulletDamage " +bulletDamage);
        bulletDamage = (150 * bulletDamage) / 100;
        Debug.Log("bulletDamage " + bulletDamage);
        if (isTraiLinh) { 
            linh1.damage = (150 * linh1.damage) / 100;
            linh1.maxHealth = (150 * linh1.maxHealth) / 100;
        }
        modelActive[currentLevel-1].SetActive(true);
    }
    public int GetPriceUpdate()
    {
        if (currentLevel - 1 < costUpgrade.Count)
            return costUpgrade[currentLevel - 1];
        return -1;
    }
}