// GizmosAdvancedDemos_Extra.cs
// 20 demo Gizmos nâng cao #31..#50
// Unity 2020+ (khuyên 2021/2022 LTS)
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class GizmosAdvancedDemos_Extra : MonoBehaviour
{
    [Header("=== Common Settings ===")]
    public Color colPrimary = new Color(0.2f, 0.9f, 1f, 1f);
    public Color colAlt = new Color(1f, 0.6f, 0.2f, 1f);
    public Color colWarn = new Color(1f, 0.2f, 0.2f, 1f);
    public Color colOK = new Color(0.2f, 1f, 0.3f, 1f);

    public Transform A, B, C, D;
    public Transform[] points;         // dùng cho nhiều demo
    public Transform target;           // target chung
    public LayerMask mask = ~0;

    // 31) Polar Grid (radar rings)
    [Header("31) Polar Grid (radar rings)")]
    public bool demoPolarGrid;
    public int polarRings = 6;
    public int polarRadials = 12;
    public float polarStep = 1f;

    // 32) Bouncing Path (phản xạ trên mặt va chạm)
    [Header("32) Bouncing Path (reflect)")]
    public bool demoBounce;
    public Vector3 bounceVelocity = new Vector3(6, 8, 0);
    public int bounceBounces = 4;
    public float bounceStepTime = 0.06f;

    // 33) Boids Neighborhood (debug lân cận)
    [Header("33) Boids Neighborhood")]
    public bool demoBoids;
    public Transform[] boids;
    public float boidNeighborRadius = 2.5f;
    public Vector3 boidForward = Vector3.forward;

    // 34) Steering: Seek/Arrive vectors
    [Header("34) Steering: Seek/Arrive")]
    public bool demoSteer;
    public float arriveSlowRadius = 4f;
    public float maxSpeed = 6f;

    // 35) Light Range/Spot Preview
    [Header("35) Light Preview")]
    public bool demoLightPreview;
    public Light[] lights;

    // 36) Audio Attenuation (min/max distance)
    [Header("36) Audio Attenuation")]
    public bool demoAudio;
    public AudioSource[] audioSources;

    // 37) Rope Catenary (xấp xỉ parabol)
    [Header("37) Rope Catenary")]
    public bool demoRope;
    public float sag = 0.5f; // độ võng 0..1
    public int ropeSegments = 32;

    // 38) Spring Oscillator (phase path)
    [Header("38) Spring Oscillator")]
    public bool demoSpring;
    public float k = 10f;      // spring
    public float cDamp = 1.2f; // damping
    public float mass = 1f;
    public float x0 = 1f;      // initial position
    public float v0 = 0f;      // initial velocity
    public int springSteps = 150;
    public float springDt = 0.02f;

    // 39) IK Chain preview (bone lines & target)
    [Header("39) IK Chain Preview")]
    public bool demoIK;
    public Transform[] bones;

    // 40) Convex Hull (2D quick gift-wrap)
    [Header("40) Convex Hull 2D (XZ plane)")]
    public bool demoHull;
    public Transform[] hullPoints;

    // 41) RRT sampling tree (trong box)
    [Header("41) RRT Sampling Tree")]
    public bool demoRRT;
    public Vector3 rrtCenter = Vector3.zero;
    public Vector3 rrtHalfExtents = new Vector3(8, 0, 8);
    public int rrtIters = 250;
    public float rrtStep = 0.8f;
    public int rrtSeed = 123;

    // 42) Octree bounds (chia đều)
    [Header("42) Octree Bounds (uniform)")]
    public bool demoOctree;
    public Vector3 octCenter = Vector3.zero;
    public Vector3 octHalf = new Vector3(5, 5, 5);
    public int octreeDepth = 2;

    // 43) BVH cho child renderers (tách theo trục dài nhất)
    [Header("43) BVH (child renderers)")]
    public bool demoBVH;
    public int bvhLeafSize = 2;

    // 44) Camera -> Object Occlusion test
    [Header("44) Occlusion: Camera->Object")]
    public bool demoOcclusion;
    public Camera cam;
    public Renderer targetRenderer;

    // 45) Field lines quanh điện tích điểm (2D)
    [Header("45) Field Lines (point charges)")]
    public bool demoField;
    [System.Serializable] public class Charge { public Transform t; public float q = 1f; }
    public Charge[] charges;
    public int fieldStreamCount = 24;
    public int fieldStreamSteps = 80;
    public float fieldStepLen = 0.15f;

    // 46) Heatmap IDW (grid XZ)
    [Header("46) Heatmap IDW (XZ)")]
    public bool demoHeatmap;
    public Vector2Int heatRes = new Vector2Int(24, 16);
    public Vector2 heatSize = new Vector2(12, 8);
    public float idwPower = 1.5f;

    // 47) Visibility Polygon (fan rays 360°)
    [Header("47) Visibility Polygon (360°)")]
    public bool demoVisPoly;
    public int visRayCount = 180;
    public float visRadius = 12f;

    // 48) Spline Lane Offset (song song đường dẫn)
    [Header("48) Spline Lane Offset")]
    public bool demoLane;
    public Transform[] lanePts;
    public float laneOffset = 1.5f;

    // 49) 3D Lattice (grid khối)
    [Header("49) 3D Lattice")]
    public bool demoLattice;
    public Vector3Int latCount = new Vector3Int(6, 4, 6);
    public Vector3 latStep = new Vector3(1, 1, 1);

    // 50) Motion Trails (ghi dấu vị trí)
    [Header("50) Motion Trails")]
    public bool demoTrails;
    public Transform[] trailTargets;
    public int trailLen = 64;

    // --- internal buffers ---
    Dictionary<Transform, Queue<Vector3>> trailBuffer = new Dictionary<Transform, Queue<Vector3>>();

    void Update()
    {
        if (!demoTrails || trailTargets == null) return;
        foreach (var t in trailTargets)
        {
            if (!t) continue;
            if (!trailBuffer.TryGetValue(t, out var q))
            {
                q = new Queue<Vector3>(trailLen);
                trailBuffer[t] = q;
            }
            if (q.Count >= trailLen) q.Dequeue();
            q.Enqueue(t.position);
        }
    }

    void OnDrawGizmos()
    {
        if (!enabled) return;

        if (demoPolarGrid) DrawPolarGrid(transform.position, polarRings, polarRadials, polarStep);

        if (demoBounce) DrawBouncePath(transform.position, transform.TransformDirection(bounceVelocity), bounceBounces, bounceStepTime);

        if (demoBoids && boids != null) DrawBoids(boids, boidNeighborRadius, boidForward);

        if (demoSteer && target) DrawSteering(transform.position, target.position, arriveSlowRadius, maxSpeed);

        if (demoLightPreview && lights != null) DrawLightPreview(lights);

        if (demoAudio && audioSources != null) DrawAudioAttenuation(audioSources);

        if (demoRope && A && B) DrawRopeCatenary(A.position, B.position, sag, ropeSegments);

        if (demoSpring) DrawSpringOscillator(x0, v0, k, cDamp, mass, springSteps, springDt);

        if (demoIK) DrawIKChain(bones, target);

        if (demoHull && hullPoints != null && hullPoints.Length >= 3) DrawConvexHullXZ(hullPoints);

        if (demoRRT) DrawRRT(rrtCenter + transform.position, rrtHalfExtents, rrtIters, rrtStep, rrtSeed);

        if (demoOctree) DrawOctree(octCenter + transform.position, octHalf, Mathf.Max(0, octreeDepth));

        if (demoBVH) DrawBVH(transform, bvhLeafSize);

        if (demoOcclusion && cam && targetRenderer) DrawOcclusion(cam, targetRenderer);

        if (demoField && charges != null) DrawFieldLines2D(transform.position, charges, fieldStreamCount, fieldStreamSteps, fieldStepLen);

        if (demoHeatmap) DrawHeatmapIDW(transform.position, heatRes, heatSize, points, idwPower);

        if (demoVisPoly) DrawVisibilityPolygon(transform.position, visRayCount, visRadius, mask);

        if (demoLane && lanePts != null && lanePts.Length >= 2) DrawLaneOffset(lanePts, laneOffset);

        if (demoLattice) DrawLattice(transform.position, latCount, latStep);

        if (demoTrails) DrawTrails(trailBuffer);
    }

    // ========== 31) Polar Grid ==========
    void DrawPolarGrid(Vector3 center, int rings, int radials, float step)
    {
        Gizmos.color = new Color(colPrimary.r, colPrimary.g, colPrimary.b, 0.6f);
        for (int r = 1; r <= rings; r++)
            DrawCircle(center, Vector3.up, r * step, 64);

        Gizmos.color = colAlt;
        for (int i = 0; i < radials; i++)
        {
            float a = (i / (float)radials) * Mathf.PI * 2f;
            Vector3 dir = new Vector3(Mathf.Cos(a), 0, Mathf.Sin(a));
            Gizmos.DrawLine(center, center + dir * rings * step);
        }
    }
    void DrawCircle(Vector3 center, Vector3 normal, float radius, int seg)
    {
        if (normal.sqrMagnitude < 1e-6f) normal = Vector3.up;
        normal.Normalize();
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

    // ========== 32) Bouncing Path ==========
    void DrawBouncePath(Vector3 start, Vector3 v0, int bounces, float dt)
    {
        Gizmos.color = colPrimary;
        Vector3 g = Physics.gravity;
        Vector3 p = start;
        Vector3 v = v0;
        for (int b = 0; b < bounces; b++)
        {
            Vector3 prev = p;
            for (int i = 0; i < 200; i++)
            {
                Vector3 nxt = p + v * dt + 0.5f * g * dt * dt;
                if (Physics.Linecast(p, nxt, out var hit, mask))
                {
                    Gizmos.color = colWarn;
                    Gizmos.DrawLine(p, hit.point);
                    Gizmos.DrawWireSphere(hit.point, 0.12f);
                    // reflect
                    v = Vector3.Reflect(v + g * dt, hit.normal) * 0.8f;
                    p = hit.point + hit.normal * 0.01f;
                    Gizmos.color = colPrimary;
                    break;
                }
                Gizmos.DrawLine(prev, nxt);
                prev = nxt;
                p = nxt;
                v += g * dt;
            }
        }
    }

    // ========== 33) Boids ==========
    void DrawBoids(Transform[] bs, float neighborR, Vector3 forwardLocal)
    {
        foreach (var b in bs)
        {
            if (!b) continue;
            Gizmos.color = new Color(colPrimary.r, colPrimary.g, colPrimary.b, 0.4f);
            Gizmos.DrawWireSphere(b.position, neighborR);
            Gizmos.color = colOK;
            Vector3 f = b.TransformDirection(forwardLocal).normalized;
            Gizmos.DrawLine(b.position, b.position + f * 1.2f);
            // neighbors
            foreach (var other in bs)
            {
                if (!other || other == b) continue;
                float d = Vector3.Distance(b.position, other.position);
                if (d <= neighborR) { Gizmos.color = colAlt; Gizmos.DrawLine(b.position, other.position); }
            }
        }
    }

    // ========== 34) Steering ==========
    void DrawSteering(Vector3 pos, Vector3 tgt, float slowRadius, float vmax)
    {
        Vector3 to = tgt - pos;
        float dist = to.magnitude;
        Vector3 desired = (dist > 1e-4f) ? (to / dist) * Mathf.Min(vmax, vmax * (dist / slowRadius)) : Vector3.zero;

        Gizmos.color = colPrimary; Gizmos.DrawLine(pos, tgt);
        Gizmos.color = colOK; Gizmos.DrawLine(pos, pos + desired);
        Gizmos.color = colAlt; Gizmos.DrawWireSphere(tgt, 0.2f);
        Gizmos.DrawWireSphere(tgt, slowRadius);
    }

    // ========== 35) Light Preview ==========
    // Thay thế hoàn toàn hàm DrawLightPreview hiện tại
    void DrawLightPreview(Light[] ls)
    {
        if (ls == null) return;

        foreach (var l in ls)
        {
            if (!l) continue;

            switch (l.type)
            {
                case LightType.Point:
                    {
                        Gizmos.color = colPrimary;
                        Gizmos.DrawWireSphere(l.transform.position, l.range);
                        break;
                    }

                case LightType.Spot:
                    {
                        Gizmos.color = colPrimary;
                        var old = Gizmos.matrix;
                        Gizmos.matrix = l.transform.localToWorldMatrix;
                        Gizmos.DrawFrustum(Vector3.zero, l.spotAngle, l.range, 0.05f, 1f);
                        Gizmos.matrix = old;
                        break;
                    }

#if UNITY_EDITOR
                // Area Light chỉ có trong Editor — bị strip ở Player
                case LightType.Area:
                    {
                        // Lấy kích thước theo đúng Editor API
                        // Cách 1: dùng property editor-only (nếu Unity giữ API)
                        Vector2 size;
#if UNITY_2021_2_OR_NEWER
                        size = l.areaSize; // Editor-only: hợp lệ trong Assembly-CSharp (Editor)
#else
                // Cách 2: fallback SerializedObject (đọc m_AreaSize)
                SerializedObject so = new SerializedObject(l);
                size = so.FindProperty("m_AreaSize").vector2Value;
#endif
                        // Vẽ khung chữ nhật theo transform của đèn
                        Gizmos.color = colAlt;
                        var old = Gizmos.matrix;
                        Gizmos.matrix = l.transform.localToWorldMatrix;
                        Gizmos.DrawWireCube(Vector3.zero, new Vector3(size.x, 0.01f, size.y));
                        Gizmos.matrix = old;
                        break;
                    }
#endif

                default:
                    // Directional/Unknown: bỏ qua hoặc tự chọn cách hiển thị
                    break;
            }
        }
    }

    // ========== 36) Audio Attenuation ==========
    void DrawAudioAttenuation(AudioSource[] srcs)
    {
        foreach (var a in srcs)
        {
            if (!a) continue;
            Gizmos.color = new Color(colPrimary.r, colPrimary.g, colPrimary.b, 0.5f);
            Gizmos.DrawWireSphere(a.transform.position, a.minDistance);
            Gizmos.color = new Color(colAlt.r, colAlt.g, colAlt.b, 0.5f);
            Gizmos.DrawWireSphere(a.transform.position, a.maxDistance);
        }
    }

    // ========== 37) Rope Catenary (parabol xấp xỉ) ==========
    void DrawRopeCatenary(Vector3 p0, Vector3 p1, float sag01, int seg)
    {
        sag01 = Mathf.Clamp01(sag01);
        Vector3 mid = (p0 + p1) * 0.5f;
        Vector3 down = Vector3.down * Vector3.Distance(p0, p1) * sag01;
        Vector3 p2 = mid + down; // điểm võng

        Gizmos.color = colPrimary;
        Vector3 prev = p0;
        for (int i = 1; i <= seg; i++)
        {
            float t = i / (float)seg;
            // quadratic Bezier p0 -> p2 -> p1
            Vector3 q = (1 - t) * (1 - t) * p0 + 2 * (1 - t) * t * p2 + t * t * p1;
            Gizmos.DrawLine(prev, q);
            prev = q;
        }
        Gizmos.color = colAlt; Gizmos.DrawWireSphere(p0, 0.05f); Gizmos.DrawWireSphere(p1, 0.05f); Gizmos.DrawWireSphere(p2, 0.05f);
    }

    // ========== 38) Spring Oscillator ==========
    void DrawSpringOscillator(float x0, float v0, float k, float c, float m, int steps, float dt)
    {
        float x = x0, v = v0;
        Vector3 origin = transform.position;
        Gizmos.color = colPrimary;
        Vector3 prev = origin + new Vector3(x, 0, 0);
        for (int i = 0; i < steps; i++)
        {
            // m*x'' + c*x' + k*x = 0  -> semi-implicit Euler
            float a = (-c * v - k * x) / m;
            v += a * dt;
            x += v * dt;
            Vector3 p = origin + new Vector3(x, 0, i * 0.02f);
            Gizmos.DrawLine(prev, p);
            prev = p;
        }
        Gizmos.color = colAlt; Gizmos.DrawLine(origin, origin + Vector3.forward * steps * 0.02f);
    }

    // ========== 39) IK Chain Preview ==========
    void DrawIKChain(Transform[] chain, Transform tgt)
    {
        if (chain == null || chain.Length == 0) return;
        Gizmos.color = colPrimary;
        for (int i = 0; i < chain.Length - 1; i++)
        {
            if (!chain[i] || !chain[i + 1]) continue;
            Gizmos.DrawLine(chain[i].position, chain[i + 1].position);
            Gizmos.DrawWireSphere(chain[i].position, 0.05f);
        }
        if (chain[chain.Length - 1]) Gizmos.DrawWireSphere(chain[chain.Length - 1].position, 0.06f);
        if (tgt) { Gizmos.color = colAlt; Gizmos.DrawWireSphere(tgt.position, 0.12f); Gizmos.DrawLine(chain[chain.Length - 1].position, tgt.position); }
    }

    // ========== 40) Convex Hull 2D (Gift Wrap trên XZ) ==========
    void DrawConvexHullXZ(Transform[] pts)
    {
        List<Vector3> P = new List<Vector3>();
        foreach (var t in pts) if (t) P.Add(t.position);
        if (P.Count < 3) return;

        // Tìm điểm thấp nhất (z nhỏ, nếu bằng thì x nhỏ)
        int start = 0;
        for (int i = 1; i < P.Count; i++)
        {
            if (P[i].z < P[start].z || (Mathf.Approximately(P[i].z, P[start].z) && P[i].x < P[start].x))
                start = i;
        }
        int p = start;
        List<int> hull = new List<int>();
        do
        {
            hull.Add(p);
            int q = (p + 1) % P.Count;
            for (int r = 0; r < P.Count; r++)
            {
                if (OrientationXZ(P[p], P[q], P[r]) < 0) q = r; // chọn trái hơn
            }
            p = q;
        } while (p != start);

        Gizmos.color = colPrimary;
        for (int i = 0; i < hull.Count; i++)
        {
            Vector3 a = P[hull[i]];
            Vector3 b = P[hull[(i + 1) % hull.Count]];
            Gizmos.DrawLine(a, b);
        }
        Gizmos.color = colAlt;
        foreach (var v in P) Gizmos.DrawWireSphere(v, 0.05f);
    }
    float OrientationXZ(Vector3 a, Vector3 b, Vector3 c)
    {
        Vector2 A = new Vector2(a.x, a.z);
        Vector2 B = new Vector2(b.x, b.z);
        Vector2 C = new Vector2(c.x, c.z);
        return (B.x - A.x) * (C.y - A.y) - (B.y - A.y) * (C.x - A.x);
    }

    // ========== 41) RRT ==========
    struct Node { public Vector3 p; public int parent; public Node(Vector3 p, int parent) { this.p = p; this.parent = parent; } }
    void DrawRRT(Vector3 center, Vector3 half, int iters, float step, int seed)
    {
        var rnd = new System.Random(seed);
        List<Node> nodes = new List<Node>();
        Vector3 start = center; nodes.Add(new Node(start, -1));

        for (int i = 0; i < iters; i++)
        {
            Vector3 rand = center + new Vector3(
                (float)(rnd.NextDouble() * 2 - 1) * half.x,
                (float)(rnd.NextDouble() * 2 - 1) * Mathf.Max(0.1f, half.y),
                (float)(rnd.NextDouble() * 2 - 1) * half.z
            );
            int best = 0; float bestD = float.MaxValue;
            for (int n = 0; n < nodes.Count; n++)
            {
                float d = (nodes[n].p - rand).sqrMagnitude;
                if (d < bestD) { bestD = d; best = n; }
            }
            Vector3 dir = (rand - nodes[best].p); float dist = dir.magnitude;
            if (dist < 1e-4f) continue; dir /= dist;
            Vector3 np = nodes[best].p + dir * Mathf.Min(step, dist);

            if (!Physics.Linecast(nodes[best].p, np, mask))
                nodes.Add(new Node(np, best));
        }

        Gizmos.color = new Color(colPrimary.r, colPrimary.g, colPrimary.b, 0.7f);
        for (int i = 1; i < nodes.Count; i++)
            Gizmos.DrawLine(nodes[i].p, nodes[nodes[i].parent].p);

        Gizmos.color = colAlt;
        Gizmos.DrawWireCube(center, half * 2f);
    }

    // ========== 42) Octree ==========
    void DrawOctree(Vector3 center, Vector3 half, int depth)
    {
        Gizmos.color = new Color(colPrimary.r, colPrimary.g, colPrimary.b, 0.6f);
        Gizmos.DrawWireCube(center, half * 2f);
        if (depth <= 0) return;
        Vector3 h2 = half * 0.5f;
        for (int x = -1; x <= 1; x += 2)
            for (int y = -1; y <= 1; y += 2)
                for (int z = -1; z <= 1; z += 2)
                {
                    Vector3 c = center + Vector3.Scale(new Vector3(x, y, z), h2);
                    DrawOctree(c, h2, depth - 1);
                }
    }

    // ========== 43) BVH ==========
    class BVHNode { public Bounds b; public BVHNode left, right; public List<Renderer> items; }
    void DrawBVH(Transform root, int leafSize)
    {
        var rends = root.GetComponentsInChildren<Renderer>();
        List<Renderer> list = new List<Renderer>(rends);
        BVHNode n = BuildBVH(list, leafSize);
        DrawBVHNode(n, 0);
    }
    BVHNode BuildBVH(List<Renderer> items, int leafSize)
    {
        if (items == null || items.Count == 0) return null;
        BVHNode node = new BVHNode { items = new List<Renderer>(items) };
        Bounds b = new Bounds(items[0].bounds.center, items[0].bounds.size);
        foreach (var r in items) { b.Encapsulate(r.bounds.min); b.Encapsulate(r.bounds.max); }
        node.b = b;

        if (items.Count <= Mathf.Max(1, leafSize)) return node;

        // split theo trục dài nhất
        Vector3 sz = b.size;
        int axis = (sz.x > sz.y && sz.x > sz.z) ? 0 : (sz.y > sz.z ? 1 : 2);
        items.Sort((a, c) => a.bounds.center[axis].CompareTo(c.bounds.center[axis]));
        int mid = items.Count / 2;
        var left = items.GetRange(0, mid);
        var right = items.GetRange(mid, items.Count - mid);
        node.left = BuildBVH(left, leafSize);
        node.right = BuildBVH(right, leafSize);
        return node;
    }
    void DrawBVHNode(BVHNode n, int depth)
    {
        if (n == null) return;
        Color col = Color.Lerp(colAlt, colPrimary, Mathf.InverseLerp(0, 6, depth));
        col.a = 0.5f;
        Gizmos.color = col;
        Gizmos.DrawWireCube(n.b.center, n.b.size);
        DrawBVHNode(n.left, depth + 1);
        DrawBVHNode(n.right, depth + 1);
    }

    // ========== 44) Occlusion ==========
    void DrawOcclusion(Camera c, Renderer r)
    {
        Bounds b = r.bounds;
        Vector3[] corners = GetBoundsCorners(b);
        bool blocked = false;
        foreach (var p in corners)
        {
            if (Physics.Linecast(c.transform.position, p, out var hit) && hit.collider && !IsPartOfRenderer(hit.collider, r))
            { blocked = true; break; }
        }
        Gizmos.color = blocked ? colWarn : colOK;
        Gizmos.DrawWireCube(b.center, b.size);
        foreach (var p in corners) Gizmos.DrawLine(c.transform.position, p);
    }
    bool IsPartOfRenderer(Collider col, Renderer r) => col && r && col.transform.IsChildOf(r.transform);
    Vector3[] GetBoundsCorners(Bounds b)
    {
        Vector3 c = b.center; Vector3 e = b.extents;
        return new[]{
            c + new Vector3(+e.x,+e.y,+e.z), c + new Vector3(+e.x,+e.y,-e.z),
            c + new Vector3(+e.x,-e.y,+e.z), c + new Vector3(+e.x,-e.y,-e.z),
            c + new Vector3(-e.x,+e.y,+e.z), c + new Vector3(-e.x,+e.y,-e.z),
            c + new Vector3(-e.x,-e.y,+e.z), c + new Vector3(-e.x,-e.y,-e.z),
        };
    }

    // ========== 45) Field Lines (2D) ==========
    void DrawFieldLines2D(Vector3 origin, Charge[] qs, int streams, int steps, float stepLen)
    {
        if (qs.Length == 0) return;
        // seed quanh mỗi charge
        foreach (var ch in qs)
        {
            if (!ch.t) continue;
            for (int s = 0; s < streams / qs.Length; s++)
            {
                float ang = (s / (float)Mathf.Max(1, streams / qs.Length)) * Mathf.PI * 2f;
                Vector3 start = ch.t.position + new Vector3(Mathf.Cos(ang), 0, Mathf.Sin(ang)) * 0.2f;
                DrawStreamline(start, qs, steps, stepLen);
            }
        }
    }
    void DrawStreamline(Vector3 start, Charge[] qs, int steps, float stepLen)
    {
        Vector3 p = start;
        Vector3 prev = p;
        Gizmos.color = colPrimary;
        for (int i = 0; i < steps; i++)
        {
            Vector3 E = Vector3.zero;
            foreach (var ch in qs)
            {
                if (!ch.t) continue;
                Vector3 d = p - ch.t.position;
                float r2 = Mathf.Max(1e-4f, d.sqrMagnitude);
                E += (d.normalized) * (ch.q / r2); // hướng từ charge ra
            }
            if (E.sqrMagnitude < 1e-6f) break;
            p += E.normalized * stepLen;
            Gizmos.DrawLine(prev, p);
            prev = p;
        }
    }

    // ========== 46) Heatmap IDW ==========
    void DrawHeatmapIDW(Vector3 center, Vector2Int res, Vector2 size, Transform[] samples, float power)
    {
        int nx = Mathf.Max(2, res.x);
        int ny = Mathf.Max(2, res.y);
        Vector3 o = center - new Vector3(size.x * 0.5f, 0, size.y * 0.5f);
        for (int y = 0; y < ny; y++)
            for (int x = 0; x < nx; x++)
            {
                Vector3 p = o + new Vector3(size.x * (x / (float)(nx - 1)), 0, size.y * (y / (float)(ny - 1)));
                float val = IDW(p, samples, power);
                Color c = Color.Lerp(colOK, colWarn, Mathf.Clamp01(val));
                c.a = 0.8f;
                Gizmos.color = c;
                Gizmos.DrawCube(p, new Vector3(size.x / nx * 0.9f, 0.02f, size.y / ny * 0.9f));
            }
        // sample points
        if (samples != null) { Gizmos.color = colAlt; foreach (var s in samples) if (s) Gizmos.DrawWireSphere(s.position, 0.1f); }
    }
    float IDW(Vector3 p, Transform[] samples, float power)
    {
        if (samples == null || samples.Length == 0) return 0.5f;
        float num = 0, den = 0;
        for (int i = 0; i < samples.Length; i++)
        {
            if (!samples[i]) continue;
            float d = Vector3.Distance(p, samples[i].position);
            d = Mathf.Max(1e-3f, d);
            float w = 1f / Mathf.Pow(d, Mathf.Max(0.1f, power));
            float v = Mathf.Clamp01(samples[i].position.y); // tạm dùng Y làm "giá trị"
            num += w * v; den += w;
        }
        return (den > 0) ? num / den : 0.5f;
    }

    // ========== 47) Visibility Polygon ==========
    void DrawVisibilityPolygon(Vector3 origin, int rays, float radius, LayerMask m)
    {
        List<Vector3> hits = new List<Vector3>(rays);
        for (int i = 0; i < rays; i++)
        {
            float a = (i / (float)rays) * Mathf.PI * 2f;
            Vector3 dir = new Vector3(Mathf.Cos(a), 0, Mathf.Sin(a));
            if (Physics.Raycast(origin + Vector3.up * 0.02f, dir, out var hit, radius, m))
                hits.Add(hit.point);
            else hits.Add(origin + dir * radius);
        }
        Gizmos.color = new Color(colPrimary.r, colPrimary.g, colPrimary.b, 0.8f);
        for (int i = 0; i < hits.Count; i++)
            Gizmos.DrawLine(hits[i], hits[(i + 1) % hits.Count]);
        Gizmos.color = colAlt; Gizmos.DrawWireSphere(origin, 0.1f);
    }

    // ========== 48) Lane Offset ==========
    void DrawLaneOffset(Transform[] pts, float off)
    {
        if (pts.Length < 2) return;
        List<Vector3> center = new List<Vector3>(pts.Length);
        foreach (var t in pts) if (t) center.Add(t.position);
        if (center.Count < 2) return;

        // vẽ center
        Gizmos.color = colPrimary;
        for (int i = 0; i < center.Count - 1; i++) Gizmos.DrawLine(center[i], center[i + 1]);

        // offset trái theo pháp tuyến XZ
        Gizmos.color = colAlt;
        for (int i = 0; i < center.Count; i++)
        {
            Vector3 fwd;
            if (i == 0) fwd = (center[i + 1] - center[i]).normalized;
            else if (i == center.Count - 1) fwd = (center[i] - center[i - 1]).normalized;
            else fwd = (center[i + 1] - center[i - 1]).normalized;
            Vector3 left = Vector3.Cross(Vector3.up, fwd).normalized;
            Vector3 p = center[i] + left * off;
            if (i > 0)
            {
                Vector3 fwdPrev = (center[Mathf.Max(0, i) - (i > 0 ? 1 : 0) + (i > 0 ? 0 : 0)] - center[Mathf.Max(0, i - 1)]); // safe
            }
            if (i > 0) Gizmos.DrawLine(center[i - 1] + Vector3.Cross(Vector3.up, ((i - 1 == 0) ? (center[1] - center[0]).normalized : (center[i - 1] - center[i - 2]).normalized)).normalized * off, p);
            Gizmos.DrawWireSphere(p, 0.05f);
        }
    }

    // ========== 49) 3D Lattice ==========
    void DrawLattice(Vector3 origin, Vector3Int count, Vector3 step)
    {
        Gizmos.color = new Color(colPrimary.r, colPrimary.g, colPrimary.b, 0.5f);
        for (int z = 0; z < count.z; z++)
            for (int y = 0; y < count.y; y++)
                for (int x = 0; x < count.x; x++)
                {
                    Vector3 p = origin + Vector3.Scale(new Vector3(x, y, z), step);
                    Gizmos.DrawWireCube(p, Vector3.Min(step, Vector3.one));
                }
        Gizmos.color = colAlt;
        Vector3 size = Vector3.Scale(new Vector3(Mathf.Max(1, count.x - 1), Mathf.Max(1, count.y - 1), Mathf.Max(1, count.z - 1)), step);
        Gizmos.DrawWireCube(origin + size * 0.5f, size);
    }

    // ========== 50) Motion Trails ==========
    void DrawTrails(Dictionary<Transform, Queue<Vector3>> buffers)
    {
        if (buffers == null) return;
        Gizmos.color = colPrimary;
        foreach (var kv in buffers)
        {
            var q = kv.Value;
            if (q == null || q.Count < 2) continue;
            Vector3 prev = Vector3.zero; bool first = true;
            foreach (var p in q)
            {
                if (first) { prev = p; first = false; continue; }
                Gizmos.DrawLine(prev, p);
                prev = p;
            }
        }
    }
}
