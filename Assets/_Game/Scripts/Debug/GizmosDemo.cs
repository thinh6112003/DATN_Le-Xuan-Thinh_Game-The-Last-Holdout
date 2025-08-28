// GizmosAdvancedDemos.cs
// by Negaxy & ChatGPT — 30+ demo ứng dụng Gizmos nâng cao
// Dành cho Unity 2020+ (khuyến nghị 2021/2022 LTS)

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; // dùng cho demo NavMesh

[ExecuteAlways]
public class GizmosDemo : MonoBehaviour
{
    [Header("=== Dùng chung ===")]
    public float unit = 1f;
    public Color colPrimary = new Color(0.2f, 0.9f, 1f, 1f);
    public Color colAlt = new Color(1f, 0.6f, 0.2f, 1f);
    public Color colWarn = new Color(1f, 0.2f, 0.2f, 1f);
    public Color colOK = new Color(0.2f, 1f, 0.3f, 1f);

    public Transform A;
    public Transform B;
    public Transform C;
    public Transform D;
    public Transform[] waypoints;
    public Camera targetCamera;

    [Header("=== 1) Trục local & scale gizmo ===")]
    public bool demoLocalAxes = true;
    public float axesLen = 1.0f;

    [Header("=== 2) Truncated Field Of View (FOV) ===")]
    public bool demoFOV;
    public float fovAngle = 90f;
    public float fovRadius = 6f;
    public LayerMask fovObstacleMask = ~0;
    public Transform[] fovTargets;

    [Header("=== 3) Bezier Quadratic (A-B-C) ===")]
    public bool demoBezierQuadratic;
    public int bezierQ_Steps = 24;

    [Header("=== 4) Bezier Cubic (A-B-C-D) ===")]
    public bool demoBezierCubic;
    public int bezierC_Steps = 32;

    [Header("=== 5) Catmull-Rom spline (waypoints) ===")]
    public bool demoCatmullRom;
    public int catmullStepsPerSeg = 12;

    [Header("=== 6) Grid XY/XZ (ô lưới debug) ===")]
    public bool demoGrid;
    public enum GridPlane { XY, XZ, YZ }
    public GridPlane gridPlane = GridPlane.XZ;
    public Vector2 gridSize = new Vector2(10, 10);
    public float gridCell = 1f;

    [Header("=== 7) Sector/Arc (góc, bán kính) ===")]
    public bool demoArc;
    public float arcRadius = 5f;
    [Range(0, 360)] public float arcAngle = 120f;

    [Header("=== 8) Dự đoán quỹ đạo bắn parabol ===")]
    public bool demoProjectile;
    public Vector3 launchVelocity = new Vector3(5, 7, 0);
    public int trajSteps = 30;
    public float timeStep = 0.05f;
    public LayerMask trajHitMask = ~0;

    [Header("=== 9) Capsule wire giữa A-B (hitbox) ===")]
    public bool demoCapsuleWire;
    public float capsuleRadius = 0.5f;
    public int capsuleSegments = 16;

    [Header("=== 10) Frustum của Camera ===")]
    public bool demoFrustum;

    [Header("=== 11) Fan Raycast (scan) ===")]
    public bool demoFanRays;
    public int rayCount = 15;
    public float rayFanAngle = 90f;
    public float rayLen = 8f;
    public LayerMask rayMask = ~0;

    [Header("=== 12) Vector field (Perlin Noise) ===")]
    public bool demoNoiseField;
    public Vector2 fieldSize = new Vector2(10, 10);
    public float fieldStep = 1f;
    public float noiseScale = 0.2f;

    [Header("=== 13) Hex Grid (axial) ===")]
    public bool demoHexGrid;
    public int hexQ = 6;
    public int hexR = 6;
    public float hexSize = 0.8f;

    [Header("=== 14) Frenet Frame dọc theo Catmull ===")]
    public bool demoFrenetFrames;
    public float frameLen = 0.5f;

    [Header("=== 15) Kiểm tra LOS giữa waypoints ===")]
    public bool demoLOSBetweenWaypoints;
    public LayerMask losMask = ~0;

    [Header("=== 16) Bounds của tất cả Renderer con ===")]
    public bool demoChildrenBounds;

    [Header("=== 17) OBB từ BoxCollider (dùng matrix) ===")]
    public bool demoOBBFromBoxCollider;
    public BoxCollider boxCollider;

    [Header("=== 18) Normals của MeshFilter ===")]
    public bool demoMeshNormals;
    public MeshFilter meshFilter;
    public float normalLength = 0.2f;
    public int maxNormals = 500;

    [Header("=== 19) Triangles của MeshFilter ===")]
    public bool demoMeshTrianglesWire;

    [Header("=== 20) NavMesh Triangulation ===")]
    public bool demoNavMesh;

    [Header("=== 21) OverlapSphere Debug ===")]
    public bool demoOverlapSphere;
    public float overlapRadius = 3f;
    public LayerMask overlapMask = ~0;

    [Header("=== 22) AABB vs OBB so sánh nhanh ===")]
    public bool demoAABBvsOBB;
    public MeshRenderer aabbRenderer;

    [Header("=== 23) Ray marching (bước dò) minh hoạ ===")]
    public bool demoRayMarchSteps;
    public Vector3 rmTargetOffset = new Vector3(5, 0, 0);
    public int rmSteps = 12;

