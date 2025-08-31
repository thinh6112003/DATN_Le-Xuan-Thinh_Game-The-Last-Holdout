using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Transform target;
    public int damage=5;
    public float speed; 
    public Coroutine coroutine;
    public BaseTower myTower; 
    void Start()
    {
        
    }
    public void SetTarget(Transform target)
    {
        this.target = target;
        newTargetPos = target.position;
    }
    // Update is called once per frame
    Vector3 newTargetPos;
    void Update()
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
    IEnumerator RemoveMissingBullet()
    {
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
        yield return null;
    }
    bool IsToTarget()
    {
        return Mathf.Abs(transform.position.x - newTargetPos.x) < 0.05f
            && Mathf.Abs(transform.position.y - newTargetPos.y) < 0.05f
            && Mathf.Abs(transform.position.z - newTargetPos.z) < 0.05f;
    }
}
