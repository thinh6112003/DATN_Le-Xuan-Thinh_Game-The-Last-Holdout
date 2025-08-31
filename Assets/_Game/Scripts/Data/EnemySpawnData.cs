using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EnemySpawnData
{
    public float timeInWave;
    public EnemyType enemyType;
    public List<int> listPathID = new List<int>();
    public int laneID;
}