    [Header("=== 24) Poisson Disk Sampling (đơn giản) ===")]
    public bool demoPoissonDisk;
    public Vector2 poissonArea = new Vector2(10, 10);
    public float poissonMinDist = 1.0f;
    public int poissonTries = 200;

    [Header("=== 25) Spiral Path (Archimedean) ===")]
    public bool demoSpiral;
    public float spiralA = 0.1f;
    public float spiralB = 0.1f;
    public int spiralTurns = 8;
    public int spiralSeg = 200;

    [Header("=== 26) Fractal Tree (L-system đơn giản) ===")]
    public bool demoFractalTree;
    public int treeDepth = 5;
    public float treeLen = 2f;
    public float treeAngle = 25f;

    [Header("=== 27) Polygon + centroid + normals (2D phẳng) ===")]
    public bool demoPolygon2D;
    public Transform[] polygon2D; // đọc theo thứ tự
    public Vector3 polygonPlaneNormal = Vector3.up;
    public float edgeNormalLen = 0.5f;

    [Header("=== 28) SDF Box sample (heat/iso) ===")]
    public bool demoSDF;
    public Vector3 sdfBoxCenter = Vector3.zero;
    public Vector3 sdfBoxHalf = Vector3.one * 2f;
    public Vector2Int sdfRes = new Vector2Int(20, 20);

    [Header("=== 29) Hierarchy lines (parent->child) ===")]
    public bool demoHierarchyLines;
    public Color hierarchyColor = new Color(1, 1, 1, 0.6f);

    [Header("=== 30) Grid Flood-Fill minh hoạ ===")]
    public bool demoFloodFill;
    public Vector2Int ffSize = new Vector2Int(16, 10);
    public Vector2Int ffStart = new Vector2Int(3, 4);
    [Range(0, 1)] public float ffWallProbability = 0.15f;
    public int ffSeed = 1234;

    // ========================= OnDrawGizmos =========================
    void OnDrawGizmos()
    {
        if (!enabled) return;

        // 1) Local axes
        if (demoLocalAxes)
        {
            DrawAxes(transform.position, transform.rotation, axesLen);
        }

        // 2) FOV
        if (demoFOV)
        {
            DrawFOV(transform.position, transform.forward, transform.up, fovRadius, fovAngle, fovTargets, fovObstacleMask);
        }

        // 3) Bezier Quadratic
        if (demoBezierQuadratic && A && B && C)
        {
            DrawBezierQuadratic(A.position, B.position, C.position, bezierQ_Steps);
        }

        // 4) Bezier Cubic
        if (demoBezierCubic && A && B && C && D)
        {
            DrawBezierCubic(A.position, B.position, C.position, D.position, bezierC_Steps);
        }

        // 5) Catmull-Rom
        if (demoCatmullRom && waypoints != null && waypoints.Length >= 4)
        {
            DrawCatmullRom(waypoints, catmullStepsPerSeg);
        }

        // 6) Grid
        if (demoGrid)
        {
            DrawGrid(transform.position, gridSize, gridCell, gridPlane);
        }

        // 7) Arc
        if (demoArc)
        {
            DrawArcSector(transform.position, transform.forward, transform.up, arcRadius, arcAngle);
        }

        // 8) Projectile trajectory
        if (demoProjectile)
        {
            DrawProjectile(transform.position, transform.TransformDirection(launchVelocity), trajSteps, timeStep, trajHitMask);
        }

        // 9) Capsule wire
        if (demoCapsuleWire && A && B)
        {
            DrawWireCapsule(A.position, B.position, capsuleRadius, capsuleSegments);
        }

        // 10) Frustum
        if (demoFrustum)
        {
            DrawCameraFrustum(targetCamera ? targetCamera : Camera.main);
        }

        // 11) Fan rays
        if (demoFanRays)
        {
            DrawFanRays(transform.position, transform.forward, transform.up, rayLen, rayCount, rayFanAngle, rayMask);
        }

        // 12) Noise field
        if (demoNoiseField)
        {
            DrawNoiseVectorField(transform.position, fieldSize, fieldStep, noiseScale);
        }

        // 13) Hex grid
        if (demoHexGrid)
        {
            DrawHexGrid(transform.position, hexQ, hexR, hexSize);
        }

        // 14) Frenet frames
        if (demoFrenetFrames && waypoints != null && waypoints.Length >= 4)
        {
            DrawCatmullFrenetFrames(waypoints, catmullStepsPerSeg, frameLen);
        }

        // 15) LOS between waypoints
        if (demoLOSBetweenWaypoints && waypoints != null && waypoints.Length >= 2)
        {
            DrawWaypointsLOS(waypoints, losMask);
        }

        // 16) Children bounds
        if (demoChildrenBounds)
        {
            DrawChildrenBounds(transform);
        }

        // 17) OBB from BoxCollider
        if (demoOBBFromBoxCollider && boxCollider)
        {
            DrawOBB(boxCollider);
        }

        // 18) Mesh normals
        if (demoMeshNormals && meshFilter && meshFilter.sharedMesh)
        {
            DrawMeshNormals(meshFilter, normalLength, maxNormals);
        }

        // 19) Mesh triangles wire
        if (demoMeshTrianglesWire && meshFilter && meshFilter.sharedMesh)
        {
            DrawMeshTriangles(meshFilter);
        }

        // 20) NavMesh triangulation
        if (demoNavMesh)
        {
            DrawNavMeshTriangulation();
        }

        // 21) OverlapSphere
        if (demoOverlapSphere)
        {
            DrawOverlapSphere(transform.position, overlapRadius, overlapMask);
        }

        // 22) AABB vs OBB
        if (demoAABBvsOBB && aabbRenderer)
        {
            DrawAABBvsOBB(aabbRenderer);
        }

        // 23) Ray marching steps (minh hoạ)
        if (demoRayMarchSteps)
        {
            DrawRayMarchSteps(transform.position, transform.position + rmTargetOffset, rmSteps);
        }

        // 24) Poisson disk
        if (demoPoissonDisk)
        {
            DrawPoissonDisk(transform.position, poissonArea, poissonMinDist, poissonTries);
        }

        // 25) Spiral
        if (demoSpiral)
        {
            DrawSpiral(transform.position, spiralA, spiralB, spiralTurns, spiralSeg);
        }

        // 26) Fractal tree (đệ quy đơn giản)
        if (demoFractalTree)
        {
            Gizmos.color = colPrimary;
            DrawFractalTree(transform.position, transform.up, treeLen, treeDepth, treeAngle);
        }

        // 27) Polygon + centroid + edge normals
        if (demoPolygon2D && polygon2D != null && polygon2D.Length >= 3)
        {
            DrawPolygon2D(polygon2D, polygonPlaneNormal, edgeNormalLen);
        }

        // 28) SDF Box sample
        if (demoSDF)
        {
            DrawSDFBoxSamples(transform.position, sdfBoxCenter, sdfBoxHalf, sdfRes);
        }

        // 29) Hierarchy lines
        if (demoHierarchyLines)
        {
            Gizmos.color = hierarchyColor;
            DrawHierarchy(transform);
        }

        // 30) Flood-fill (lưới ngẫu nhiên)
        if (demoFloodFill)
        {
            DrawFloodFill(transform.position, ffSize, ffStart, ffWallProbability, ffSeed);
        }
    }

