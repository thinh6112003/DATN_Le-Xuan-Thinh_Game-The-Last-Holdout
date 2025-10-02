using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayUI : Singleton<GamePlayUI>
{
    public TextMeshProUGUI textHeart; 
    public TextMeshProUGUI textWaveCount;
    public TextMeshProUGUI textCoin;
    public TextMeshProUGUI textLevel;
    public int totalWaveCount;
    public GamePlayData gamePlayData;
    public Button pauseBtn;
    
    // Bi?n ð?m enemy cho win condition
    public int countEnemy = 0;
    
    public void Start()
    {
        pauseBtn.onClick.AddListener(()=>{
            GameStateManager.Instance.ChangeState(GameState.Pause);
        });
    }
    
    public void InitNewGame()
    {
        gamePlayData = DataManager.Instance.gamePlayData;
        textLevel.text = "Level " + (gamePlayData.level+1).ToString();
        totalWaveCount = gamePlayData.waveCount;
        gamePlayData.waveCount = 1;
        UpdateUIInGame();
    }
    
    public void UpdateUIInGame()
    {
        if(gamePlayData.heart >= 0) textHeart.text = gamePlayData.heart.ToString();
        textCoin.text = gamePlayData.coin.ToString();
        textWaveCount.text = "Wave "+  gamePlayData.waveCount.ToString() + "/" + totalWaveCount;
    }
    
    // Kh?i t?o ð?m enemy t? level data
    public void InitEnemyCount(LevelData levelData)
    {
        countEnemy = 0;
        // Ð?m t?ng s? enemy trong t?t c? wave
        foreach (WaveData waveData in levelData.listWaveDatas)
        {
            countEnemy += waveData.listEnemySpawnDatas.Count;
        }
        Debug.Log($"Total enemies in level: {countEnemy}");
    }
    
    // Ðý?c g?i khi enemy b? tiêu di?t
    public void EnemyDestroyed()
    {
        countEnemy--;
        Debug.Log($"Enemy destroyed. Remaining: {countEnemy}");
        CheckWinCondition();
    }
    
    // Ki?m tra ði?u ki?n th?ng
    private void CheckWinCondition()
    {
        if (countEnemy <= 0 && gamePlayData.heart > 0)
        {
            Debug.Log("Win condition met!");
            GameStateManager.Instance.ChangeStateAfter(1.5f,GameState.Win);
        }
    }
}
