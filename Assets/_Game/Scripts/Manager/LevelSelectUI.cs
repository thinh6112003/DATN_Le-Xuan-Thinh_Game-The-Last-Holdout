using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectUI : MonoBehaviour
{
    [SerializeField] private List<ItemLevel> itemLevels = new List<ItemLevel>(5);
    public Color deActiveColor;
    public Color activeColor;
    List<int> levelStars = new List<int>(5);
    int currentLevel;
    public void Init()
    {
        levelStars = DataManager.Instance.userData.levelStars;
        currentLevel = DataManager.Instance.userData.currentLevel;
        for (int i = 0; i < itemLevels.Count; i++) 
        {
            SetActiveLevel(i, i <= currentLevel, levelStars[i]);
        }
    }
    public void SetActiveLevel(int index, bool status, int startmax)
    {
        ItemLevel item = itemLevels[index];
        item.button.interactable = status;
        item.status = status;
        item.levelText.color = status? activeColor : deActiveColor;
        if(status == true)
        {
            for(int i = 0; i < item.stars.Count; i++)
            {
                item.stars[i].SetActive(!(i < startmax));
            }
            int indexLevel = index;
            item.button.onClick.RemoveAllListeners();
            item.button.onClick.AddListener(() => GameManager.Instance.PlayGame(indexLevel));
        }
    }
    public void GetUserData()
    {

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
[Serializable]
public class ItemLevel
{
    public List<GameObject> stars = new List<GameObject>(3);
    public Button button;
    public int index = 0;
    public bool status= false;
    public Text levelText;
}