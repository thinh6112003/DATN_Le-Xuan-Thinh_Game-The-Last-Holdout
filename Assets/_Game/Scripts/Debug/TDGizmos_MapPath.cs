// TDGizmos_MapPath.cs
// 15 Gizmos nhóm Map & Path cho Tower Defense
// Unity 2020+ (khuyên LTS)

using UnityEngine;

[ExecuteAlways]
public class TDGizmos_MapPath : MonoBehaviour
{
    [Header("Common Settings")]
    public Color colPath = new(0.2f, 0.9f, 1f, 1f);
    public Color colArrow = new(1f, 0.85f, 0.2f, 1f);
    public Color colWarn = new(1f, 0.25f, 0.25f, 1f);
    public Color colOK = new(0.2f, 1f, 0.3f, 1f);
    public Color colZone = new(0.9f, 0.4f, 1f, .6f);

    public float arrowHeadLen = 0.35f;
    public float arrowHeadAngle = 20f;

    [Header("Waypoints chính")]
    public Transform[] mainWaypoints;

    // ===== Map & Path demos (1–15) =====
    public bool drawEnemyPath = true;             // 1
    public bool drawWaypointArrows = true;        // 2
    public bool drawMultiplePaths = true;         // 3
    public Transform[][] extraPaths = new Transform[0][];
    public bool drawBlockedPath = true;           // 4
    public LayerMask blockMask = ~0;

    public bool drawSpawnZones = true;            // 5
    public Transform[] spawnPoints;
    public float spawnRadius = 1f;

    public bool drawGoalZone = true;              // 6
    public Transform goal;
    public Vector3 goalSize = new(2, 0.2f, 2);

    public bool drawPathHeatmap = true;           // 7
    public Vector2Int heatmapSize = new(12, 8);
    public float heatCell = 1f;

    public bool drawAltRoute = true;              // 8
    public Transform[] altRoute;

    public bool drawFlyingPath = true;            // 9
    public Transform[] flyingWaypoints;

    public bool drawDynamicPath = true;           // 10
    public Transform[] dynamicPath;

    public bool drawWaypointLabels = true;        // 11

    public bool drawPatrolPath = true;            // 12
    public Transform[] patrolWaypoints;

    public bool drawRoadWidth = true;             // 13
    public float roadWidth = 2f;

    public bool drawTunnelBridge = true;          // 14
    public Transform[] tunnelWaypoints;

    public bool drawDangerZones = true;           // 15
    public Transform[] dangerZones;
    public float dangerRadius = 1.5f;

    // ========== OnDrawGizmos ==========
    void OnDrawGizmos()
    {
        if (!enabled) return;

        // (1) Enemy Path Preview
        if (drawEnemyPath) DrawPath(mainWaypoints, colPath);

        // (2) Waypoint Arrows
        if (drawWaypointArrows) DrawPathArrows(mainWaypoints, colArrow);

        // (3) Multiple Path Highlight
        if (drawMultiplePaths && extraPaths != null)
        {
            foreach (var path in extraPaths)
                DrawPath(path, new Color(0.3f, 0.7f, 1f, .6f));
        }

        // (4) Blocked Path Debug
        if (drawBlockedPath) DrawBlockedPath(mainWaypoints);

        // (5) Spawn Zones
        if (drawSpawnZones && spawnPoints != null)
        {
            Gizmos.color = colZone;
            foreach (var sp in spawnPoints)
                if (sp) DrawDisc(sp.position, Vector3.up, spawnRadius, 48);
        }

        // (6) Goal Zone
        if (drawGoalZone && goal)
        {
            Gizmos.color = colZone;
            Gizmos.DrawWireCube(goal.position, goalSize);
        }

        // (7) Path Heatmap
        if (drawPathHeatmap) DrawHeatmap(transform.position, heatmapSize, heatCell);

        // (8) Alternate Route
        if (drawAltRoute) DrawPath(altRoute, new Color(1f, .5f, .2f, .8f));

        // (9) Flying Path
        if (drawFlyingPath) DrawPath(flyingWaypoints, new Color(.7f, .7f, 1f, 1f));

        // (10) Dynamic Path
        if (drawDynamicPath) DrawPath(dynamicPath, new Color(1f, .9f, .1f, 1f));

        // (11) Waypoint Labels
        if (drawWaypointLabels) DrawLabels(mainWaypoints);

        // (12) Patrol Path
        if (drawPatrolPath) DrawPath(patrolWaypoints, new Color(.2f, 1f, .5f, 1f));

        // (13) Road Width
        if (drawRoadWidth) DrawRoadWidth(mainWaypoints, roadWidth);

        // (14) Tunnel/Bridge
        if (drawTunnelBridge) DrawPath(tunnelWaypoints, new Color(.8f, .3f, 1f, 1f));

        // (15) Danger Zones
        if (drawDangerZones && dangerZones != null)
        {
            Gizmos.color = colWarn;
            foreach (var dz in dangerZones)
                if (dz) DrawDisc(dz.position, Vector3.up, dangerRadius, 48);
        }
    }

