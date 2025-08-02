using CardSystem;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class Card : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler,
    IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    public Canvas canvas;
    private Vector3 _offset;

    [Header("Settings")]
    public float hoverScale = 1.1f;
    public float animTime = 0.2f;

    private RectTransform _rect;
    private ClipboardManager _manager;
    [HideInInspector] public Transform slot;

    public int duration;
    public int passed;

    private void Start()
    {
        _rect = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        _manager = GetComponentInParent<ClipboardManager>();
        slot = transform.parent;
        SnapToSlot();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!_manager.dragging)
            transform.DOScale(hoverScale, animTime);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!_manager.dragging)
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
        _offset = transform.localPosition - (Vector3)localPoint;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        transform.DOScale(1f, animTime);
        transform.SetParent(canvas.transform, true);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out var localPoint
        );
        _offset = _rect.anchoredPosition - localPoint;
        transform.SetAsLastSibling();
        _manager.NotifyBeginDrag();
    }

    public void OnDrag(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out var localPoint
        );
        _rect.anchoredPosition = new Vector3(localPoint.x, localPoint.y, 0) + _offset;

        _manager.NotifyDragging(this);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _manager.NotifyEndDrag(this);
        transform.SetParent(slot, true);
        SnapToSlot();
    }

    public void SnapToSlot()
    {
        transform.DOLocalMove(Vector3.zero, animTime).SetEase(Ease.OutQuad);
    }

    public void Remove()
    {
        _manager.NotifyRemove(GetComponent<ApplyCard>());
    }
}