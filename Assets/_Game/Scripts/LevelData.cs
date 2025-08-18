using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class LevelData 
{
    public List<string> queueData = new List<string>();
    public List<bool> slotData = new List<bool>();
    public int rows;
    public int cols;
    public List<int> gridData = new List<int>();
}
