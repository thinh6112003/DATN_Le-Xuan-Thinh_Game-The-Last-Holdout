using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Transform target;
    public float speed; 
    void Start()
    {
        
    }
    public void SetTarget(Transform target)
    {
        this.target = target;
    }
    // Update is called once per frame
    void Update()
    {
        Vector3 newpos = Vector3.MoveTowards(transform.position, target.position, Time.deltaTime * speed);
        transform.position = newpos;
        transform.LookAt(target.position);
    }
}