    // ========== Helpers ==========

    void DrawPath(Transform[] pts, Color col)
    {
        if (pts == null || pts.Length < 2) return;
        Gizmos.color = col;
        for (int i = 0; i < pts.Length - 1; i++)
        {
            if (!pts[i] || !pts[i + 1]) continue;
            Gizmos.DrawLine(pts[i].position, pts[i + 1].position);
            Gizmos.DrawWireSphere(pts[i].position, 0.1f);
        }
        Gizmos.DrawWireSphere(pts[pts.Length - 1].position, 0.12f);
    }

    void DrawPathArrows(Transform[] pts, Color col)
    {
        if (pts == null || pts.Length < 2) return;
        Gizmos.color = col;
        for (int i = 0; i < pts.Length - 1; i++)
        {
            if (!pts[i] || !pts[i + 1]) continue;
            DrawArrow(pts[i].position, pts[i + 1].position);
        }
    }

    void DrawBlockedPath(Transform[] pts)
    {
        if (pts == null || pts.Length < 2) return;
        for (int i = 0; i < pts.Length - 1; i++)
        {
            if (!pts[i] || !pts[i + 1]) continue;
            var a = pts[i].position; var b = pts[i + 1].position;
            bool blocked = Physics.Linecast(a + Vector3.up * 0.05f, b + Vector3.up * 0.05f, blockMask);
            Gizmos.color = blocked ? colWarn : colOK;
            Gizmos.DrawLine(a, b);
        }
    }

    void DrawHeatmap(Vector3 center, Vector2Int size, float cell)
    {
        Vector3 origin = center - new Vector3(size.x * cell, 0, size.y * cell) * 0.5f;
        for (int z = 0; z < size.y; z++)
            for (int x = 0; x < size.x; x++)
            {
                Vector3 c = origin + new Vector3((x + 0.5f) * cell, 0, (z + 0.5f) * cell);
                float val = Mathf.PerlinNoise(x * 0.2f, z * 0.2f); // giả sử density
                Color col = Color.Lerp(colOK, colWarn, val);
                col.a = 0.3f;
                Gizmos.color = col;
                Gizmos.DrawCube(c, new Vector3(cell * 0.9f, 0.02f, cell * 0.9f));
            }
    }

    void DrawLabels(Transform[] pts)
    {
        if (pts == null) return;
#if UNITY_EDITOR
        for (int i = 0; i < pts.Length; i++)
        {
            if (!pts[i]) continue;
            UnityEditor.Handles.Label(pts[i].position + Vector3.up * 0.5f, $"WP {i}");
        }
#endif
    }

    void DrawRoadWidth(Transform[] pts, float width)
    {
        if (pts == null || pts.Length < 2) return;
        Gizmos.color = new Color(.4f, .4f, .4f, .3f);
        for (int i = 0; i < pts.Length - 1; i++)
        {
            if (!pts[i] || !pts[i + 1]) continue;
            Vector3 a = pts[i].position;
            Vector3 b = pts[i + 1].position;
            Vector3 dir = (b - a).normalized;
            Vector3 left = Vector3.Cross(Vector3.up, dir) * width * 0.5f;
            Gizmos.DrawLine(a - left, b - left);
            Gizmos.DrawLine(a + left, b + left);
        }
    }

    void DrawArrow(Vector3 from, Vector3 to)
    {
        Gizmos.DrawLine(from, to);
        Vector3 dir = (to - from).normalized;
        Vector3 right = Quaternion.LookRotation(dir) * Quaternion.Euler(0, 180 + arrowHeadAngle, 0) * Vector3.forward;
        Vector3 left = Quaternion.LookRotation(dir) * Quaternion.Euler(0, 180 - arrowHeadAngle, 0) * Vector3.forward;
        Gizmos.DrawLine(to, to + right * arrowHeadLen);
        Gizmos.DrawLine(to, to + left * arrowHeadLen);
    }

    void DrawDisc(Vector3 c, Vector3 normal, float r, int seg)
    {
        if (r <= 0f) return;
        normal.Normalize();
        Vector3 u = Vector3.Cross(normal, Vector3.up);
        if (u.sqrMagnitude < 1e-6f) u = Vector3.Cross(normal, Vector3.right);
        u.Normalize();
        Vector3 v = Vector3.Cross(normal, u);
        Vector3 prev = c + u * r;
        for (int i = 1; i <= seg; i++)
        {
            float ang = (i / (float)seg) * Mathf.PI * 2f;
            Vector3 p = c + (Mathf.Cos(ang) * u + Mathf.Sin(ang) * v) * r;
            Gizmos.DrawLine(prev, p); prev = p;
        }
    }
}
