using System.Collections;
using System.Collections.Generic;
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
    void Start()
    {
        StartCoroutine(WaveSpawnHandle());
    }
    bool clickPlayWave = false;
    IEnumerator WaveSpawnHandle()
    {
        int currentWaveIDLocal = currentWaveID;
        bool showPlayWave = false;
        bool showButtonNewWave = false; 
        currentEnemyID = 0;
        float timer = 0;
        float maxTime = levelData.listWaveDatas[currentWaveIDLocal].timeForWave;
        float waitToSpawn = levelData.listWaveDatas[currentWaveIDLocal].timeForWave;
        float waitOfNext = levelData.listWaveDatas[currentWaveIDLocal + 1].timeForWave;
        startWaveCanvas.SetActive(true);
        startWave.onClick.AddListener(() =>
        {
            startWaveCanvas.SetActive(false);
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
                SpawnEnemy(enemySpawnData.enemyType, enemySpawnData.listPathID, enemySpawnData.laneID);
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
                StartCoroutine(WaveSpawnHandle());
                showPlayWave = true;
            }
            yield return null;
        }
        if (currentWaveID == currentWaveIDLocal)
        {
            currentWaveID++;
            startWaveCanvas.SetActive(false);
            StartCoroutine (WaveSpawnHandle());
        }
        yield return null;
    }
    public void SpawnEnemy(EnemyType enemyType, List<int> listWayPoint, int laneID)
    {
        BaseEnemy newEnemy = Instantiate(enemyPrefabs[(int)enemyType]);
        newEnemy.gameObject.SetActive(true);
        newEnemy.waypointMover.waypoints = mapData.GetPath(listWayPoint, laneID);
        newEnemy.transform.position = newEnemy.waypointMover.waypoints[0].position;
    }
}
