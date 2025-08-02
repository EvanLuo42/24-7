using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ClipboardManager : MonoBehaviour
{
    private readonly List<Transform> _cardSlots = new();
    
    public GameObject cardSlotPrefab;
    public GameObject cardPrefab;
    private Card draggingCard;
    [HideInInspector] public bool dragging;

    private void Start()
    {
        var colors = new List<Color>
        {
            Color.blue,
            Color.magenta,
            Color.green,
            Color.cyan,
            Color.black
        };
        for (var i = 0; i < 5; i++)
        {
            var slot = Instantiate(cardSlotPrefab, transform, false).transform;
            _cardSlots.Add(slot);
            var card = Instantiate(cardPrefab, slot.transform, false);
            card.GetComponent<Image>().color = colors[i];
        }
    }
    
    public List<Transform> GetCardSlots() => _cardSlots;

    public void NotifyBeginDrag(Card card)
    {
        draggingCard = card;
        dragging = true;
    }

    [Obsolete("Obsolete")]
    public void NotifyDragging(Card card)
    {
        float minDist = float.MaxValue;
        int bestIndex = -1;

        for (int i = 0; i < _cardSlots.Count; i++)
        {
            float dist = Vector2.Distance(card.transform.position, _cardSlots[i].position);
            if (dist < minDist)
            {
                minDist = dist;
                bestIndex = i;
            }
        }

        if (bestIndex >= 0 && _cardSlots[bestIndex] != card.slot)
        {
            Dictionary<Card, Vector3> originalWorldPos = new();

            foreach (Transform slot in _cardSlots)
            {
                var c = slot.childCount > 0 ? slot.GetChild(0).GetComponent<Card>() : null;
                if (c != null && c != card)
                {
                    c.transform.SetParent(slot, false);
                }
            }
            
            Dictionary<Card, Vector3> slotPositions = new();
            foreach (Transform slot in _cardSlots)
            {
                var c = slot.childCount > 0 ? slot.GetChild(0).GetComponent<Card>() : null;
                if (c != null && c != card)
                {
                    slotPositions[c] = slot.position;
                }
            }
            
            card.slot.SetSiblingIndex(bestIndex);
            
            foreach (var kv in slotPositions)
            {
                var c = kv.Key;
                var newWorldPos = kv.Value;

                var oldWorldPos = c.transform.position;

                Canvas canvas = FindObjectOfType<Canvas>();
                c.transform.SetParent(canvas.transform, true);
                c.transform.position = oldWorldPos;

                c.transform.SetParent(c.slot, true);
                c.transform.DOMove(newWorldPos, c.animTime).SetEase(Ease.OutQuad);
            }
        }
    }

    public void NotifyEndDrag(Card card)
    {
        draggingCard = null;
        dragging = false;
    }
}