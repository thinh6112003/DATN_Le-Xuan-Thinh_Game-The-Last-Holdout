using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchorTower : BaseTower
{
    public Transform archer;
    public override void Start()
    {
        base.Start();
    }
    public void Update()
    {
        if(enemy != null)
        {
            archer.LookAt(enemy.position);
        }
    }
}
