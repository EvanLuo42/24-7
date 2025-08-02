using DG.Tweening;
using UnityEngine;

public class ObjectController : MonoBehaviour
{
    private MeshRenderer _meshRenderer;

    private void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    private void OnMouseDown()
    {
        
        transform.DOKill();
        _meshRenderer.material
            .DOFade(0f, 0.5f)
            .OnComplete(() =>
            {
                var clipboard = FindFirstObjectByType<ClipboardManager>();
                
                Destroy(gameObject);
            });
    }
}
