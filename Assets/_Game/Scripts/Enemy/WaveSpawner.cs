using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class WaveSpawner : MonoBehaviour
{
    public List<BaseEnemy> enemyPrefabs;
    public float timeSpawnNewEnemy;
    public MapData mapData;
    public int currentWaveID;
    public int currentEnemyID;
    public Button startWave;
    public GameObject startWaveCanvas;
    public LevelData levelData;
    public Image progress;
    public List<GameObject> characterList;

    public int currentShowID = 0;

    public static WaveSpawner Instance;
    void Start()
    {
        StartCoroutine(WaveSpawnHandle());
        Instance = this;
    }
    bool clickPlayWave = false;
    bool isStartWave = true;
    IEnumerator WaveSpawnHandle()
    {
        int currentWaveIDLocal = currentWaveID;
        if (currentWaveID == UIManager.Instance.totalWaveCount) yield break;
        bool showPlayWave = false;
        bool showButtonNewWave = false; 
        currentEnemyID = 0;
        float timer = 0;
        Debug.Log(currentWaveIDLocal);
        float maxTime = levelData.listWaveDatas[currentWaveIDLocal].timeForWave;
        float waitToSpawn = levelData.listWaveDatas[currentWaveIDLocal].timeForReady;
        float waitOfNext = (currentWaveIDLocal +1) >= UIManager.Instance.totalWaveCount ? 1000 : levelData.listWaveDatas[currentWaveIDLocal + 1].timeForReady;
        startWaveCanvas.SetActive(true);
        startWave.onClick.RemoveAllListeners();
        startWave.onClick.AddListener(() =>
        {
            startWaveCanvas.SetActive(false);
            if(!isStartWave){
                DataManager.Instance.gamePlayData.waveCount++;
                UIManager.Instance.UpdateUIInGame();
            }
            else
            {
                isStartWave = false;
            }
        });
        while (startWaveCanvas.activeInHierarchy && (timer < waitToSpawn || currentWaveIDLocal == 0))
        { 
            timer += Time.deltaTime;
            progress.fillAmount = timer / waitToSpawn;
            yield return null;
        }
        timer = 0;
        EnemySpawnData enemySpawnData = levelData.listWaveDatas[currentWaveIDLocal].listEnemySpawnDatas[currentEnemyID];
        float waitTimeNewEnemy = enemySpawnData.timeInWave;
        while (timer < maxTime +waitOfNext) 
        {
            timer += Time.deltaTime;
            if(timer > waitTimeNewEnemy)
            {
                bool showChar = currentEnemyID == levelData.listWaveDatas[currentWaveIDLocal].listEnemySpawnDatas.Count - 1;
                SpawnEnemy(enemySpawnData.enemyType, enemySpawnData.listPathID, enemySpawnData.laneID, showChar);
                currentEnemyID++;
                if(currentEnemyID == levelData.listWaveDatas[currentWaveIDLocal].listEnemySpawnDatas.Count)
                {
                    waitTimeNewEnemy = 1000;
                    continue;
                }
                enemySpawnData= levelData.listWaveDatas[currentWaveIDLocal].listEnemySpawnDatas[currentEnemyID];
                waitTimeNewEnemy = enemySpawnData.timeInWave;
            }
            if(timer > maxTime && !showPlayWave)
            {
                currentWaveID++;
                StartCoroutine(  WaveSpawnHandle());
                showPlayWave = true;
            }
            yield return null;
        }
    }
    public void SpawnEnemy(EnemyType enemyType, List<int> listWayPoint, int laneID, bool showChar)
    {
        BaseEnemy newEnemy = Instantiate(enemyPrefabs[(int)enemyType]);
        newEnemy.gameObject.SetActive(true);
        newEnemy.waypointMover.waypoints = mapData.GetPath(listWayPoint, laneID);
        newEnemy.transform.position = newEnemy.waypointMover.waypoints[0].position;
        newEnemy.hasChar = showChar;
    }
    public void ShowNewChar()
    {
        characterList[currentShowID].SetActive(true);
        currentShowID++;
    }
    public TextMeshProUGUI GetText()
    {
        return characterList[currentShowID].GetComponentInChildren<TextMeshProUGUI>();
    }
}
