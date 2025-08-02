using System;
using System.Collections.Generic;
using System.Linq;
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
    
    private int lastBestIndex = -1;

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
        for (var i = 0; i < 1; i++)
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
    
    public void NotifyDragging(Card card)
    {
        var minDist = float.MaxValue;
        var bestIndex = -1;

        for (var i = 0; i < _cardSlots.Count; i++)
        {
            var dist = Vector2.Distance(card.transform.position, _cardSlots[i].position);
            if (!(dist < minDist)) continue;
            minDist = dist;
            bestIndex = i;
        }

        if (bestIndex < 0 || _cardSlots[bestIndex] == card.slot) return;

        lastBestIndex = bestIndex;

        foreach (var trans in _cardSlots.Where(trans => trans.childCount > 0 && trans.GetChild(0) != card.transform))
        {
            trans.GetChild(0).DOLocalMoveX(0, 0.2f);
        }
        
        var offset = Vector3.Distance(_cardSlots[1].position, _cardSlots[0].position) * 0.4f;

        for (var i = 0; i < _cardSlots.Count; i++)
        {
            if (_cardSlots[i] == card.slot || _cardSlots[i].childCount == 0)
                continue;

            var otherCard = _cardSlots[i].GetChild(0);
            if (i < bestIndex)
                otherCard.DOLocalMoveX(-offset, 0.2f);
            else if (i >= bestIndex)
                otherCard.DOLocalMoveX(offset, 0.2f);
        }
    }

    public void NotifyEndDrag(Card card)
    {
        draggingCard = null;
        dragging = false;
        
        foreach (var slot in _cardSlots.Where(slot => slot.childCount > 0))
        {
            slot.GetChild(0).DOLocalMoveX(0, 0.2f);
        }
        
        List<Card> cards = new();
        foreach (var slot in _cardSlots)
        {
            if (slot.childCount <= 0) continue;
            var c = slot.GetChild(0).GetComponent<Card>();
            if (c != card)
                cards.Add(c);
        }
        
        if (lastBestIndex >= 0 && lastBestIndex < _cardSlots.Count)
            cards.Insert(lastBestIndex, card);
        else
            cards.Insert(card.slot.GetSiblingIndex(), card);
        
        for (var i = 0; i < _cardSlots.Count && i < cards.Count; i++)
        {
            var targetSlot = _cardSlots[i];
            var c = cards[i];
            c.transform.SetParent(targetSlot, true);
            c.slot = targetSlot;
            c.SnapToSlot();
        }

        lastBestIndex = -1;
    }
}