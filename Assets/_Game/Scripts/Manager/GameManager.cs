using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public List<LevelController> listLevel;
    LevelController newLevel;
    int currentLevel = 0;
    
    private void Start()
    {
        // Khởi động trạng thái đầu tiên
        GameStateManager.Instance.ChangeState(GameState.Loading);
        //PlayGame();
    }
    public void PlayGame(int index)
    {
        Debug.Log("index lv gm "+ index);
        newLevel =  Instantiate(listLevel[index]);
        if (newLevel == null)
            Debug.Log("new level null");
        if(newLevel.waveSpawner.levelData == null ) 
            Debug.Log("level data null");
        if(newLevel.waveSpawner == null ) 
            Debug.Log("wave spawner null");
        newLevel.waveSpawner.levelData = Resources.Load<LevelData>("Level "+ (index+1).ToString()); 
        
        GamePlayData gamePlayData = new GamePlayData();
        newLevel.waveSpawner.levelData.SetDataGamePlay(ref gamePlayData);
        gamePlayData.level = index;
        DataManager.Instance.InitNewGame(gamePlayData);
        newLevel.gamePlayUI.InitNewGame();
        newLevel.waveSpawner.InitGame();
        GameStateManager.Instance.ChangeState(GameState.Gameplay);
        Time.timeScale = 1f;
        currentLevel = index;
    }
    public void RestartGame()
    {
        Destroy(newLevel.gameObject);
        PlayGame(currentLevel);
        GameStateManager.Instance.ChangeState(GameState.Gameplay);
    }
    public void RemoveCurrentLevel() { 
        if(newLevel != null) Destroy(newLevel.gameObject);
        currentLevel = 0;
        newLevel = null;
    }
    public void PlayGame()
    {
        DataManager.Instance.InitNewGame(new GamePlayData());
        GamePlayUI.Instance.InitNewGame();
    }
}
