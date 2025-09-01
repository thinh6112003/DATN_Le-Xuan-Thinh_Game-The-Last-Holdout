using DG.Tweening;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    public GameObject winGameUI;
    public GameObject loseGameUI;
    public GameObject pauseGameUI;
    public GameObject gamePlayUI;

    public TextMeshProUGUI textHeart;
    public TextMeshProUGUI textWaveCount;
    public TextMeshProUGUI textCoin;

    public int totalWaveCount;
    public GamePlayData gamePlayData;

    private void Start()
    {
    }
    public async void ShowWin()
    {
        winGameUI.gameObject.SetActive(true);
    }
    public async void ShowLose()
    {
        loseGameUI.gameObject.SetActive(true); 
    }
    public async void HideLose()
    {
        loseGameUI.gameObject.SetActive(false);
    }
    public async void HideWin()
    {
        winGameUI.gameObject.SetActive(false);
    }
    public async void ShowPauseGameUI()
    {
        pauseGameUI.gameObject.SetActive(true);
    }
    public async void HidePauseGameUI()
    {
        pauseGameUI.gameObject.SetActive(false);
    }
    public async void ShowGamePlayUI()
    {
        gamePlayUI.gameObject.SetActive(true);
    }
    public async void HideGamePlayUI()
    {
        gamePlayUI.gameObject.SetActive(false);
    }
    
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
