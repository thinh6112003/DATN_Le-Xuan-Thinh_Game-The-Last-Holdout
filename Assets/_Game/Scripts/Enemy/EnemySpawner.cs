using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Transform spawnPos;
    public BaseEnemy baseEnemyPrefab;
    public BaseEnemy baseEnemyPrefab2;
    public List<Transform> listWaypoint;
    public List<Transform> listWaypoint2;
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
            int random2 = Random.Range(0,2);
            BaseEnemy newEnemy = Instantiate( random==0 ? baseEnemyPrefab : baseEnemyPrefab2);
            newEnemy.gameObject.SetActive(true);
            newEnemy.transform.position = spawnPos.position;
            newEnemy.waypointMover.waypoints = random2 == 0 ? listWaypoint: listWaypoint2;

            float randomTime = Random.Range(timeSpawnNewEnemy, timeSpawnNewEnemy * 1.75f);
            yield return new WaitForSeconds(timeSpawnNewEnemy);
        }
    }
}
