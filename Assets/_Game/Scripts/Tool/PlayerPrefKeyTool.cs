#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System;

[InitializeOnLoad]
public class GameDataEditorWindow : EditorWindow
{
    private UserData data;
    private Wrapper wrapper;
    private SerializedObject serializedData;
    private Vector2 scroll;

    static GameDataEditorWindow()
    {
        // Tự động reload khi dừng play mode
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
    }

    private static void OnPlayModeStateChanged(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.EnteredEditMode)
        {
            var window = GetWindow<GameDataEditorWindow>(false, "Game Data", false);
            if (window != null)
            {
                window.LoadGameData();
            }
        }
    }

    [MenuItem("Tools/Game Data Viewer")]
    public static void ShowWindow()
    {
        GetWindow<GameDataEditorWindow>("Game Data");
    }

    private void OnEnable()
    {
        LoadGameData();
    }

    private void OnGUI()
    {
        if (data == null)
        {
            EditorGUILayout.HelpBox("Không có dữ liệu hoặc lỗi khi tải GameData.", MessageType.Warning);
            if (GUILayout.Button("Tạo dữ liệu mặc định"))
            {
                data = new UserData();
                BindData();
            }
            return;
        }

        // 🔄 Nếu ScriptableObject bị huỷ, khởi tạo lại
        if (serializedData == null || serializedData.targetObject == null)
        {
            BindData();
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("🎮 Game Data", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        scroll = EditorGUILayout.BeginScrollView(scroll);

        serializedData.Update();

        SerializedProperty iterator = serializedData.GetIterator();
        iterator.NextVisible(true);

        while (iterator.NextVisible(false))
        {
            if (iterator.name != "m_Script")
                EditorGUILayout.PropertyField(iterator, true);
        }

        serializedData.ApplyModifiedProperties();
        EditorGUILayout.EndScrollView();

        GUILayout.Space(10);
        GUILayout.BeginHorizontal();

        if (GUILayout.Button("💾 Save"))
        {
            SaveGameData();
        }

        if (GUILayout.Button("🔁 Reload"))
        {
            LoadGameData();
        }

        if (GUILayout.Button("🗑️ Reset"))
        {
            if (EditorUtility.DisplayDialog("Reset Game Data", "Bạn có chắc muốn xóa và tạo lại dữ liệu mặc định không?", "Có", "Không"))
            {
                data = new UserData();
                SaveGameData();
                BindData();
            }
        }

        GUILayout.EndHorizontal();
    }

    private void LoadGameData()
    {
        if (PlayerPrefs.HasKey("DataUser"))
        {
            string json = PlayerPrefs.GetString("DataUser");
            data = JsonUtility.FromJson<UserData>(json);
        }
        else
        {
            data = new UserData();
        }

        BindData();
    }

    private void SaveGameData()
    {
        string json = JsonUtility.ToJson(data, true);
        PlayerPrefs.SetString("DataUser", json);
        PlayerPrefs.Save();
        Debug.Log("✅ GameData đã được lưu:\n" + json);
    }

    private void BindData()
    {
        if (wrapper != null)
        {
            DestroyImmediate(wrapper);
        }

        wrapper = ScriptableObject.CreateInstance<Wrapper>();
        wrapper.hideFlags = HideFlags.DontSave;
        wrapper.data = data;
        serializedData = new SerializedObject(wrapper);
    }

    [Serializable]
    public class Wrapper : ScriptableObject
    {
        public UserData data;
    }
}
#endif
