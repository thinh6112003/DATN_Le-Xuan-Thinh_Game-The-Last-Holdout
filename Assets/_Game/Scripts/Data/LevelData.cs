using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "LevelData", menuName = "level",order = 1)]
public class LevelData : ScriptableObject
{
    public List<WaveData> listWaveDatas;
}
