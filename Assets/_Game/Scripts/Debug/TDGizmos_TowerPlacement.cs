// TDGizmos_TowerPlacement.cs
// Tower Defense Gizmos — Group 2: Tower Placement & Building (16–30)
// Unity 2020+ (khuyên LTS)

using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class TDGizmos_TowerPlacement : MonoBehaviour
{
    [Header("=== Common Colors ===")]
    public Color colGrid = new(0.9f, 0.9f, 0.9f, 0.15f);
    public Color colOK = new(0.2f, 1f, 0.3f, 0.45f);
    public Color colBad = new(1f, 0.2f, 0.2f, 0.45f);
    public Color colRange = new(1f, 0.85f, 0.2f, 0.85f);
    public Color colUpg = new(0.2f, 0.8f, 1f, 0.85f);
    public Color colLOS = new(0.6f, 1f, 0.9f, 0.85f);
    public Color colSell = new(1f, 0.4f, 0.6f, 0.85f);
    public Color colOverlap1 = new(0.2f, 1f, 0.3f, 0.25f);
    public Color colOverlap2 = new(1f, 0.8f, 0.2f, 0.35f);
    public Color colOverlap3 = new(1f, 0.2f, 0.2f, 0.45f);

    [Header("=== Grid Config ===")]
    public Vector2Int gridCount = new(20, 12);        // (X,Z) số ô
    public float cell = 1.0f;                          // kích thước ô
    public Vector3 gridCenter;                         // tâm lưới (world)

    [Header("=== Layers & Data Sources ===")]
    public LayerMask buildBlockMask = ~0;              // vật cản xây (rocks, path, units...)
    public LayerMask terrainMask = ~0;                 // raycast lấy cao độ
    public Transform[] mainPathWaypoints;              // phục vụ No-Build gần path

    [Header("=== Towers (để vẽ range/level/coverage...) ===")]
    public Transform[] towers;                         // danh sách tower đã đặt
    public float[] towerRange;                         // bán kính tấn công hiện tại
    public float[] towerUpgradeRange;                  // bán kính sau nâng cấp
    public int[] towerLevel;                           // level hiện tại
    public int[] towerCost;                            // giá tower (nếu cần hiện)

    [Header("=== Ghost (đang preview đặt) ===")]
    public Transform ghostTower;
    public Vector2Int footprintCells = new(2, 2);      // footprint theo ô
    public float ghostRange = 3.5f;                    // range ghost
    public float ghostUpgradeRange = 4.5f;             // range sau nâng cấp
    [Range(0, 180)] public float ghostLOSAngle = 90f;   // fan-cone góc bắn
    public Transform ghostForwardRef;                  // hướng bắn (nếu null dùng ghostTower.forward)
    public int ghostCost = 100;                        // giá ghost
    public float sellRadius = 4.0f;                    // vùng ảnh hưởng khi bán

    [Header("=== 16) Buildable Grid Overlay ===")]
    public bool drawBuildableGrid = true;

    [Header("=== 17) Invalid Placement Cells ===")]
    public bool drawInvalidCells = true;

    [Header("=== 18) Tower Footprint ===")]
    public bool drawFootprint = true;

    [Header("=== 19) Tower Range Circle ===")]
    public bool drawTowerRange = true;

    [Header("=== 20) Upgrade Range Circle ===")]
    public bool drawUpgradeRange = true;

    [Header("=== 21) Tower Line of Sight (Fan Cone) ===")]
    public bool drawLOS = true;

    [Header("=== 22) Tower Height Indicator ===")]
    public bool drawHeightIndicator = true;
    public float heightRayUp = 50f;                    // ray lên
    public float heightRayDown = 200f;                 // ray xuống

    [Header("=== 23) Tower/Cell Cost Label ===")]
    public bool drawCostLabel = true;
    public bool costLabelEveryCell = false;            // true = mỗi ô đều ghi giá (có thể nặng)

    [Header("=== 24) Tower Level Indicator ===")]
    public bool drawLevelLabel = true;

    [Header("=== 25) Placement Preview Ghost (full) ===")]
    public bool drawGhostPreview = true;

    [Header("=== 26) Tower Coverage Overlap ===")]
    public bool drawCoverageOverlap = true;
    public Vector2Int overlapRes = new(36, 24);        // mẫu heatmap coverage

    [Header("=== 27) No-Build Zone near Path ===")]
    public bool drawNoBuildNearPath = true;
    public float minDistanceToPath = 1.0f;             // ô cách path < min → cấm xây

    [Header("=== 28) Terrain Height Debug ===")]
    public bool drawTerrainHeight = true;
    public float heightLow = 0f, heightHigh = 10f;     // min/max để normalize màu

    [Header("=== 29) Build Queue Indicator ===")]
    public bool drawBuildQueue = true;
    public Transform[] constructingTowers;             // tower đang xây
    [Range(0, 1)] public float[] constructingProgress;  // tiến độ 0..1

    [Header("=== 30) Sell Range Preview ===")]
    public bool drawSellRange = true;

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        if (!enabled) return;

        // 16) Grid overlay
        if (drawBuildableGrid) DrawGridOverlay();

        // 17) Invalid cells (physics cản)
        if (drawInvalidCells) DrawInvalidCells(buildBlockMask);

        // 27) No-Build near path
        if (drawNoBuildNearPath) DrawNoBuildNearPath();

        // 28) Terrain Height Debug
        if (drawTerrainHeight) DrawTerrainHeightMap();

        // 18) Footprint (theo ghost)
        if (drawFootprint && ghostTower) DrawGhostFootprintCells();

        // 19,20) Range & Upgrade Range
        if (drawTowerRange) DrawAllTowerRanges();
        if (drawUpgradeRange) DrawAllTowerUpgradeRanges();

        // 21) LOS Fan (ghost)
        if (drawLOS && ghostTower) DrawLOS_Fan(ghostTower.position, GetGhostForward(), ghostLOSAngle, ghostRange);

        // 22) Height indicator cho towers
        if (drawHeightIndicator) DrawHeightIndicators();

        // 23) Cost label
        if (drawCostLabel) DrawCostLabels();

        // 24) Level label
        if (drawLevelLabel) DrawLevelLabels();

        // 25) Ghost preview (full: footprint + range + LOS + cost)
        if (drawGhostPreview && ghostTower) DrawGhostFullPreview();

        // 26) Coverage overlap heat (đếm số tower cover 1 điểm)
        if (drawCoverageOverlap) DrawCoverageOverlapHeat();

        // 29) Build queue indicator
        if (drawBuildQueue) DrawBuildQueueIndicators();

        // 30) Sell range preview (ghost)
        if (drawSellRange && ghostTower) DrawDisc(ghostTower.position, Vector3.up, sellRadius, 72, colSell);
    }
