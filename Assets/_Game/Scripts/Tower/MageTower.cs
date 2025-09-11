using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageTower : BaseTower
{
    public Transform archer;
    public Animator animator;
    public override void Start()
    {
        base.Start();
    }
    public void Update()
    {
        if(enemy != null)
        {
            archer.LookAt(enemy.position);
            animator.SetBool("Fire", true);
        }
        else
        {
            animator.SetBool("Fire", false);
        }
    }
}