    // ========================= Utility RENDERERS =========================

    // 1) Axes triad
    void DrawAxes(Vector3 pos, Quaternion rot, float len)
    {
        Matrix4x4 m = Matrix4x4.TRS(pos, rot, Vector3.one);
        Gizmos.matrix = m;

        Gizmos.color = Color.red; DrawArrow(Vector3.zero, Vector3.right * len);
        Gizmos.color = Color.green; DrawArrow(Vector3.zero, Vector3.up * len);
        Gizmos.color = Color.blue; DrawArrow(Vector3.zero, Vector3.forward * len);

        Gizmos.matrix = Matrix4x4.identity;
    }

    // Simple arrow line with head
    void DrawArrow(Vector3 from, Vector3 to, float headLen = 0.25f, float headAngle = 18f)
    {
        Gizmos.DrawLine(from, to);
        Vector3 dir = (to - from);
        if (dir.sqrMagnitude < 1e-6f) return;
        dir.Normalize();
        Vector3 right = Quaternion.LookRotation(dir) * Quaternion.Euler(0, 180 + headAngle, 0) * Vector3.forward;
        Vector3 left = Quaternion.LookRotation(dir) * Quaternion.Euler(0, 180 - headAngle, 0) * Vector3.forward;
        Gizmos.DrawLine(to, to + right * headLen);
        Gizmos.DrawLine(to, to + left * headLen);
    }

    // 2) FOV
    void DrawFOV(Vector3 pos, Vector3 forward, Vector3 up, float radius, float angle, Transform[] targets, LayerMask obstacleMask)
    {
        Gizmos.color = new Color(colPrimary.r, colPrimary.g, colPrimary.b, 0.2f);
        DrawArcSector(pos, forward, up, radius, angle);

        // rays
        Gizmos.color = colPrimary;
        int steps = 32;
        for (int i = 0; i <= steps; i++)
        {
            float t = Mathf.Lerp(-angle * 0.5f, angle * 0.5f, i / (float)steps);
            Vector3 dir = Quaternion.AngleAxis(t, up) * forward;
            if (Physics.Raycast(pos, dir, out var hit, radius, obstacleMask))
            {
                Gizmos.color = colWarn; Gizmos.DrawLine(pos, hit.point);
            }
            else
            {
                Gizmos.color = colOK; Gizmos.DrawLine(pos, pos + dir * radius);
            }
        }

        // targets
        if (targets != null)
        {
            foreach (var t in targets)
            {
                if (!t) continue;
                Vector3 toT = t.position - pos;
                float dist = toT.magnitude;
                float ang = Vector3.Angle(forward, toT);
                bool inCone = dist <= radius && ang <= angle * 0.5f;
                Gizmos.color = inCone ? colOK : colWarn;
                Gizmos.DrawWireSphere(t.position, 0.15f);
            }
        }
    }

    // 3) Bezier Quadratic
    void DrawBezierQuadratic(Vector3 a, Vector3 b, Vector3 c, int steps)
    {
        Vector3 prev = a;
        Gizmos.color = colPrimary;
        for (int i = 1; i <= steps; i++)
        {
            float t = i / (float)steps;
            Vector3 p = BezierQuad(a, b, c, t);
            Gizmos.DrawLine(prev, p);
            prev = p;
        }
        Gizmos.color = colAlt; Gizmos.DrawWireSphere(a, 0.07f); Gizmos.DrawWireSphere(b, 0.07f); Gizmos.DrawWireSphere(c, 0.07f);
    }
    Vector3 BezierQuad(Vector3 a, Vector3 b, Vector3 c, float t)
    {
        float u = 1 - t;
        return u * u * a + 2 * u * t * b + t * t * c;
    }

