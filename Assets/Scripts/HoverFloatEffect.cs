using System;
using DG.Tweening;
using UnityEngine;

public class HoverFloatEffect : MonoBehaviour
{
    [Header("Float")]
    public float floatHeight = 0.2f;
    public float duration = 0.3f;
    public Ease easeType = Ease.OutSine;

    private Vector3 _originalPos;
    private Tween _currentTween;
    private bool _hovering;

    void Start()
    {
        _originalPos = transform.localPosition;
        _hovering = false;
    }

    void OnMouseEnter()
    {
        if (_hovering) return;

        _hovering = true;

        _currentTween?.Kill();
        _currentTween = transform.DOLocalMoveY(_originalPos.y + floatHeight, duration)
            .SetEase(easeType);
    }

    void OnMouseExit()
    {
        if (!_hovering) return;

        _hovering = false;

        _currentTween?.Kill();
        _currentTween = transform.DOLocalMoveY(_originalPos.y, duration)
            .SetEase(easeType);
    }

    private void OnMouseDown()
    {
        _currentTween.Kill();
    }
}
