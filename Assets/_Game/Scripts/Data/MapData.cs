using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapData : MonoBehaviour
{
    public List<PathData> listPath;
    public List<Transform> GetPath(List<int> pathIDList, int lineID)
    {
        List<Transform> path = new List<Transform>();
        path.Add(listPath[pathIDList[0]].lines[lineID].listWaypoint[0]);
        for(int i = 0;i < pathIDList.Count;i++)
        {
            for(int j = 1; j< listPath[pathIDList[i]].lines[lineID].listWaypoint.Count; j++)
            {
                path.Add(listPath[pathIDList[i]].lines[lineID].listWaypoint[j]);
            }
        }
        return path;
    }
}
[Serializable]
public class PathData { 
    public List<LineData> lines;
}
[Serializable]
public class LineData {
    public List<Transform> listWaypoint;
}
