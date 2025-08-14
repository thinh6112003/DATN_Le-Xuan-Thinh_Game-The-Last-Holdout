using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTower : MonoBehaviour
{
    public Bullet bullet;
    public Transform enemy;
    public Transform gunTransform;
    void Start()
    {
        StartCoroutine(Fire());
    }
    public IEnumerator Fire()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.25f);
            Bullet newBullet =  Instantiate(bullet);
            newBullet.transform.position = gunTransform.position;
            newBullet.SetTarget(enemy.transform);
        }
        yield return null;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
