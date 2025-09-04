using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtilleryBullet : Bullet
{
    public Transform target;
    public int damage=5;
    public float speed; 
    public Coroutine coroutine;
    public BaseTower myTower; 
    void Start()
    {
        
    }
    Vector3 newTargetPos;
    public override void Update()
    {
        if (target.gameObject.activeInHierarchy) newTargetPos = target.position;
        else
        {
            newTargetPos = target.position + Vector3.down * 0.75f ;
        }
        if (!target.gameObject.activeInHierarchy && coroutine == null && IsToTarget())
        {
            coroutine = StartCoroutine(RemoveMissingBullet());
        } 
        Vector3 bulletPos = Vector3.MoveTowards(transform.position, newTargetPos, Time.deltaTime * speed);
        transform.position = bulletPos;
        transform.LookAt(newTargetPos);
    }
}
