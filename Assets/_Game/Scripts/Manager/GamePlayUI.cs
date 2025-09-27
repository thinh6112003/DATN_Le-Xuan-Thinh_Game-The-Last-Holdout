using TMPro;
using UnityEngine;

public class GamePlayUI : Singleton<GamePlayUI>
{
    public TextMeshProUGUI textHeart;
    public TextMeshProUGUI textWaveCount;
    public TextMeshProUGUI textCoin;
    public int totalWaveCount;
    public GamePlayData gamePlayData;

    public void InitNewGame()
    {
        gamePlayData = DataManager.Instance.gamePlayData;
        totalWaveCount = gamePlayData.waveCount;
        gamePlayData.waveCount = 1;
        UpdateUIInGame();
    }
    public void UpdateUIInGame()
    {
        textHeart.text = gamePlayData.heart.ToString();
        textCoin.text = gamePlayData.coin.ToString();
        textWaveCount.text = "Wave "+  gamePlayData.waveCount.ToString() + "/" + totalWaveCount;
    }
}
