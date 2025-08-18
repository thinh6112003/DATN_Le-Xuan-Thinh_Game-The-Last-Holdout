using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTower : MonoBehaviour
{
    public Bullet bullet;
    public Transform enemy;
    public Transform gunTransform;
    public int Health; 
    void Start()
    {
        StartCoroutine(Fire());
    }
    public IEnumerator Fire()
    {
        while (enemy != null)
        {
            yield return new WaitForSeconds(0.25f);
            if (enemy == null) yield break;
            Bullet newBullet =  Instantiate(bullet);
            newBullet.transform.position = gunTransform.position;
            newBullet.SetTarget(enemy);
        }
        yield return null;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
