using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UIManager : Singleton<UIManager>
{
    public GameObject loadingScreen;
    public Slider loadingProgressBar;
    public GameObject storyScreen;
    public List<GameObject> storyImages;
    public Button skipStoryButton;
    public GameObject homeScreen;
    public GameObject settingPopup;
    public GameObject levelSelectScreen;
    public GameObject pauseScreen;
    public GameObject winScreen;
    public GameObject loseScreen;
    public GameObject victoryScreen;
    public GameObject confirmQuitPopup; // Popup xác nhận quit

    public LevelSelectUI levelSelectUI;

    public Button playBtn;
    public Button settingBtn;
    public Button closeSettingBtn;
    public Button quitBtn;
    public Button restartBtn;
    public Button resumeBtn;
    public Button homeBtn; // Button Back từ Pause, Win, Lose về Home

    public Button replayLoseBtn;
    public Button backLoseBtn;
    public Button replayWinBtn;
    public Button nextWinBtn;
    public Button goHomeVictoryBtn;

    public Button closePauseBtn;
    public Button backFromLevelSelectBtn; // Button Back từ Level Select về Home
    public Button confirmQuitBtn; // Button Confirm trong popup quit
    public Button cancelQuitBtn; // Button Cancel trong popup quit

    private int currentStoryIndex = 0;

    private void Start()
    {
        playBtn.onClick.AddListener(() => GameStateManager.Instance.ChangeState(GameState.LevelSelect));
        settingBtn.onClick.AddListener(() => settingPopup.SetActive(true));
        closeSettingBtn.onClick.AddListener(() => settingPopup.SetActive(false));
        quitBtn.onClick.AddListener(() => ShowConfirmQuitPopup());
        backFromLevelSelectBtn.onClick.AddListener(() => GameStateManager.Instance.ChangeState(GameState.Home));

        restartBtn.onClick.AddListener(() => GameManager.Instance.RestartGame());
        resumeBtn.onClick.AddListener(() => GameStateManager.Instance.ChangeState(GameState.Gameplay));
        homeBtn.onClick.AddListener(() => GameStateManager.Instance.ChangeState(GameState.Home));
        closePauseBtn.onClick.AddListener(() => GameStateManager.Instance.ChangeState(GameState.Gameplay));
        replayLoseBtn.onClick.AddListener(() => GameManager.Instance.RestartGame());
        backLoseBtn.onClick.AddListener(() => GameStateManager.Instance.ChangeState(GameState.LevelSelect));
        replayWinBtn.onClick.AddListener(() => GameManager.Instance.RestartGame());
        nextWinBtn.onClick.AddListener(() => GameStateManager.Instance.ChangeState(GameState.LevelSelect));
        goHomeVictoryBtn.onClick.AddListener(() => GameStateManager.Instance.ChangeState(GameState.Home));

        // Setup confirm quit popup buttons
        confirmQuitBtn.onClick.AddListener(() => ConfirmQuit());
        cancelQuitBtn.onClick.AddListener(() => CancelQuit());
        levelSelectUI.Init();
    }

    private void ShowConfirmQuitPopup()
    {
        confirmQuitPopup.SetActive(true);
    }

    private void ConfirmQuit()
    {
        confirmQuitPopup.SetActive(false);
        GameStateManager.Instance.ChangeState(GameState.Quit);
    }

    private void CancelQuit()
    {
        confirmQuitPopup.SetActive(false);
    }

    public void OnGameStateChanged(GameState state)
    {
        if(!IsPopUpState(state)) HideAllScreens();
        switch (state)
        {
            case GameState.Loading:
                loadingScreen.SetActive(true);
                StartCoroutine(ShowLoadingProgress());
                break;
            case GameState.Story:
                storyScreen.SetActive(true);
                ShowStoryImage(0);
                break;
            case GameState.Home:
                homeScreen.SetActive(true);
                break;
            case GameState.LevelSelect:
                levelSelectScreen.SetActive(true);
                break;
            case GameState.Gameplay:
                // UI gameplay sẽ được kích hoạt sau khi load level
                break;
            case GameState.Pause:
                pauseScreen.SetActive(true);
                break;
            case GameState.Win:
                levelSelectUI.Init();
                winScreen.SetActive(true);
                break;
            case GameState.Lose:
                loseScreen.SetActive(true);
                break;
            case GameState.Victory:
                levelSelectUI.Init();
                victoryScreen.SetActive(true);
                break;
            case GameState.Quit:
                QuitApplication();
                break;
        }
    }

    public bool IsPopUpState(GameState state)
    {
        switch(state)
        {
            case GameState.Pause:
            case GameState.Win:
            case GameState.Lose:
                return true;
            default:
                return false;
        }
    }

    private void QuitApplication()
    {
        // Lưu dữ liệu trước khi thoát
        DataManager.Instance.SaveUserData(DataManager.Instance.userData);

#if UNITY_EDITOR
        // Trong Unity Editor: Stop play mode
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // Trên thiết bị thực: Thoát ứng dụng
        Application.Quit();
#endif
    }

    private void HideAllScreens()
    {
        loadingScreen.SetActive(false);
        storyScreen.SetActive(false);
        homeScreen.SetActive(false);
        settingPopup.SetActive(false);
        levelSelectScreen.SetActive(false);
        pauseScreen.SetActive(false);
        winScreen.SetActive(false);
        loseScreen.SetActive(false);
        victoryScreen.SetActive(false);
        confirmQuitPopup.SetActive(false); // Ẩn popup quit khi chuyển state
    }

    private IEnumerator ShowLoadingProgress()
    {
        float progress = 0f;
        while (progress < 1f)
        {
            progress += Time.deltaTime * 0.5f;
            loadingProgressBar.value = progress;
            yield return null;
        }
        GameStateManager.Instance.ChangeState(GameState.Story);
    }

    public void ShowStoryImage(int index)
    {
        for (int i = 0; i < storyImages.Count; i++)
            storyImages[i].SetActive(i == index);
        currentStoryIndex = index;
    }

    public void NextStoryImage()
    {
        if (currentStoryIndex < storyImages.Count - 1)
            ShowStoryImage(currentStoryIndex + 1);
        else
            GameStateManager.Instance.ChangeState(GameState.Home);
    }

    public void SkipStory()
    {
        GameStateManager.Instance.ChangeState(GameState.Home);
    }

    // Public methods để có thể gọi từ Button OnClick trong Inspector
    public void BackToHome()
    {
        GameStateManager.Instance.ChangeState(GameState.Home);
    }

    public void GoToLevelSelect()
    {
        GameStateManager.Instance.ChangeState(GameState.LevelSelect);
    }

    public void QuitGame()
    {
        ShowConfirmQuitPopup();
    }

    public void ConfirmQuitGame()
    {
        ConfirmQuit();
    }

    public void CancelQuitGame()
    {
        CancelQuit();
    }
}