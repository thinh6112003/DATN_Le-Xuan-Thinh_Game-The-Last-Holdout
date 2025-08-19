using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Transform spawnPos;
    public BaseEnemy baseEnemyPrefab;
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
            BaseEnemy newEnemy = Instantiate(baseEnemyPrefab);
            newEnemy.transform.position = spawnPos.position;
            newEnemy.waypointMover.waypoints = listWaypoint;
            yield return new WaitForSeconds(timeSpawnNewEnemy);
        }
    }
}
