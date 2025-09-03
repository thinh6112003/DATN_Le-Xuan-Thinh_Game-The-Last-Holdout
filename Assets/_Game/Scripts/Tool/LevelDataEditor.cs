using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class LevelDataEditor : EditorWindow
{
    private LevelData levelData;
    private int selectedWaveIndex = -1;
    private Vector2 leftScroll, rightScroll;

    // nhớ trạng thái foldout của enemy
    private Dictionary<int, bool> enemyFoldout = new Dictionary<int, bool>();

    [MenuItem("Tools/Level Data Editor")]
    public static void ShowWindow()
    {
        GetWindow<LevelDataEditor>("Level Data Editor");
    }

    private void OnGUI()
    {
        GUILayout.Label("Level Data Editor", EditorStyles.boldLabel);

        levelData = (LevelData)EditorGUILayout.ObjectField("Level Data", levelData, typeof(LevelData), false);

        if (levelData == null) return;

        EditorGUILayout.Space();
        EditorGUILayout.BeginHorizontal();

        DrawWaveSidebar();
        DrawWaveDetail();

        EditorGUILayout.EndHorizontal();

        // Auto save khi có thay đổi
        if (GUI.changed)
        {
            Undo.RecordObject(levelData, "Level Data Changed");
            EditorUtility.SetDirty(levelData);
            AssetDatabase.SaveAssets();
        }
    }

    private void DrawWaveSidebar()
    {
        EditorGUILayout.BeginVertical(GUILayout.Width(180));
        GUILayout.Label("Waves", EditorStyles.boldLabel);

        leftScroll = EditorGUILayout.BeginScrollView(leftScroll);

        for (int i = 0; i < levelData.listWaveDatas.Count; i++)
        {
            GUIStyle style = (i == selectedWaveIndex) ? EditorStyles.helpBox : EditorStyles.miniButton;
            if (GUILayout.Button($"Wave {i + 1}", style))
            {
                selectedWaveIndex = i;
            }
        }

        EditorGUILayout.EndScrollView();

        if (GUILayout.Button("+ Add Wave"))
        {
            Undo.RecordObject(levelData, "Add Wave");
            levelData.listWaveDatas.Add(new WaveData { listEnemySpawnDatas = new List<EnemySpawnData>() });
            selectedWaveIndex = levelData.listWaveDatas.Count - 1;
        }

        if (selectedWaveIndex >= 0 && selectedWaveIndex < levelData.listWaveDatas.Count)
        {
            if (GUILayout.Button("Remove Selected Wave"))
            {
                Undo.RecordObject(levelData, "Remove Wave");
                levelData.listWaveDatas.RemoveAt(selectedWaveIndex);
                selectedWaveIndex = -1;
            }
        }

        EditorGUILayout.EndVertical();
    }

    private void DrawWaveDetail()
    {
        EditorGUILayout.BeginVertical("box");
        rightScroll = EditorGUILayout.BeginScrollView(rightScroll);

        if (selectedWaveIndex < 0 || selectedWaveIndex >= levelData.listWaveDatas.Count)
        {
            GUILayout.Label("Select a Wave to edit", EditorStyles.centeredGreyMiniLabel);
        }
        else
        {
            var wave = levelData.listWaveDatas[selectedWaveIndex];

            GUILayout.Label($"Wave {selectedWaveIndex + 1} Detail", EditorStyles.boldLabel);
            wave.timeForWave = EditorGUILayout.FloatField("Time For Wave", wave.timeForWave);
            wave.timeForReady = EditorGUILayout.FloatField("Time For Ready", wave.timeForReady);

            EditorGUILayout.Space();
            GUILayout.Label("Enemy Spawns", EditorStyles.boldLabel);

            // === Header row ===
            EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
            GUILayout.Label("Time", GUILayout.Width(60));
            GUILayout.Label("Type", GUILayout.Width(100));
            GUILayout.Label("Lane", GUILayout.Width(75));
            GUILayout.Label(" ", GUILayout.Width(20));
            EditorGUILayout.EndHorizontal();

            for (int j = 0; j < wave.listEnemySpawnDatas.Count; j++)
            {
                var enemy = wave.listEnemySpawnDatas[j];

                EditorGUILayout.BeginVertical("box");

                // === Row: Time | Type | Lane | X ===
                EditorGUILayout.BeginHorizontal();

                enemy.timeInWave = EditorGUILayout.FloatField(enemy.timeInWave, GUILayout.Width(60));
                enemy.enemyType = (EnemyType)EditorGUILayout.EnumPopup(enemy.enemyType, GUILayout.Width(100));

                GUILayout.Label("Lane", GUILayout.Width(35));
                enemy.laneID = EditorGUILayout.IntField(enemy.laneID, GUILayout.Width(40));

                GUILayout.FlexibleSpace();

                if (GUILayout.Button("X", GUILayout.Width(20)))
                {
                    Undo.RecordObject(levelData, "Remove Enemy");
                    wave.listEnemySpawnDatas.RemoveAt(j);
                    break;
                }

                EditorGUILayout.EndHorizontal();

                // === Paths foldout ===
                if (!enemyFoldout.ContainsKey(j)) enemyFoldout[j] = false;
                enemyFoldout[j] = EditorGUILayout.Foldout(enemyFoldout[j], "Paths", true);

                if (enemyFoldout[j])
                {
                    EditorGUI.indentLevel++;
                    for (int k = 0; k < enemy.listPathID.Count; k++)
                    {
                        EditorGUILayout.BeginHorizontal();
                        enemy.listPathID[k] = EditorGUILayout.IntField($"Path {k + 1}", enemy.listPathID[k]);
                        if (GUILayout.Button("X", GUILayout.Width(20)))
                        {
                            Undo.RecordObject(levelData, "Remove Path");
                            enemy.listPathID.RemoveAt(k);
                            break;
                        }
                        EditorGUILayout.EndHorizontal();
                    }

                    if (GUILayout.Button("+ Add Path"))
                    {
                        Undo.RecordObject(levelData, "Add Path");
                        enemy.listPathID.Add(0);
                    }
                    EditorGUI.indentLevel--;
                }

                EditorGUILayout.EndVertical();
            }

            if (GUILayout.Button("+ Add Enemy"))
            {
                Undo.RecordObject(levelData, "Add Enemy");
                wave.listEnemySpawnDatas.Add(new EnemySpawnData());
            }
        }

        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();
    }
}
