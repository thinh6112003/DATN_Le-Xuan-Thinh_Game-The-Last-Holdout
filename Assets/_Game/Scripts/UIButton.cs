using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIButton : Button
{

    [SerializeField]
    public bool isScale = true;

    private Image hover;

    private Vector3 vec3Default;

    protected override void Awake()
    {
        vec3Default = Vector3.one;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        if (isScale && this.interactable)
        {
            if (hover != null)
            {
                hover.gameObject.SetActive(true);
                hover.transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutBack).SetUpdate(true);
            }
            else
                transform.DOScale(new Vector3(1.02f, 1.02f, 1), 0.1f).SetUpdate(true);
        }
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        if (isScale && this.interactable)
        {
            transform.DOScale(vec3Default, 0.1f).SetUpdate(true);
            if (hover != null)
            {
                hover.transform.DOScale(vec3Default, 0.05f).SetUpdate(true).OnComplete(() =>
                {
                    hover.gameObject.SetActive(false);
                });
            }
        }
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        //
        if (isScale && this.interactable)
        {
            transform.DOScale(new Vector3(0.95f, 0.95f, 1), 0.1f).SetUpdate(true);
        }
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        if (isScale && this.interactable)
            transform.DOScale(vec3Default, 0.1f).SetUpdate(true);
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
        System.Random rd = new System.Random();
        int random = rd.Next(1, 100);

    }

    public void OnSetButtonColor(bool isInteractable)
    {
        Image image = GetComponentInChildren<Image>();
        image.color = new Color(image.color.r, image.color.g, image.color.b, isInteractable ? 1f : 0.5f);
        interactable = isInteractable;
    }
}
