using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Transform spawnPos;
    public BaseEnemy baseEnemyPrefab;
    public BaseEnemy baseEnemyPrefab2;
    public List<Transform> listWaypoint;
    public float timeSpawnNewEnemy;
    void Start()
    {
        StartCoroutine(SpawnEnemy());
    }
    IEnumerator SpawnEnemy()
    {
        while (true) 
        {
            int random = Random.Range(0,2);
            BaseEnemy newEnemy = Instantiate( random==0 ? baseEnemyPrefab : baseEnemyPrefab2);
            newEnemy.transform.position = spawnPos.position;
            newEnemy.waypointMover.waypoints = listWaypoint;
            float randomTime = Random.Range(timeSpawnNewEnemy, timeSpawnNewEnemy * 1.75f);
            yield return new WaitForSeconds(timeSpawnNewEnemy);
        }
    }
}
