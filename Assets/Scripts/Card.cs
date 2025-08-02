using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler,
    IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    private Canvas canvas;
    private Vector3 offset;
    private bool isDragging;

    [Header("Settings")]
    public float hoverScale = 1.1f;
    public float animTime = 0.2f;

    private RectTransform rect;
    private ClipboardManager manager;
    [HideInInspector] public Transform slot;

    private void Start()
    {
        rect = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        manager = GetComponentInParent<ClipboardManager>();
        slot = transform.parent;
        SnapToSlot();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!manager.dragging)
            transform.DOScale(hoverScale, animTime);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!manager.dragging)
            transform.DOScale(1f, animTime);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out var localPoint
        );
        offset = transform.localPosition - (Vector3)localPoint;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        isDragging = true;
        transform.DOScale(1f, animTime);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out var localPoint
        );
        offset = rect.anchoredPosition - localPoint;
        transform.SetParent(canvas.transform, true);
        transform.SetAsLastSibling();
        manager.NotifyBeginDrag(this);
    }

    public void OnDrag(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out var localPoint
        );
        transform.localPosition = new Vector3(localPoint.x, localPoint.y, 0) + offset;

        manager.NotifyDragging(this);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;
        manager.NotifyEndDrag(this);
        transform.SetParent(slot, true);
        SnapToSlot();
    }

    public void SnapToSlot()
    {
        transform.DOLocalMove(Vector3.zero, animTime).SetEase(Ease.OutQuad);
    }
}