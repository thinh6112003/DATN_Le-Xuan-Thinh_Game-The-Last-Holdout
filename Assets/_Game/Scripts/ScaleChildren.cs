using UnityEngine;

public class ScaleChildren : MonoBehaviour
{
    [SerializeField] private float scaleMultiplier = 2.5f;

    [ContextMenu("Scale All Children")]
    private void ScaleAllChildren()
    {
        foreach (Transform child in GetComponentsInChildren<Transform>(true))
        {
            if (child == transform) continue; // bỏ qua chính object cha
            child.localScale *= scaleMultiplier;
        }
    }
}
