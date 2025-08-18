using DG.Tweening;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    public GameObject winGameUI;
    public GameObject loseGameUI;
    public GameObject pauseGameUI;
    public GameObject gamePlayUI;
    public Image dim;

    [Header("WinGameUI Element")]
    public CanvasGroup nextLevelButton;
    public GameObject effectwin;
    public Transform wintext;
    public CanvasGroup tranh;
    public Image haoQuangImg;
    public Image fadewin;
    public Transform TranhTransform;
    GameObject cloneTranh = null;
    [Header("LoseGameUI Element")]
    public CanvasGroup replayButton;
    public Transform losetext;
    public Image fadelose;
    [Header("SettingPopup Element")]
    public Transform popupPause;
    public Image fadepause;
    public GameObject offbtnSoundIMG;
    public GameObject offbtnMusicIMG;
    public GameObject offbtnrungIMG;
    public GameObject onbtnSoungImg;
    public GameObject onbtnMusicImg;
    public GameObject onbtnRungImg;

    public UserData userData;
    private void Start()
    {
        userData = DataManager.Instance.userData;
        onbtnSoungImg.SetActive(userData.sfxStatus);
        offbtnSoundIMG.SetActive(!userData.sfxStatus);

        onbtnMusicImg.SetActive(userData.musicStatus);
        offbtnMusicIMG.SetActive(!userData.musicStatus);

        onbtnRungImg.SetActive(userData.rungStatus);
        offbtnrungIMG.SetActive(!userData.rungStatus);
    }
    public async void ShowWin()
    {
        winGameUI.gameObject.SetActive(true);
        effectwin.SetActive(false);
        AudioManager.Instance.PlaySFX(SFXType.Win);
        AudioManager.Instance.StopMusic();
        if(cloneTranh != null)
        {
            Destroy(cloneTranh);
        }

        fadewin.color = new Color(0, 0, 0, 0);
        fadewin.DOFade(0.8f, 0.3f).SetEase(Ease.OutQuint);
        wintext.localScale = new Vector3(0.6f, 0.6f, 0.6f);
        wintext.DOScale(new Vector3(1, 1, 1), 0.8f).SetEase(Ease.OutBack,3f);

        haoQuangImg.color = new Color(1, 1, 1, 0);
        haoQuangImg.DOFade(1, 0.4f).SetEase(Ease.OutCubic).SetDelay(0.4f);

        nextLevelButton.alpha = 0;
        nextLevelButton.DOFade(1, 0.5f).SetEase(Ease.OutCubic).SetDelay(0.8f);
        await Task.Delay(200);
        effectwin.SetActive(true);
    }
    public async void ShowLose()
    {
        AudioManager.Instance.PlaySFX(SFXType.Lose);
        loseGameUI.gameObject.SetActive(true); 
        replayButton.transform.localScale = new Vector3(1, 1, 1);
        fadelose.color = new Color(0, 0, 0, 0);
        fadelose.DOFade(0.8f, 0.3f).SetEase(Ease.OutQuint);
        losetext.GetComponent<CanvasGroup>().alpha = 0 ;
        losetext.GetComponent<CanvasGroup>().DOFade(1, 0.8f).SetEase(Ease.OutCubic);
        losetext.localScale = new Vector3(2.5f, 5f, 0.6f);
        losetext.DOScale(new Vector3(1, 1, 1), 0.8f).SetEase(Ease.OutCubic);
        replayButton.alpha = 0;
        replayButton.DOFade(1, 0.5f).SetEase(Ease.OutCubic).SetDelay(0.8f);
        await Task.Delay(200);
    }
    public async void HideLose()
    {
        fadelose.DOFade(0, 0.3f).SetEase(Ease.OutCubic).SetDelay(0.2f);
        losetext.transform.DOScale(new Vector3(0.6f, 0.6f, 0.6f), 0.4f).SetEase(Ease.InBack, 2f);
        losetext.GetComponent<CanvasGroup>().DOFade(0, 0.4f).SetEase(Ease.OutSine);
        replayButton.DOFade(0, 0.4f).SetEase(Ease.OutSine);
        replayButton.transform.DOScale(new Vector3(0.6f, 0.6f, 0.6f), 0.4f).SetEase(Ease.InBack, 2f);
        await Task.Delay(400);

        loseGameUI.gameObject.SetActive(false);
    }
    public async void HideWin()
    {
        fadewin.DOFade(0, 0.3f).SetEase(Ease.OutCubic).SetDelay(0.2f);
        wintext.transform.DOScale(new Vector3(0.6f, 0.6f, 0.6f), 0.4f).SetEase(Ease.InBack, 2f);
        wintext.GetComponent<CanvasGroup>().DOFade(0, 0.4f).SetEase(Ease.OutSine);
        nextLevelButton.DOFade(0, 0.4f).SetEase(Ease.OutSine);
        tranh.DOFade(0,0.4f).SetEase(Ease.OutSine);
        nextLevelButton.transform.DOScale(new Vector3(0.6f, 0.6f, 0.6f), 0.4f).SetEase(Ease.InBack,2f);
        haoQuangImg.DOFade(0, 0.4f).SetEase(Ease.OutSine);
        await Task.Delay(400);
        wintext.GetComponent<CanvasGroup>().alpha = 1;
        nextLevelButton.transform.localScale = new Vector3(1, 1, 1);
        winGameUI.gameObject.SetActive(false);
    }
    public async void ShowPauseGameUI()
    {
        pauseGameUI.gameObject.SetActive(true);
        fadepause.color = new Color(0,0,0, 0);
        popupPause.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        popupPause.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
        popupPause.transform.DOScale(new Vector3(1, 1, 1), 0.4f).SetEase(Ease.OutBack,2.5f);
        fadepause.DOFade(0.8f, 0.3f).SetEase(Ease.OutQuint);
    }
    public async void HidePauseGameUI()
    {
        fadepause.DOFade(0, 0.3f).SetEase(Ease.InQuint).SetDelay(0.2f);
        popupPause.GetComponent<Image>().DOFade(0, 0.3f).SetEase(Ease.Linear).SetDelay(0.2f);
        await popupPause.transform.DOScale(new Vector3(0.6f, 0.6f, 0.6f), 0.4f).SetEase(Ease.InBack,2.5f).AsyncWaitForCompletion();
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

    public void OnclickSound()
    {
        userData.sfxStatus = !userData.sfxStatus;
        onbtnSoungImg.SetActive(userData.sfxStatus);
        offbtnSoundIMG.SetActive(!userData.sfxStatus);
    }
    public void OnclickMusic()
    {
        userData.musicStatus = !userData.musicStatus;
        onbtnMusicImg.SetActive(userData.musicStatus);
        offbtnMusicIMG.SetActive(!userData.musicStatus);

    }
    public void OnclickRung()
    {
        userData.rungStatus = !userData.rungStatus;
        onbtnRungImg.SetActive(userData.rungStatus);
        offbtnrungIMG.SetActive(!userData.rungStatus);
    }

}
