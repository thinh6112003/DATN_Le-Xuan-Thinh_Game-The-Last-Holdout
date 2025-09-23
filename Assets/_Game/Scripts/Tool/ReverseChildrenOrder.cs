using UnityEngine;
using System.Collections.Generic;

public class ReverseChildrenOrder : MonoBehaviour
{
    [ContextMenu("Reverse Children Order")]
    private void ReverseOrder()
    {
        int childCount = transform.childCount;

        // Lưu danh sách con
        List<Transform> children = new List<Transform>();
        for (int i = 0; i < childCount; i++)
        {
            children.Add(transform.GetChild(i));
        }

        // Đảo ngược thứ tự
        for (int i = 0; i < childCount; i++)
        {
            children[i].SetSiblingIndex(childCount - 1 - i);
        }
    }
}
