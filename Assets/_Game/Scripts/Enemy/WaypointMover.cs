using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointMover : MonoBehaviour
{
    public List<Transform> waypoints; // List waypoint theo thứ tự
    public float moveSpeed = 3f;       // Tốc độ di chuyển
    public float reachDistance = 0.1f; // Khoảng cách coi là đã tới waypoint

    private int currentIndex = 0;
    public bool isAttack = false;
    public bool isFly = false;
    Vector3 offset;

    private void Start()
    {
        offset = isFly ? Vector3.up * 1.5f : Vector3.zero;
        if (waypoints.Count > 0)
            StartCoroutine(MoveAlongWaypoints());
    }

    private IEnumerator MoveAlongWaypoints()
    {
        while (currentIndex < waypoints.Count)
        {
            Transform target = waypoints[currentIndex];

            // Di chuyển tới waypoint hiện tại
            while (Vector3.Distance(transform.position, target.position+ offset) > reachDistance)
            {
                if (isAttack)
                {
                    yield return null;
                    continue;
                }
                transform.position = Vector3.MoveTowards(
                    transform.position,
                    target.position+ offset,
                    moveSpeed * Time.deltaTime
                );
                transform.rotation = target.rotation;
                yield return null;
            }

            // Chuyển sang waypoint tiếp theo
            currentIndex++;
            yield return null;
        }
    }
}
