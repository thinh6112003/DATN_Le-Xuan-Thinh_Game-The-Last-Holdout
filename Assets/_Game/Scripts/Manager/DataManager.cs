using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    public UserData userData;
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
}
