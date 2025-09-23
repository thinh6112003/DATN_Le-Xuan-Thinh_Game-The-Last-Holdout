using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    public TextMeshProUGUI text;
    public float moveSpeed;
    public float duration;
    public void SetText(TextMeshProUGUI textMesh, Vector3 origin)
    {
        text.text = textMesh.text;
        text.color = textMesh.color;
        transform.DOMove(origin +Vector3.up *  moveSpeed * 3f, duration).SetEase(Ease.OutCubic).From(origin);
        text.DOFade(0, duration).SetEase(Ease.OutCubic);
        float scale = transform.localScale.x;
        transform.DOScale(scale* 5f, duration).SetEase(Ease.OutCubic).From(scale * 2f);
    }
}