    // 4) Bezier Cubic
    void DrawBezierCubic(Vector3 a, Vector3 b, Vector3 c, Vector3 d, int steps)
    {
        Vector3 prev = a;
        Gizmos.color = colPrimary;
        for (int i = 1; i <= steps; i++)
        {
            float t = i / (float)steps;
            Vector3 p = BezierCubic(a, b, c, d, t);
            Gizmos.DrawLine(prev, p);
            prev = p;
        }
        Gizmos.color = colAlt;
        Gizmos.DrawWireSphere(a, 0.07f); Gizmos.DrawWireSphere(b, 0.07f);
        Gizmos.DrawWireSphere(c, 0.07f); Gizmos.DrawWireSphere(d, 0.07f);
    }
    Vector3 BezierCubic(Vector3 a, Vector3 b, Vector3 c, Vector3 d, float t)
    {
        float u = 1 - t;
        return u * u * u * a + 3 * u * u * t * b + 3 * u * t * t * c + t * t * t * d;
    }

    // 5) Catmull-Rom
    void DrawCatmullRom(Transform[] pts, int stepsPerSeg)
    {
        if (pts.Length < 4) return;
        Gizmos.color = colPrimary;
        for (int i = 0; i < pts.Length - 3; i++)
        {
            Vector3 prev = CatmullRom(pts[i].position, pts[i + 1].position, pts[i + 2].position, pts[i + 3].position, 0f);
            for (int s = 1; s <= stepsPerSeg; s++)
            {
                float t = s / (float)stepsPerSeg;
                Vector3 p = CatmullRom(pts[i].position, pts[i + 1].position, pts[i + 2].position, pts[i + 3].position, t);
                Gizmos.DrawLine(prev, p);
                prev = p;
            }
        }
        Gizmos.color = colAlt;
        foreach (var t in pts) if (t) Gizmos.DrawWireSphere(t.position, 0.06f);
    }
    Vector3 CatmullRom(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        // Catmull-Rom với tension 0.5
        float t2 = t * t, t3 = t2 * t;
        return 0.5f * ((2f * p1) +
                       (-p0 + p2) * t +
                       (2f * p0 - 5f * p1 + 4f * p2 - p3) * t2 +
                       (-p0 + 3f * p1 - 3f * p2 + p3) * t3);
    }

    // 6) Grid
    void DrawGrid(Vector3 origin, Vector2 size, float cell, GridPlane plane)
    {
        Gizmos.color = new Color(colPrimary.r, colPrimary.g, colPrimary.b, 0.5f);
        int nx = Mathf.Max(1, Mathf.RoundToInt(size.x / cell));
        int ny = Mathf.Max(1, Mathf.RoundToInt(size.y / cell));
        Vector3 right, up;
        switch (plane)
        {
            case GridPlane.XY: right = Vector3.right; up = Vector3.up; break;
            case GridPlane.XZ: right = Vector3.right; up = Vector3.forward; break;
            default: right = Vector3.forward; up = Vector3.up; break;
        }
        Vector3 start = origin - (right * size.x * 0.5f) - (up * size.y * 0.5f);
        for (int x = 0; x <= nx; x++)
        {
            Vector3 a = start + right * (x * cell);
            Vector3 b = a + up * (ny * cell);
            Gizmos.DrawLine(a, b);
        }
        for (int y = 0; y <= ny; y++)
        {
            Vector3 a = start + up * (y * cell);
            Vector3 b = a + right * (nx * cell);
            Gizmos.DrawLine(a, b);
        }
    }

    // 7) Arc/Sector
    void DrawArcSector(Vector3 pos, Vector3 forward, Vector3 up, float radius, float angle)
    {
        int seg = 60;
        float half = angle * 0.5f;
        Vector3 left = Quaternion.AngleAxis(-half, up) * forward;
        Gizmos.DrawLine(pos, pos + left * radius);
        Vector3 prev = left;
        for (int i = 1; i <= seg; i++)
        {
            float t = Mathf.Lerp(-half, half, i / (float)seg);
            Vector3 dir = Quaternion.AngleAxis(t, up) * forward;
            Gizmos.DrawLine(pos + prev * radius, pos + dir * radius);
            prev = dir;
        }
        Vector3 right = Quaternion.AngleAxis(half, up) * forward;
        Gizmos.DrawLine(pos, pos + right * radius);
    }

    // 8) Projectile trajectory
    void DrawProjectile(Vector3 start, Vector3 v0, int steps, float dt, LayerMask mask)
    {
        Vector3 g = Physics.gravity;
        Vector3 prev = start;
        Gizmos.color = colPrimary;
        for (int i = 1; i <= steps; i++)
        {
            float t = i * dt;
            Vector3 p = start + v0 * t + 0.5f * g * t * t;

            if (Physics.Linecast(prev, p, out var hit, mask))
            {
                Gizmos.color = colWarn;
                Gizmos.DrawLine(prev, hit.point);
                Gizmos.DrawWireSphere(hit.point, 0.15f);
                break;
            }

            Gizmos.DrawLine(prev, p);
            prev = p;
        }
        Gizmos.color = colAlt;
        Gizmos.DrawWireSphere(start, 0.08f);
    }

