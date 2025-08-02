using System;
using CardSystem.CardEffect;
using CardSystem.Data;
using DG.Tweening;
using UnityEngine;

public class ObjectController : MonoBehaviour
{
    public CardEffect dayCardEffect;
    public CardEffect nightCardEffect;
    
    private MeshRenderer _meshRenderer;
    
    private bool _isDragging;
    private Vector3 _dragOffset;
    private float _dragDepth;
    
    private Vector3 _originalPosition;

    private void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _originalPosition = transform.position;
    }

    private void OnMouseDown()
    {

        SfxManager.Instance.Play("Click Object");
        // 何尝不算一种 NTR
        switch (GameContext.currentPhase)
        {
            case LoopManager.LoopPhase.Dawn:
                transform.DOKill();
                _meshRenderer.material
                    .DOFade(0f, 0.5f)
                    .OnComplete(() =>
                    {
                        var clipboard = FindFirstObjectByType<ClipboardManager>();
                
                        clipboard.AddCard(dayCardEffect);
                
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

    private void OnMouseEnter()
    {
        SfxManager.Instance.Play("Hover Card");
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
