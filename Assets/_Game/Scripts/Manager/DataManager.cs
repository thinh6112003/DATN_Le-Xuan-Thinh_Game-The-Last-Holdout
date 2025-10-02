using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    public UserData userData;
    public GamePlayData gamePlayData;
    public void Awake()
    {
        userData = LoadUserData();
    }
    private const string USER_DATA_KEY = "UserData";

    public void SaveUserData(UserData data)
    {
        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(USER_DATA_KEY, json);
        PlayerPrefs.Save();
    }

    public UserData LoadUserData()
    {
        if (PlayerPrefs.HasKey(USER_DATA_KEY))
        {
            string json = PlayerPrefs.GetString(USER_DATA_KEY);
            return JsonUtility.FromJson<UserData>(json);
        }
        return new UserData(); // Trả về mặc định nếu chưa có dữ liệu
    }

    public void ClearUserData()
    {
        PlayerPrefs.DeleteKey(USER_DATA_KEY);
    }
    private void OnApplicationQuit()
    {
        SaveUserData(userData); // Lưu dữ liệu khi ứng dụng thoát
    }
    public void InitNewGame(GamePlayData newGamePlayData)
    {
        gamePlayData = newGamePlayData;

    }

    // Thêm các phương thức cập nhật dữ liệu người chơi
    public void SetLevelStar(int level, int star)
    {
        if (userData.levelStars[level]< star)
        {
            userData.levelStars[level] = star;
        }
    }

    public void SetCurrentLevel(int level)
    {
        userData.currentLevel = level;
        SaveUserData(userData);
    }

    public void SetMusicStatus(bool status)
    {
        userData.musicStatus = status;
        SaveUserData(userData);
    }

    public void SetSFXStatus(bool status)
    {
        userData.sfxStatus = status;
        SaveUserData(userData);
    }

    public void SetCurrentVolume(float volume)
    {
        userData.currentVolume = volume;
        SaveUserData(userData);
    }

    public void SetFirstTimePlay(bool isFirst)
    {
        userData.firstTimePlay = isFirst;
        SaveUserData(userData);
    }
    public void CompleteLevel()
    {
        if (userData.currentLevel >= userData.levelStars.Count - 1)
            return; // Đã hoàn thành tất cả các level
        userData.currentLevel++;
        SaveUserData(userData);
    }
    public void SetStar()
    {
        int idLevelCurrent = gamePlayData.level;
        int currentHeart = gamePlayData.heart;
        int startHeart = gamePlayData.startHeart;
        if(currentHeart == startHeart)
        {
            SetLevelStar(idLevelCurrent, 3);
            return;
        }
        if (currentHeart> startHeart / 2f)
        {
            SetLevelStar(idLevelCurrent, 2);
        }
        else
        {
            SetLevelStar(idLevelCurrent, 1);
        }

    }
}
