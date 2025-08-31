using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WaveData
{
    public List<EnemySpawnData> listEnemySpawnDatas;
    public float timeForWave;
    public float timeForReady;
}
