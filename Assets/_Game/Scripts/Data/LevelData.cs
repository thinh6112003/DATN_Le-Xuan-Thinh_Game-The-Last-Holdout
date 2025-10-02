using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "LevelData", menuName = "level",order = 1)]
public class LevelData : ScriptableObject
{
    public List<WaveData> listWaveDatas;
    public int goldStart;
    public int heartStart;
    public string wordEnglishReward;

    public void SetDataGamePlay(ref GamePlayData gamePlayData)
    {
        gamePlayData.coin = goldStart;
        gamePlayData.heart = heartStart;
        gamePlayData.startHeart = heartStart;
        gamePlayData.waveCount = listWaveDatas.Count;
    }
}