    // 9) Wire Capsule A-B
    void DrawWireCapsule(Vector3 a, Vector3 b, float r, int segments = 16)
    {
        Vector3 axis = (b - a);
        float len = axis.magnitude;
        if (len < 1e-4f) { Gizmos.DrawWireSphere(a, r); return; }
        Vector3 dir = axis / len;

        // Orthonormal basis (u,v) ⟂ dir
        Vector3 u = Vector3.Cross(dir, Vector3.up);
        if (u.sqrMagnitude < 1e-6f) u = Vector3.Cross(dir, Vector3.right);
        u.Normalize();
        Vector3 v = Vector3.Cross(dir, u);

        Gizmos.color = colPrimary;

        // Rings
        for (int i = 0; i < segments; i++)
        {
            float a0 = (i / (float)segments) * Mathf.PI * 2;
            float a1 = ((i + 1) / (float)segments) * Mathf.PI * 2;
            Vector3 off0 = (Mathf.Cos(a0) * u + Mathf.Sin(a0) * v) * r;
            Vector3 off1 = (Mathf.Cos(a1) * u + Mathf.Sin(a1) * v) * r;

            Gizmos.DrawLine(a + off0, a + off1);
            Gizmos.DrawLine(b + off0, b + off1);
            Gizmos.DrawLine(a + off0, b + off0);
        }

        // Hemispheres (approx bằng các vòng quay 90°)
        int hemiSteps = Mathf.Max(4, segments / 4);
        for (int j = 0; j <= hemiSteps; j++)
        {
            float t = j / (float)hemiSteps * 90f;
            Quaternion q = Quaternion.AngleAxis(t, u);
            Quaternion q2 = Quaternion.AngleAxis(t, v);
            DrawCircle(a, q * dir, r);
            DrawCircle(b, q * dir, r);
            DrawCircle(a, q2 * dir, r);
            DrawCircle(b, q2 * dir, r);
        }
    }

    void DrawCircle(Vector3 center, Vector3 normal, float r, int seg = 40)
    {
        if (normal.sqrMagnitude < 1e-6f) normal = Vector3.up;
        normal.Normalize();
        Vector3 u = Vector3.Cross(normal, Vector3.up);
        if (u.sqrMagnitude < 1e-6f) u = Vector3.Cross(normal, Vector3.right);
        u.Normalize();
        Vector3 v = Vector3.Cross(normal, u);

        Vector3 prev = center + u * r;
        for (int i = 1; i <= seg; i++)
        {
            float ang = (i / (float)seg) * Mathf.PI * 2f;
            Vector3 p = center + (Mathf.Cos(ang) * u + Mathf.Sin(ang) * v) * r;
            Gizmos.DrawLine(prev, p);
            prev = p;
        }
    }

    // 10) Camera frustum
    void DrawCameraFrustum(Camera cam)
    {
        if (!cam) return;
        Gizmos.color = colPrimary;
        Matrix4x4 temp = Gizmos.matrix;
        Gizmos.matrix = cam.transform.localToWorldMatrix;
        Gizmos.DrawFrustum(Vector3.zero, cam.fieldOfView, cam.farClipPlane, cam.nearClipPlane, cam.aspect);
        Gizmos.matrix = temp;
    }

    // 11) Fan Rays
    void DrawFanRays(Vector3 pos, Vector3 forward, Vector3 up, float length, int count, float angle, LayerMask mask)
    {
        for (int i = 0; i < count; i++)
        {
            float t = (i / Mathf.Max(1f, count - 1f)) - 0.5f;
            float a = t * angle;
            Vector3 dir = Quaternion.AngleAxis(a, up) * forward;
            if (Physics.Raycast(pos, dir, out var hit, length, mask))
            {
                Gizmos.color = colWarn; Gizmos.DrawLine(pos, hit.point);
            }
            else
            {
                Gizmos.color = colOK; Gizmos.DrawLine(pos, pos + dir * length);
            }
        }
        Gizmos.color = colAlt; Gizmos.DrawWireSphere(pos, 0.06f);
    }

    // 12) Noise vector field
    void DrawNoiseVectorField(Vector3 origin, Vector2 size, float step, float scale)
    {
        int nx = Mathf.Max(1, Mathf.RoundToInt(size.x / step));
        int ny = Mathf.Max(1, Mathf.RoundToInt(size.y / step));
        Gizmos.color = colPrimary;
        for (int y = 0; y <= ny; y++)
        {
            for (int x = 0; x <= nx; x++)
            {
                Vector3 p = origin + new Vector3((x - nx * 0.5f) * step, 0, (y - ny * 0.5f) * step);
                float n = Mathf.PerlinNoise(p.x * scale + 100f, p.z * scale + 200f);
                float ang = n * Mathf.PI * 2f;
                Vector3 dir = new Vector3(Mathf.Cos(ang), 0, Mathf.Sin(ang));
                DrawArrow(p, p + dir * (step * 0.4f), headLen: step * 0.15f);
            }
        }
    }