#endif

    // ============================================================
    // =============== GRID & CELL HELPERS ========================
    // ============================================================

    Vector3 OriginWorld() =>
        gridCenter - new Vector3(gridCount.x * cell, 0, gridCount.y * cell) * 0.5f;

    Vector3 CellCenterWorld(int gx, int gz)
    {
        Vector3 origin = OriginWorld();
        return origin + new Vector3((gx + 0.5f) * cell, 0, (gz + 0.5f) * cell);
    }

    bool WorldToGrid(Vector3 world, out int gx, out int gz)
    {
        Vector3 origin = OriginWorld();
        float fx = (world.x - origin.x) / cell - 0.5f;
        float fz = (world.z - origin.z) / cell - 0.5f;
        gx = Mathf.FloorToInt(fx);
        gz = Mathf.FloorToInt(fz);
        return (gx >= 0 && gz >= 0 && gx < gridCount.x && gz < gridCount.y);
    }

    // ============================================================
    // =============== (16) GRID OVERLAY ==========================
    // ============================================================

    void DrawGridOverlay()
    {
        Gizmos.color = colGrid;
        Vector3 size = new(gridCount.x * cell, 0, gridCount.y * cell);
        Gizmos.DrawWireCube(gridCenter, size);

        // vẽ ô
        for (int z = 0; z < gridCount.y; z++)
            for (int x = 0; x < gridCount.x; x++)
                DrawGridCell(CellCenterWorld(x, z), colGrid);
    }

    void DrawGridCell(Vector3 center, Color c)
    {
        Gizmos.color = c;
        Vector3 half = new(cell * 0.5f, 0, cell * 0.5f);
        Vector3 a = center + new Vector3(-half.x, 0, -half.z);
        Vector3 b = center + new Vector3(half.x, 0, -half.z);
        Vector3 d = center + new Vector3(-half.x, 0, half.z);
        Vector3 e = center + new Vector3(half.x, 0, half.z);
        Gizmos.DrawLine(a, b); Gizmos.DrawLine(b, e);
        Gizmos.DrawLine(e, d); Gizmos.DrawLine(d, a);
    }

    // ============================================================
    // =============== (17) INVALID PLACEMENT CELLS ===============
    // ============================================================

    void DrawInvalidCells(LayerMask mask)
    {
        for (int z = 0; z < gridCount.y; z++)
            for (int x = 0; x < gridCount.x; x++)
            {
                Vector3 c = CellCenterWorld(x, z);
                bool blocked = Physics.CheckBox(
                    c,
                    new Vector3(cell, 0.2f, cell) * 0.49f,
                    Quaternion.identity,
                    mask,
                    QueryTriggerInteraction.Ignore);

                if (blocked)
                {
                    Gizmos.color = colBad;
                    Gizmos.DrawCube(c, new Vector3(cell, 0.02f, cell) * 0.95f);
                }
            }
    }

    // ============================================================
    // =============== (27) NO-BUILD NEAR PATH ====================
    // ============================================================

    void DrawNoBuildNearPath()
    {
        if (mainPathWaypoints == null || mainPathWaypoints.Length < 2) return;

        for (int z = 0; z < gridCount.y; z++)
            for (int x = 0; x < gridCount.x; x++)
            {
                Vector3 c = CellCenterWorld(x, z);
                float d = DistancePointToPolylineXZ(c, mainPathWaypoints);
                if (d < minDistanceToPath)
                {
                    Gizmos.color = new Color(colBad.r, colBad.g, colBad.b, 0.35f);
                    Gizmos.DrawCube(c, new Vector3(cell, 0.02f, cell) * 0.95f);
                }
            }

        // trực quan hoá đường path
        Gizmos.color = new Color(0.2f, 0.7f, 1f, 0.75f);
        for (int i = 0; i < mainPathWaypoints.Length - 1; i++)
            if (mainPathWaypoints[i] && mainPathWaypoints[i + 1])
                Gizmos.DrawLine(mainPathWaypoints[i].position, mainPathWaypoints[i + 1].position);
    }

    // khoảng cách điểm → polyline (lấy nhỏ nhất giữa các đoạn) trên mặt phẳng XZ
    float DistancePointToPolylineXZ(Vector3 p, Transform[] wps)
    {
        float best = float.MaxValue;
        for (int i = 0; i < wps.Length - 1; i++)
        {
            if (!wps[i] || !wps[i + 1]) continue;
            float d = DistancePointToSegmentXZ(p, wps[i].position, wps[i + 1].position);
            if (d < best) best = d;
        }
        return best;
    }
    float DistancePointToSegmentXZ(Vector3 p, Vector3 a, Vector3 b)
    {
        Vector2 P = new(p.x, p.z);
        Vector2 A = new(a.x, a.z);
        Vector2 B = new(b.x, b.z);
        Vector2 AB = B - A;
        float t = Vector2.Dot(P - A, AB) / Mathf.Max(1e-6f, AB.sqrMagnitude);
        t = Mathf.Clamp01(t);
        Vector2 proj = A + AB * t;
        return Vector2.Distance(P, proj);
    }

    // ============================================================
    // =============== (28) TERRAIN HEIGHT DEBUG ==================
    // ============================================================

    void DrawTerrainHeightMap()
    {
        for (int z = 0; z < gridCount.y; z++)
            for (int x = 0; x < gridCount.x; x++)
            {
                Vector3 c = CellCenterWorld(x, z) + Vector3.up * heightRayUp;
                if (Physics.Raycast(c, Vector3.down, out var hit, heightRayUp + heightRayDown, terrainMask))
                {
                    float h = hit.point.y;
                    float t = Mathf.InverseLerp(heightLow, heightHigh, h);
                    Color hc = Color.Lerp(new Color(0.2f, 0.6f, 1f, 0.3f), new Color(1f, 0.4f, 0.2f, 0.5f), t);
                    Gizmos.color = hc;
                    Gizmos.DrawCube(hit.point + Vector3.up * 0.01f, new Vector3(cell * 0.9f, 0.02f, cell * 0.9f));
                }
            }
    }

    // ============================================================
    // =============== (18) GHOST FOOTPRINT CELLS =================
    // ============================================================

    void DrawGhostFootprintCells()
    {
        if (!WorldToGrid(ghostTower.position, out int gx, out int gz)) return;

        for (int dz = 0; dz < footprintCells.y; dz++)
            for (int dx = 0; dx < footprintCells.x; dx++)
            {
                int cx = gx + dx;
                int cz = gz + dz;
                if (cx < 0 || cz < 0 || cx >= gridCount.x || cz >= gridCount.y) continue;

                Vector3 c = CellCenterWorld(cx, cz);
                bool blocked = Physics.CheckBox(
                    c,
                    new Vector3(cell, 0.2f, cell) * 0.49f,
                    Quaternion.identity,
                    buildBlockMask,
                    QueryTriggerInteraction.Ignore);

                Gizmos.color = blocked ? colBad : colOK;
                Gizmos.DrawCube(c, new Vector3(cell, 0.02f, cell) * 0.95f);
            }
    }

    // ============================================================
    // =============== (19) TOWER RANGE CIRCLE ====================
    // ============================================================

    void DrawAllTowerRanges()
    {
        if (towers == null || towerRange == null) return;
        int n = Mathf.Min(towers.Length, towerRange.Length);
        for (int i = 0; i < n; i++)
        {
            if (!towers[i]) continue;
            DrawDisc(towers[i].position, Vector3.up, Mathf.Max(0, towerRange[i]), 72, colRange);
        }
        if (ghostTower) DrawDisc(ghostTower.position, Vector3.up, ghostRange, 72, new Color(colRange.r, colRange.g, colRange.b, 0.5f));
    }

    // ============================================================
    // =============== (20) UPGRADE RANGE CIRCLE ==================
    // ============================================================

    void DrawAllTowerUpgradeRanges()
    {
        if (towers != null && towerUpgradeRange != null)
        {
            int n = Mathf.Min(towers.Length, towerUpgradeRange.Length);
            for (int i = 0; i < n; i++)
            {
                if (!towers[i]) continue;
                DrawDisc(towers[i].position, Vector3.up, Mathf.Max(0, towerUpgradeRange[i]), 72, colUpg);
            }
        }
        if (ghostTower) DrawDisc(ghostTower.position, Vector3.up, ghostUpgradeRange, 72, new Color(colUpg.r, colUpg.g, colUpg.b, 0.5f));
    }

    // ============================================================
    // =============== (21) LINE OF SIGHT (FAN CONE) ==============
    // ============================================================

    Vector3 GetGhostForward()
    {
        if (ghostForwardRef) return ghostForwardRef.forward;
        if (ghostTower) return ghostTower.forward;
        return Vector3.forward;
    }

    void DrawLOS_Fan(Vector3 origin, Vector3 forward, float angle, float radius)
    {
        forward.y = 0; forward.Normalize();
        Color rim = colLOS;
        DrawFan(origin, Vector3.up, forward, radius, angle, 48, rim);
    }

    // ============================================================
    // =============== (22) HEIGHT INDICATOR ======================
    // ============================================================

    void DrawHeightIndicators()
    {
        if (towers == null) return;
        foreach (var t in towers)
        {
            if (!t) continue;
            Vector3 up = t.position + Vector3.up * heightRayUp;
            Vector3 down = t.position + Vector3.up * heightRayUp + Vector3.down * (heightRayUp + heightRayDown);
            if (Physics.Raycast(up, Vector3.down, out var hit, heightRayUp + heightRayDown, terrainMask))
            {
                Gizmos.color = new Color(1f, 1f, 1f, 0.8f);
                Gizmos.DrawLine(t.position, hit.point);
#if UNITY_EDITOR
                UnityEditor.Handles.Label(hit.point + Vector3.up * 0.25f, $"h= {hit.point.y:F2}");
#endif
            }
            else
            {
                Gizmos.color = new Color(1f, 1f, 1f, 0.3f);
                Gizmos.DrawLine(t.position, down);
            }
        }
    }

    // ============================================================
    // =============== (23) COST LABEL ============================
    // ============================================================

    void DrawCostLabels()
    {
#if UNITY_EDITOR
        // Cost mỗi tower
        if (towers != null && towerCost != null)
        {
            int n = Mathf.Min(towers.Length, towerCost.Length);
            for (int i = 0; i < n; i++)
            {
                if (!towers[i]) continue;
                UnityEditor.Handles.Label(towers[i].position + Vector3.up * 1.2f, $"${towerCost[i]}");
            }
        }
        // Cost ghost
        if (ghostTower)
            UnityEditor.Handles.Label(ghostTower.position + Vector3.up * 1.2f, $"${ghostCost}");

        // (tuỳ chọn) cost trên mọi ô grid
        if (costLabelEveryCell)
        {
            for (int z = 0; z < gridCount.y; z++)
                for (int x = 0; x < gridCount.x; x++)
                    UnityEditor.Handles.Label(CellCenterWorld(x, z) + Vector3.up * 0.05f, $"${ghostCost}");
        }
#endif
    }

    // ============================================================
    // =============== (24) LEVEL LABEL ===========================
    // ============================================================

    void DrawLevelLabels()
    {
#if UNITY_EDITOR
        if (towers == null || towerLevel == null) return;
        int n = Mathf.Min(towers.Length, towerLevel.Length);
        for (int i = 0; i < n; i++)
        {
            if (!towers[i]) continue;
            UnityEditor.Handles.Label(towers[i].position + Vector3.up * 1.6f, $"Lv {towerLevel[i]}");
        }
#endif
    }

    // ============================================================
    // =============== (25) GHOST FULL PREVIEW ====================
    // ============================================================

    void DrawGhostFullPreview()
    {
        // footprint validity
        DrawGhostFootprintCells();
        // ranges
        DrawDisc(ghostTower.position, Vector3.up, ghostRange, 72, new Color(colRange.r, colRange.g, colRange.b, 0.55f));
        DrawDisc(ghostTower.position, Vector3.up, ghostUpgradeRange, 72, new Color(colUpg.r, colUpg.g, colUpg.b, 0.45f));
        // LOS
        DrawLOS_Fan(ghostTower.position, GetGhostForward(), ghostLOSAngle, ghostRange);
#if UNITY_EDITOR
        // cost label
        UnityEditor.Handles.Label(ghostTower.position + Vector3.up * 1.2f, $"${ghostCost}");
#endif
    }

    // ============================================================
    // =============== (26) COVERAGE OVERLAP HEAT ================
    // ============================================================

    void DrawCoverageOverlapHeat()
    {
        if (towers == null || towerRange == null) return;
        int n = Mathf.Min(towers.Length, towerRange.Length);
        if (n == 0) return;

        Vector3 origin = gridCenter - new Vector3(gridCount.x * cell, 0, gridCount.y * cell) * 0.5f;
        Vector3 areaSize = new(gridCount.x * cell, 0, gridCount.y * cell);

        for (int j = 0; j < overlapRes.y; j++)
            for (int i = 0; i < overlapRes.x; i++)
            {
                Vector3 p = origin + new Vector3(areaSize.x * (i + 0.5f) / overlapRes.x, 0, areaSize.z * (j + 0.5f) / overlapRes.y);
                int count = 0;
                for (int k = 0; k < n; k++)
                {
                    if (!towers[k]) continue;
                    float d = Vector3.Distance(new Vector3(p.x, towers[k].position.y, p.z), towers[k].position);
                    if (d <= towerRange[k]) count++;
                }
                if (count <= 0) continue;

                Color c = count == 1 ? colOverlap1 : (count == 2 ? colOverlap2 : colOverlap3);
                Gizmos.color = c;
                float sx = areaSize.x / overlapRes.x * 0.95f;
                float sz = areaSize.z / overlapRes.y * 0.95f;
                Gizmos.DrawCube(p + Vector3.up * 0.01f, new Vector3(sx, 0.02f, sz));
            }
    }

    // ============================================================
    // =============== (29) BUILD QUEUE INDICATOR =================
    // ============================================================

    void DrawBuildQueueIndicators()
    {
        if (constructingTowers == null || constructingProgress == null) return;
        int n = Mathf.Min(constructingTowers.Length, constructingProgress.Length);
        for (int i = 0; i < n; i++)
        {
            if (!constructingTowers[i]) continue;
            float t = Mathf.Clamp01(constructingProgress[i]);
            Vector3 pos = constructingTowers[i].position + Vector3.up * 1.4f;
            DrawProgressPie(pos, 0.5f, t);
#if UNITY_EDITOR
            UnityEditor.Handles.Label(pos + Vector3.up * 0.2f, $"{t * 100f:F0}%");
#endif
        }
    }

    // ============================================================
    // =============== (30) SELL RANGE PREVIEW ====================
    // ============================================================
    // (đã vẽ ở OnDrawGizmos nếu ghostTower != null)

    // ============================================================
    // ================== DRAW PRIMITIVES =========================
    // ============================================================

    void DrawDisc(Vector3 center, Vector3 normal, float radius, int seg, Color color)
    {
        if (radius <= 0f) return;
        Gizmos.color = color;
        normal = normal.sqrMagnitude < 1e-6f ? Vector3.up : normal.normalized;
        Vector3 u = Vector3.Cross(normal, Vector3.up);
        if (u.sqrMagnitude < 1e-6f) u = Vector3.Cross(normal, Vector3.right);
        u.Normalize();
        Vector3 v = Vector3.Cross(normal, u);
        Vector3 prev = center + u * radius;
        for (int i = 1; i <= seg; i++)
        {
            float ang = (i / (float)seg) * Mathf.PI * 2f;
            Vector3 p = center + (Mathf.Cos(ang) * u + Mathf.Sin(ang) * v) * radius;
            Gizmos.DrawLine(prev, p);
            prev = p;
        }
    }

    void DrawFan(Vector3 center, Vector3 normal, Vector3 forward, float radius, float angleDeg, int seg, Color color)
    {
        Gizmos.color = color;
        normal = normal.sqrMagnitude < 1e-6f ? Vector3.up : normal.normalized;
        forward = forward.sqrMagnitude < 1e-6f ? Vector3.forward : forward.normalized;

        float half = angleDeg * 0.5f;
        Vector3 leftDir = Quaternion.AngleAxis(-half, normal) * forward;
        Vector3 rightDir = Quaternion.AngleAxis(half, normal) * forward;

        Gizmos.DrawLine(center, center + leftDir * radius);
        Gizmos.DrawLine(center, center + rightDir * radius);

        Vector3 prev = leftDir;
        for (int i = 1; i <= seg; i++)
        {
            float a = Mathf.Lerp(-half, half, i / (float)seg);
            Vector3 dir = Quaternion.AngleAxis(a, normal) * forward;
            Gizmos.DrawLine(center + prev * radius, center + dir * radius);
            prev = dir;
        }
    }

    void DrawProgressPie(Vector3 center, float r, float t01)
    {
        t01 = Mathf.Clamp01(t01);
        int seg = Mathf.Max(6, Mathf.RoundToInt(60 * t01));
        Vector3 normal = Vector3.up;

        Vector3 u = Vector3.right, v = Vector3.forward;
        Vector3 prev = center;
        for (int i = 0; i <= seg; i++)
        {
            float ang = (i / (float)seg) * Mathf.PI * 2f * t01;
            Vector3 rim = center + (Mathf.Cos(ang) * u + Mathf.Sin(ang) * v) * r;
            Gizmos.color = new Color(1f, 0.85f, 0.2f, 0.8f);
            Gizmos.DrawLine(center, rim);
            if (i > 0) Gizmos.DrawLine(prev, rim);
            prev = rim;
        }
        // viền tròn nhạt
        DrawDisc(center, normal, r, 48, new Color(1f, 1f, 1f, 0.3f));
    }
}
