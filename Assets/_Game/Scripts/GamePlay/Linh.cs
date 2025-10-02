using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using static UnityEngine.GraphicsBuffer;

public class Linh : MonoBehaviour
{
    public Animator anim;
    public Transform originTransform;
    public LinhState currentState;
    public float speed= 6;
    public float timeLiveAway;
    public int damage;
    public int health;
    public int maxHealth;
    public float timer;
    public GameObject model;
    public BaseEnemy enemyCurrent;
    public BaseTower BaseTower;
    public HealthLinh healthLinh;
    public Vector3 attackPos;

    private void Start()
    {
        maxHealth = health;
        currentState = LinhState.Idle;
        anim.SetBool("idle", true);
    }
    private void Update()
    {
        switch (currentState)
        {
            case LinhState.Idle:
                if(enemyCurrent == null )
                {
                    if (BaseTower.allEnemyInRange.Count > 0)
                    {
                        foreach(BaseEnemy enemy in BaseTower.allEnemyInRange)
                        {
                            if (!enemy.dead && !enemy.isFly)
                            {
                                enemyCurrent = enemy;
                                enemy.SetAttack(this);
                                anim.SetBool("idle", false);
                                anim.SetBool("attack", false);
                                anim.SetBool("run", true);
                                currentState = LinhState.ToTarget;
                                break;
                            }
                        }
                    }
                } 
                break;
            case LinhState.ToTarget:
                if(enemyCurrent == null || enemyCurrent.dead)
                {
                    currentState = LinhState.Idle;
                    anim.SetBool("idle", true);
                    anim.SetBool("run", false);
                    anim.SetBool("attack", false);
                    enemyCurrent = null;
                    break;
                }
                Debug.Log("to target");
                Vector3 pos = enemyCurrent.transform.position;
                pos.y = transform.position.y;
                transform.position = Vector3.MoveTowards(
                    transform.position,
                    pos,
                    speed * Time.deltaTime
                );
                float angleY = Mathf.Atan2(
                    (pos.x - transform.position.x),
                    (pos.z - transform.position.z)
                ) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, angleY, 0);
                if (Vector3.Distance(transform.position, enemyCurrent.transform.position) < 1.5f)
                {
                    currentState = LinhState.Attack;
                    anim.SetBool("idle", false);
                    anim.SetBool("run", false);
                    anim.SetBool("attack", true);
                    enemyCurrent.TakeDamage(damage);
                    timer = 0;
                    if (enemyCurrent.health <= 0)
                    {
                        currentState = LinhState.Idle;
                        anim.SetBool("idle", true);
                        anim.SetBool("run", false);
                        anim.SetBool("attack", false);
                    }
                }
                break;
            case LinhState.Attack:
                Debug.Log("attack ");
                timer += Time.deltaTime;
                if (timer >= 1.333f)
                {
                    timer = 0;
                    if (enemyCurrent != null && !enemyCurrent.dead)
                    {
                        enemyCurrent.TakeDamage(damage);
                    }
                    else
                    {
                        enemyCurrent = null;
                        currentState = LinhState.Idle;
                        anim.SetBool("attack", false);
                        anim.SetBool("run", false);
                        anim.SetBool("idle", true);
                    }
                }
                break;
            case LinhState.Die:
                timer += Time.deltaTime;
                if (timer >= timeLiveAway)
                {
                    timer = 0;
                    enemyCurrent = null; 
                    currentState = LinhState.Idle;
                    model.SetActive(true);
                    anim.SetBool("idle", true);
                    health = maxHealth;
                    healthLinh.gameObject.SetActive(true);
                    healthLinh.UpdateHealthSlider();
                }
                break;
            default:
                break;
        }
    }
    public void TakeDamage(int damage)
    {
        health -= damage;
        if(health < 0)
        {
            currentState = LinhState.Die;
            model.SetActive(false);
            healthLinh.gameObject.SetActive(false);
            anim.SetBool("die", false);
            timer = 0;
        }
        healthLinh.UpdateHealthSlider();
    }
}
public enum LinhState
{
    Idle, ToTarget, Attack, Die
}