    // 13) Hex grid axial (q,r)
    void DrawHexGrid(Vector3 center, int qCount, int rCount, float size)
    {
        float w = size * 2f;
        float h = Mathf.Sqrt(3f) * size;
        Vector3 origin = center - new Vector3(qCount * 0.5f * 0.75f * w, 0, rCount * 0.5f * h);

        for (int r = 0; r < rCount; r++)
        {
            for (int q = 0; q < qCount; q++)
            {
                float x = (q * 0.75f) * w;
                float z = (r * h) + ((q % 2 == 0) ? 0f : h * 0.5f);
                Vector3 c = origin + new Vector3(x, 0, z);
                Gizmos.color = colPrimary;
                DrawHex(c, size);
            }
        }
    }

    void DrawHex(Vector3 c, float size)
    {
        Vector3[] v = new Vector3[6];
        for (int i = 0; i < 6; i++)
        {
            float a = Mathf.Deg2Rad * (60f * i + 30f);
            v[i] = c + new Vector3(Mathf.Cos(a) * size, 0, Mathf.Sin(a) * size);
        }
        for (int i = 0; i < 6; i++) Gizmos.DrawLine(v[i], v[(i + 1) % 6]);
    }

    // 14) Frenet frames dọc Catmull
    void DrawCatmullFrenetFrames(Transform[] pts, int stepsPerSeg, float len)
    {
        if (pts.Length < 4) return;
        for (int i = 0; i < pts.Length - 3; i++)
        {
            for (int s = 0; s <= stepsPerSeg; s++)
            {
                float t = s / (float)stepsPerSeg;
                Vector3 p = CatmullRom(pts[i].position, pts[i + 1].position, pts[i + 2].position, pts[i + 3].position, t);
                // Tangent
                float dt = 0.01f;
                Vector3 p2 = CatmullRom(pts[i].position, pts[i + 1].position, pts[i + 2].position, pts[i + 3].position, Mathf.Clamp01(t + dt));
                Vector3 tangent = (p2 - p).normalized;
                Vector3 normal = Vector3.Cross(Vector3.up, tangent).normalized; // simple approx
                Vector3 binorm = Vector3.Cross(tangent, normal);

                Gizmos.color = Color.red; Gizmos.DrawLine(p, p + tangent * len);
                Gizmos.color = Color.green; Gizmos.DrawLine(p, p + normal * len);
                Gizmos.color = Color.blue; Gizmos.DrawLine(p, p + binorm * len);
            }
        }
    }

    // 15) LOS giữa các waypoint
    void DrawWaypointsLOS(Transform[] pts, LayerMask mask)
    {
        for (int i = 0; i < pts.Length - 1; i++)
        {
            var a = pts[i].position; var b = pts[i + 1].position;
            if (Physics.Linecast(a, b, mask)) { Gizmos.color = colWarn; }
            else { Gizmos.color = colOK; }
            Gizmos.DrawLine(a, b);
        }
    }

    // 16) Children bounds
    void DrawChildrenBounds(Transform root)
    {
        Bounds? total = null;
        var rends = root.GetComponentsInChildren<Renderer>();
        Gizmos.color = colPrimary;
        foreach (var r in rends)
        {
            Bounds b = r.bounds;
            DrawBounds(b, colAlt);
            total = total.HasValue ? Encaps(total.Value, b) : b;
        }
        if (total.HasValue) DrawBounds(total.Value, colPrimary);
    }
    Bounds Encaps(Bounds a, Bounds b) { a.Encapsulate(b.min); a.Encapsulate(b.max); return a; }
    void DrawBounds(Bounds b, Color c)
    {
        Gizmos.color = c;
        Gizmos.DrawWireCube(b.center, b.size);
    }

    // 17) OBB từ BoxCollider
    void DrawOBB(BoxCollider bc)
    {
        Matrix4x4 m = bc.transform.localToWorldMatrix;
        Matrix4x4 old = Gizmos.matrix;
        Gizmos.matrix = m;
        Gizmos.color = colPrimary;
        Gizmos.DrawWireCube(bc.center, bc.size);
        Gizmos.matrix = old;
    }

    // 18) Mesh normals
    void DrawMeshNormals(MeshFilter mf, float len, int limit)
    {
        var mesh = mf.sharedMesh;
        var verts = mesh.vertices;
        var norms = mesh.normals;
        Matrix4x4 m = mf.transform.localToWorldMatrix;
        Gizmos.color = colPrimary;
        int count = Mathf.Min(limit, Mathf.Min(verts.Length, norms.Length));
        for (int i = 0; i < count; i++)
        {
            Vector3 p = m.MultiplyPoint3x4(verts[i]);
            Vector3 n = m.MultiplyVector(norms[i]).normalized;
            Gizmos.DrawLine(p, p + n * len);
        }
    }

    // 19) Mesh triangles wire
    void DrawMeshTriangles(MeshFilter mf)
    {
        var mesh = mf.sharedMesh;
        var verts = mesh.vertices;
        var tris = mesh.triangles;
        Matrix4x4 m = mf.transform.localToWorldMatrix;
        Gizmos.color = colAlt;
        for (int i = 0; i < tris.Length; i += 3)
        {
            Vector3 a = m.MultiplyPoint3x4(verts[tris[i]]);
            Vector3 b = m.MultiplyPoint3x4(verts[tris[i + 1]]);
            Vector3 c = m.MultiplyPoint3x4(verts[tris[i + 2]]);
            Gizmos.DrawLine(a, b); Gizmos.DrawLine(b, c); Gizmos.DrawLine(c, a);
        }
    }

