#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;
using System.IO;

public class SceneSwitcher : EditorWindow
{
    private static string[] scenePaths;
    private const string SceneFolder = "Assets/_Game/Scenes"; // Đổi đường dẫn nếu cần

    [MenuItem("Change Scene/Open Scene List")]
    private static void ShowSceneMenu()
    {
        SceneSwitcher window = GetWindow<SceneSwitcher>();
        window.titleContent = new GUIContent("Scene Switcher");
        window.ShowUtility();
    }

    private void OnEnable()
    {
        scenePaths = Directory.GetFiles(SceneFolder, "*.unity", SearchOption.AllDirectories);
    }

    private void OnGUI()
    {
        GUILayout.Label("Select a Scene", EditorStyles.boldLabel);
        if (scenePaths.Length == 0)
        {
            EditorGUILayout.LabelField("No scenes found");
        }
        else
        {
            foreach (var scenePath in scenePaths)
            {
                string sceneName = Path.GetFileNameWithoutExtension(scenePath);
                if (GUILayout.Button(sceneName))
                {
                    OpenScene(scenePath);
                    Close();
                }
            }
        }
    }

    private static void OpenScene(string scenePath)
    {
        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
        {
            EditorSceneManager.OpenScene(scenePath);
        }
    }
}
#endif