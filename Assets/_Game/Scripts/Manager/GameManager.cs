using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private void Start()
    {
        InitGame();   
    }
    public void InitGame()
    {
        DataManager.Instance.InitNewGame(new GamePlayData());
        UIManager.Instance.InitNewGame();
        //int idlevel = DataManager.Instance.userData.currentLevel;
        //string fileName = "level" + idlevel; // không có phần mở rộng
        //TextAsset jsonTextAsset = Resources.Load<TextAsset>(fileName);
        //LevelData levelData = null; // Khởi tạo biến levelData
        //if (jsonTextAsset != null)
        //{
        //    levelData = JsonUtility.FromJson<LevelData>(jsonTextAsset.text);
        //    Debug.Log("Level loaded: " + fileName);
        //}
        //else
        //{
        //    Debug.LogError("Không tìm thấy file: " + fileName + " trong Resources");
        //}
        //GridManager.Instance.LoadGrid(levelData.cols, levelData.rows, levelData.gridData);
        //SlotManager.Instance.LoadSlots();
    }
}
