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

    void Start()
    { 
        StartCoroutine(EnemyDetecting());
    }
    public IEnumerator Fire()
    {
        while (enemy != null)
        {
            yield return new WaitForSeconds(1f/ bulletPerSecond);
            if (enemy == null) break;
            Bullet newBullet =  Instantiate(bullet);
            newBullet.damage = bulletDamage;
            newBullet.myTower = this;
            newBullet.transform.position = gunTransform.position;
            newBullet.SetTarget(enemy);
        }
        StartCoroutine(EnemyDetecting());
        yield return null;
    }
    public IEnumerator EnemyDetecting()
    {
        while (enemy == null)
        {
            if(allEnemyInRange.Count > 0)
            {
                BaseEnemy enemyNearest = allEnemyInRange.First();
                float minDistance = Vector3.Distance(enemyNearest.transform.position, transform.position);
                foreach (BaseEnemy enemyTmp in allEnemyInRange)
                {
                    if(enemyTmp != enemyNearest)
                    {
                        float distanse = Vector3.Distance(transform.position, enemyTmp.transform.position);
                        if (distanse< minDistance)
                        {
                            enemyNearest = enemyTmp;
                            minDistance = distanse;
                        }
                    }
                }
                enemy = enemyNearest.transform;
            }
            yield return null;
        }
        StartCoroutine(Fire());
        yield return null;
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            BaseEnemy newEnemy = other.gameObject.GetComponent<BaseEnemy>();
            if (!allEnemyInRange.Contains(newEnemy))
            {
                Debug.Log("ua alo 1=======");
                newEnemy.allTowerIn.Add(this);
                allEnemyInRange.Add(newEnemy);
            }
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("enemy out ");
            BaseEnemy outEnemy = other.gameObject.GetComponent<BaseEnemy>();
            allEnemyInRange.Remove(outEnemy);
            outEnemy.RemoveTowerIn(this);
            if (enemy == outEnemy.transform)
            {
                enemy = null;
                Debug.Log("ua alo 2+++++++++++++++++");
            }
        }
    }
    public void HandleEnemyDead(BaseEnemy _enemy)
    {
        allEnemyInRange.Remove(_enemy);
        if (enemy == _enemy.transform)
        {
            enemy = null;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
