using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements.Experimental;

public class SettingUI : MonoBehaviour
{
    public GameObject mussicOnImg;
    public GameObject musicOffImg;
    public GameObject soundOnImg;
    public GameObject soundOffImg;

    public UserData userData;

    public Button musicBtn;
    public Button soundBtn;
    public Slider volumeSlider;
    const float maxVolume = 1f;
    const float minVolume = 0f;

    private void Start()
    {
        userData = DataManager.Instance.userData;
        musicOffImg.SetActive(!userData.musicStatus);
        mussicOnImg.SetActive(userData.musicStatus);
        soundOffImg.SetActive(!userData.sfxStatus);
        soundOnImg.SetActive(userData.sfxStatus);
        volumeSlider.value = userData.volume;
        volumeSlider.onValueChanged.AddListener(ChangeVolume);
        musicBtn.onClick.AddListener(ToggleMussic);
        soundBtn.onClick.AddListener(ToggleSound);
    }
    public void ToggleMussic()
    {
        userData.musicStatus = !userData.musicStatus;
        musicOffImg.SetActive(!userData.musicStatus);
        mussicOnImg.SetActive(userData.musicStatus);
    }
    public void ToggleSound()
    {
        userData.sfxStatus = !userData.sfxStatus;
        soundOffImg.SetActive(!userData.sfxStatus);
        soundOnImg.SetActive(userData.sfxStatus);
    }
    public void ChangeVolume(float value)
    {
        userData.volume = Mathf.Clamp(value, minVolume, maxVolume);
    }
}
