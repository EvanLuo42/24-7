using CardSystem.CardEffect;
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
        if (FindFirstObjectByType<GameManager>().CurrentPhase == GamePhase.Plan)
        {
            transform.DOKill();
            _meshRenderer.material
                .DOFade(0f, 0.5f)
                .OnComplete(() =>
                {
                    var clipboard = FindFirstObjectByType<ClipboardManager>();
                
                    clipboard.AddCard(dayCardEffect);
                
                    Destroy(gameObject);
                });
            return;
        }
        _isDragging = true;
        _dragDepth = Vector3.Distance(Camera.main!.transform.position, transform.position);
        _dragOffset = transform.position - GetMouseWorldPosition();
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
            Destroy(gameObject);
        }
        else
        {
            transform.DOMove(_originalPosition, 0.3f).SetEase(Ease.OutQuad);
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
