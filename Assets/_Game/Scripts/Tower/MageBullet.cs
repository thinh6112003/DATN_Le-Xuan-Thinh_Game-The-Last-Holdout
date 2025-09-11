using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MageBullet : Bullet
{
    public override IEnumerator RemoveMissingBullet()
    {
        yield return new WaitForSeconds(0.2f);
        Destroy(gameObject);
        yield return null;
    }
}