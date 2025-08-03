using System;
using CardSystem.CardEffect;
using CardSystem.Data;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public class ObjectController : MonoBehaviour
{
    public CardEffect dayCardEffect;
    public CardEffect nightCardEffect;
    public GameObject card;
    
    private MeshRenderer _meshRenderer;
    
    private bool _isDragging;
    private Vector3 _dragOffset;
    private float _dragDepth;
    
    private Vector3 _originalPosition;

    private bool _deleting;

    private void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _originalPosition = transform.position;
    }

    private void OnMouseDown()
    {
        if (_deleting) return;
        SfxManager.Instance.Play("Click Object");
        switch (GameContext.currentPhase)
        {
            case LoopManager.LoopPhase.Dawn:
                var loopManager = FindFirstObjectByType<LoopManager>();
                if (loopManager.turnOperateCount == 1) return;
                transform.DOKill();
                _deleting = true;
                loopManager.turnOperateCount++;
                _meshRenderer.material.DOFade(0f, 0.5f).OnComplete(() =>
                    {
                        var clipboard = FindFirstObjectByType<ClipboardManager>();
                
                        clipboard.AddCardFromObject(card);
                
                        Destroy(gameObject);
                    });
                break;
            
            case LoopManager.LoopPhase.Day:
                break;
            
            case LoopManager.LoopPhase.Night:
                _isDragging = true;
                _dragDepth = Vector3.Distance(Camera.main!.transform.position, transform.position);
                _dragOffset = transform.position - GetMouseWorldPosition();
                break;
        }
    }
    
    private void OnMouseDrag()
    {
        if (!_isDragging) return;

        var targetPos = GetMouseWorldPosition() + _dragOffset;
        transform.position = new Vector3(targetPos.x, _originalPosition.y, targetPos.z);
    }

    private void OnMouseUp()
    {
        if (!_isDragging) return;
        _isDragging = false;

        var trashBin = GameObject.Find("TrashBin");
        if (trashBin && IsOverlapping(trashBin))
        {
            nightCardEffect?.ApplyEffect();
            // hash coded,只为模拟使用。应该使用CE，直接写到 nightCardEffect里，每个牌不一样
            GameContext.Attributes.SleepingHours -= 2;
            Destroy(gameObject);
        }
        else
        {
            transform.DOMove(_originalPosition, 0.3f).SetEase(Ease.OutQuad);
        }
    }
    
    private void OnMouseExit()
    {
        // Hide card info when mouse leaves the object
        var monitorDisplay = FindFirstObjectByType<MonitorDisplay>();
        if (monitorDisplay != null)
        {
            monitorDisplay.HideCardInfo();
        }
    }

    private void OnMouseEnter()
    {
        SfxManager.Instance.Play("Hover Card");
        
        // Display card info on monitor
        var monitorDisplay = FindFirstObjectByType<MonitorDisplay>();
        if (!monitorDisplay) return;
        switch (GameContext.currentPhase)
        {
            case LoopManager.LoopPhase.Dawn:
            case LoopManager.LoopPhase.Day:
                monitorDisplay.DisplayCardInfo(dayCardEffect);
                break;
            case LoopManager.LoopPhase.Night:
                monitorDisplay.DisplayCardInfo(nightCardEffect);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private bool IsOverlapping(GameObject other)
    {
        var myCollider = GetComponent<Collider>();
        var otherCollider = other.GetComponent<Collider>();
        if (!myCollider || !otherCollider) return false;

        return myCollider.bounds.Intersects(otherCollider.bounds);
    }

    private Vector3 GetMouseWorldPosition()
    {
        var screenPos = Input.mousePosition;
        screenPos.z = _dragDepth;
        return Camera.main!.ScreenToWorldPoint(screenPos);
    }
}
