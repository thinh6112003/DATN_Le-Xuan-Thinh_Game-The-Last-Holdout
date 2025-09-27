using UnityEngine;
using UnityEngine.UI; // Thư viện để làm việc với các thành phần UI như Button, Text
using System.Collections; // Thư viện cho Coroutines (nếu cần hiệu ứng chuyển cảnh)

/// <summary>
/// UIManager quản lý tất cả các màn hình và popup trong game.
/// Nó hoạt động như một Singleton để dễ dàng truy cập từ các script khác.
/// </summary>
public class UIManager : MonoBehaviour
{
    #region Singleton
    // Triển khai Singleton Pattern
    private static UIManager _instance;
    public static UIManager Instance
    {
        get
        {
            if (_instance == null)
            {
                // Tìm một đối tượng UIManager trong Scene
                _instance = FindObjectOfType<UIManager>();
                if (_instance == null)
                {
                    // Nếu không tìm thấy, tạo một GameObject mới và thêm component UIManager vào
                    GameObject singletonObject = new GameObject("UIManager");
                    _instance = singletonObject.AddComponent<UIManager>();
                }
            }
            return _instance;
        }
    }

    private void Awake()
    {
        // Đảm bảo chỉ có một instance duy nhất tồn tại
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
            // DontDestroyOnLoad(gameObject); // Bỏ comment dòng này nếu UIManager cần tồn tại qua các Scene
        }
    }
    #endregion

    #region UI References
    // Tham chiếu đến các GameObjects của các màn hình và popup
    [Header("Screens")]
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private GameObject storyScreen;
    [SerializeField] private GameObject homeScreen;
    [SerializeField] private GameObject levelSelectScreen;
    [SerializeField] private GameObject gameplayScreen;
    [SerializeField] private GameObject victoryScreen;

    [Header("Popups")]
    [SerializeField] private GameObject settingsPopup;
    [SerializeField] private GameObject pausePopup;
    [SerializeField] private GameObject loseLevelPopup; // Sử dụng Popup thay vì Screen để hiển thị trên GameplayScreen
    [SerializeField] private GameObject winLevelPopup;  // Tương tự

    [Header("Gameplay UI Elements")]
    [SerializeField] private Text goldText;
    [SerializeField] private Text livesText;
    [SerializeField] private Text waveText;

    #endregion

    private void Start()
    {
        // Khi game bắt đầu, ẩn tất cả các màn hình và chỉ hiển thị màn hình Loading
        HideAllScreens();
        ShowLoadingScreen();
        // Giả lập quá trình tải game, sau đó chuyển sang Story hoặc Home
        StartCoroutine(InitialLoad());
    }

    // Giả lập việc tải tài nguyên
    private IEnumerator InitialLoad()
    {
        yield return new WaitForSeconds(2f); // Chờ 2 giây
        HideLoadingScreen();

        // Kiểm tra xem đây có phải lần đầu người chơi vào game không (dựa trên PlayerPrefs)
        bool isFirstTime = PlayerPrefs.GetInt("FirstTimePlay", 1) == 1;

        if (isFirstTime)
        {
            ShowStoryScreen();
            PlayerPrefs.SetInt("FirstTimePlay", 0); // Đánh dấu không còn là lần đầu
            PlayerPrefs.Save();
        }
        else
        {
            ShowHomeScreen();
        }
    }

    #region Screen & Popup Control Methods

    /// <summary>
    /// Ẩn tất cả các màn hình và popup chính.
    /// </summary>
    public void HideAllScreens()
    {
        loadingScreen.SetActive(false);
        storyScreen.SetActive(false);
        homeScreen.SetActive(false);
        levelSelectScreen.SetActive(false);
        gameplayScreen.SetActive(false);
        victoryScreen.SetActive(false);

        settingsPopup.SetActive(false);
        pausePopup.SetActive(false);
        winLevelPopup.SetActive(false);
        loseLevelPopup.SetActive(false);
    }

    // Các phương thức để hiển thị từng màn hình cụ thể
    public void ShowLoadingScreen() => loadingScreen.SetActive(true);
    public void HideLoadingScreen() => loadingScreen.SetActive(false);

    public void ShowStoryScreen()
    {
        HideAllScreens();
        storyScreen.SetActive(true);
    }

    public void ShowHomeScreen()
    {
        HideAllScreens();
        homeScreen.SetActive(true);
    }

    public void ShowLevelSelectScreen()
    {
        HideAllScreens();
        levelSelectScreen.SetActive(true);
    }

    public void ShowGameplayScreen()
    {
        HideAllScreens();
        gameplayScreen.SetActive(true);
    }

    public void ShowVictoryScreen()
    {
        HideAllScreens();
        victoryScreen.SetActive(true);
    }

    // Các phương thức để quản lý popup
    public void ToggleSettingsPopup(bool show) => settingsPopup.SetActive(show);
    public void TogglePausePopup(bool show) => pausePopup.SetActive(show);
    public void ShowWinLevelPopup() => winLevelPopup.SetActive(true);
    public void ShowLoseLevelPopup() => loseLevelPopup.SetActive(true);

    #endregion

    #region UI Data Update Methods

    /// <summary>
    /// Cập nhật số vàng hiển thị trên giao diện Gameplay.
    /// </summary>
    /// <param name="amount">Số vàng hiện tại.</param>
    public void UpdateGold(int amount)
    {
        if (goldText != null)
        {
            goldText.text = amount.ToString();
        }
    }

    /// <summary>
    /// Cập nhật số mạng sống hiển thị trên giao diện Gameplay.
    /// </summary>
    /// <param name="count">Số mạng còn lại.</param>
    public void UpdateLives(int count)
    {
        if (livesText != null)
        {
            livesText.text = "Lives: " + count;
        }
    }

    /// <summary>
    /// Cập nhật thông tin về đợt tấn công của kẻ địch.
    /// </summary>
    /// <param name="currentWave">Đợt hiện tại.</param>
    /// <param name="totalWaves">Tổng số đợt.</param>
    public void UpdateWaveInfo(int currentWave, int totalWaves)
    {
        if (waveText != null)
        {
            waveText.text = $"Wave: {currentWave} / {totalWaves}";
        }
    }

    #endregion
}