    // 20) NavMesh triangulation
    void DrawNavMeshTriangulation()
    {
        var tri = NavMesh.CalculateTriangulation();
        Gizmos.color = colPrimary;
        for (int i = 0; i < tri.indices.Length; i += 3)
        {
            Vector3 a = tri.vertices[tri.indices[i]];
            Vector3 b = tri.vertices[tri.indices[i + 1]];
            Vector3 c = tri.vertices[tri.indices[i + 2]];
            Gizmos.DrawLine(a, b); Gizmos.DrawLine(b, c); Gizmos.DrawLine(c, a);
        }
    }

    // 21) OverlapSphere
    void DrawOverlapSphere(Vector3 center, float radius, LayerMask mask)
    {
        Gizmos.color = colPrimary; Gizmos.DrawWireSphere(center, radius);
        var cols = Physics.OverlapSphere(center, radius, mask, QueryTriggerInteraction.Ignore);
        foreach (var c in cols)
        {
            Gizmos.color = colOK;
            Gizmos.DrawLine(center, c.bounds.center);
            Gizmos.DrawWireCube(c.bounds.center, c.bounds.size * 0.2f);
        }
    }

    // 22) AABB vs OBB
    void DrawAABBvsOBB(MeshRenderer mr)
    {
        // AABB (world)
        Gizmos.color = colPrimary;
        Gizmos.DrawWireCube(mr.bounds.center, mr.bounds.size);

        // OBB (dựa theo transform local của object)
        var mf = mr.GetComponent<MeshFilter>();
        if (mf && mf.sharedMesh)
        {
            Matrix4x4 m = mr.transform.localToWorldMatrix;
            Gizmos.color = colAlt;
            Gizmos.matrix = m;
            Gizmos.DrawWireCube(mf.sharedMesh.bounds.center, mf.sharedMesh.bounds.size);
            Gizmos.matrix = Matrix4x4.identity;
        }
    }

    // 23) Ray-march steps (minh hoạ không phải real SDF)
    void DrawRayMarchSteps(Vector3 start, Vector3 end, int steps)
    {
        Gizmos.color = colPrimary;
        Vector3 prev = start;
        for (int i = 1; i <= steps; i++)
        {
            float t = i / (float)steps;
            Vector3 p = Vector3.Lerp(start, end, t);
            Gizmos.DrawLine(prev, p);
            prev = p;
        }
        Gizmos.color = colAlt; Gizmos.DrawWireSphere(start, 0.1f); Gizmos.DrawWireSphere(end, 0.1f);
    }

    // 24) Poisson disk (dart-throw)
    void DrawPoissonDisk(Vector3 origin, Vector2 area, float minDist, int tries)
    {
        var points = new List<Vector2>();
        var rand = new System.Random(12345 + tries);
        float minSqr = minDist * minDist;

        for (int i = 0; i < tries; i++)
        {
            float x = (float)rand.NextDouble() * area.x - area.x * 0.5f;
            float y = (float)rand.NextDouble() * area.y - area.y * 0.5f;
            Vector2 p = new Vector2(x, y);
            bool ok = true;
            for (int j = 0; j < points.Count; j++)
            {
                if ((points[j] - p).sqrMagnitude < minSqr) { ok = false; break; }
            }
            if (ok) points.Add(p);
        }

        Gizmos.color = colPrimary;
        foreach (var p in points)
        {
            Vector3 wp = origin + new Vector3(p.x, 0, p.y);
            Gizmos.DrawWireSphere(wp, minDist * 0.5f);
        }

        // Khung
        Gizmos.color = colAlt;
        Vector3 c = origin;
        Vector3 sz = new Vector3(area.x, 0, area.y);
        Vector3 a = c + new Vector3(-sz.x / 2, 0, -sz.z / 2);
        Vector3 b = c + new Vector3(sz.x / 2, 0, -sz.z / 2);
        Vector3 d = c + new Vector3(sz.x / 2, 0, sz.z / 2);
        Vector3 e = c + new Vector3(-sz.x / 2, 0, sz.z / 2);
        Gizmos.DrawLine(a, b); Gizmos.DrawLine(b, d); Gizmos.DrawLine(d, e); Gizmos.DrawLine(e, a);
    }

    // 25) Spiral
    void DrawSpiral(Vector3 c, float A, float B, int turns, int seg)
    {
        Gizmos.color = colPrimary;
        Vector3 prev = c;
        float maxTheta = turns * Mathf.PI * 2f;
        for (int i = 1; i <= seg; i++)
        {
            float t = i / (float)seg;
            float th = t * maxTheta;
            float r = A + B * th;
            Vector3 p = c + new Vector3(Mathf.Cos(th) * r, 0, Mathf.Sin(th) * r);
            Gizmos.DrawLine(prev, p);
            prev = p;
        }
    }

    // 26) Fractal tree
    void DrawFractalTree(Vector3 start, Vector3 dir, float len, int depth, float ang)
    {
        if (depth <= 0 || len <= 0.02f) return;
        Vector3 end = start + dir * len;
        Gizmos.DrawLine(start, end);

        Quaternion q1 = Quaternion.AngleAxis(ang, Vector3.forward);
        Quaternion q2 = Quaternion.AngleAxis(-ang, Vector3.forward);

        DrawFractalTree(end, q1 * dir, len * 0.7f, depth - 1, ang);
        DrawFractalTree(end, q2 * dir, len * 0.7f, depth - 1, ang);
    }

    // 27) Polygon 2D + centroid + edge normals
    void DrawPolygon2D(Transform[] verts, Vector3 planeNormal, float nLen)
    {
        int n = verts.Length;
        if (n < 3) return;
        Vector3 centroid = Vector3.zero;
        for (int i = 0; i < n; i++) centroid += verts[i].position;
        centroid /= n;

        Gizmos.color = colPrimary;
        for (int i = 0; i < n; i++)
        {
            Vector3 a = verts[i].position;
            Vector3 b = verts[(i + 1) % n].position;
            Gizmos.DrawLine(a, b);

            // Normals cạnh
            Vector3 edge = (b - a);
            Vector3 nrm = Vector3.Cross(planeNormal.normalized, edge.normalized);
            Vector3 mid = (a + b) * 0.5f;
            Gizmos.color = colAlt;
            Gizmos.DrawLine(mid, mid + nrm * nLen);
            Gizmos.color = colPrimary;
        }

        Gizmos.color = colOK;
        Gizmos.DrawWireSphere(centroid, 0.08f);
    }

    // 28) SDF Box sampling trên lưới XZ
    void DrawSDFBoxSamples(Vector3 origin, Vector3 center, Vector3 half, Vector2Int res)
    {
        int nx = Mathf.Max(2, res.x);
        int ny = Mathf.Max(2, res.y);
        Vector3 o = origin + new Vector3(-nx * 0.5f, 0, -ny * 0.5f);

        for (int y = 0; y < ny; y++)
        {
            for (int x = 0; x < nx; x++)
            {
                Vector3 p = o + new Vector3(x, 0, y);
                float d = SDF_Box(p - center, half);
                // màu theo khoảng cách
                Color c = Color.Lerp(colOK, colWarn, Mathf.InverseLerp(0f, 2f, Mathf.Abs(d)));
                c.a = 0.8f;
                Gizmos.color = c;
                float s = Mathf.Clamp(1f - Mathf.Abs(d) * 0.5f, 0.2f, 1f);
                Gizmos.DrawCube(p, new Vector3(s, 0.02f, s));
            }
        }

        // Vẽ khối box
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(center, half * 2f);
    }
    float SDF_Box(Vector3 p, Vector3 b)
    {
        Vector3 q = new Vector3(Mathf.Abs(p.x), Mathf.Abs(p.y), Mathf.Abs(p.z)) - b;
        return Mathf.Max(Mathf.Max(q.x, q.y), q.z);
    }

    // 29) Hierarchy lines
    void DrawHierarchy(Transform t)
    {
        for (int i = 0; i < t.childCount; i++)
        {
            Transform c = t.GetChild(i);
            Gizmos.DrawLine(t.position, c.position);
            DrawHierarchy(c);
        }
    }

    // 30) Flood-fill minh hoạ
    void DrawFloodFill(Vector3 origin, Vector2Int size, Vector2Int start, float wallProb, int seed)
    {
        int W = Mathf.Max(3, size.x);
        int H = Mathf.Max(3, size.y);
        bool[,] wall = new bool[W, H];
        var rnd = new System.Random(seed);
        for (int y = 0; y < H; y++) for (int x = 0; x < W; x++) wall[x, y] = rnd.NextDouble() < wallProb;

        // mở điểm start
        start.x = Mathf.Clamp(start.x, 0, W - 1);
        start.y = Mathf.Clamp(start.y, 0, H - 1);
        wall[start.x, start.y] = false;

        // flood
        bool[,] vis = new bool[W, H];
        var q = new Queue<Vector2Int>();
        q.Enqueue(start);
        vis[start.x, start.y] = true;
        int[] dx = { 1, -1, 0, 0 };
        int[] dy = { 0, 0, 1, -1 };
        while (q.Count > 0)
        {
            var u = q.Dequeue();
            for (int k = 0; k < 4; k++)
            {
                int nx = u.x + dx[k], ny = u.y + dy[k];
                if (nx < 0 || ny < 0 || nx >= W || ny >= H) continue;
                if (wall[nx, ny] || vis[nx, ny]) continue;
                vis[nx, ny] = true; q.Enqueue(new Vector2Int(nx, ny));
            }
        }

        // vẽ
        float cell = 0.6f;
        Vector3 basePos = origin - new Vector3(W * cell * 0.5f, 0, H * cell * 0.5f);
        for (int y = 0; y < H; y++) for (int x = 0; x < W; x++)
            {
                Vector3 p = basePos + new Vector3(x * cell, 0, y * cell);
                if (wall[x, y]) { Gizmos.color = colWarn; Gizmos.DrawCube(p, new Vector3(cell * 0.9f, 0.02f, cell * 0.9f)); }
                else if (vis[x, y]) { Gizmos.color = colOK; Gizmos.DrawCube(p, new Vector3(cell * 0.9f, 0.02f, cell * 0.9f)); }
                else { Gizmos.color = new Color(0.5f, 0.5f, 0.5f, 0.4f); Gizmos.DrawCube(p, new Vector3(cell * 0.9f, 0.01f, cell * 0.9f)); }
            }

        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(origin, new Vector3(W * cell, 0, H * cell));
    }
}
