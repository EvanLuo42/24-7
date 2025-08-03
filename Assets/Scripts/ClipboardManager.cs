using System.Collections.Generic;
using System.Linq;
using CardSystem;
using CardSystem.CardEffect;
using CardSystem.Data;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ClipboardManager : MonoBehaviour
{
    public List<Transform> CardSlots { get; } = new();

    public GameObject cardSlotPrefab;
    public Cards cardsSo;
    
    [HideInInspector] public bool dragging;
    public Button basicCardButton;

    public CardEffect goWorkEffect;
    public GameObject workOverloadPrefab;
    
    private int _lastBestIndex = -1;

    private void Start()
    {
        for (var i = 0; i < 12; i++)
        {
            var slot = Instantiate(cardSlotPrefab, transform, false).transform;
            CardSlots.Add(slot);
        }

        basicCardButton.onClick.AddListener(OnClickAddBasicCard);
    }

    public void NotifyBeginDrag()
    {
        dragging = true;
    }
    
    public void NotifyDragging(Card card)
    {
        var minDist = float.MaxValue;
        var bestIndex = -1;

        for (var i = 0; i < CardSlots.Count; i++)
        {
            var dist = Vector2.Distance(card.transform.position, CardSlots[i].position);
            if (!(dist < minDist)) continue;
            minDist = dist;
            bestIndex = i;
        }

        if (bestIndex < 0 || CardSlots[bestIndex] == card.slot) return;

        _lastBestIndex = bestIndex;

        foreach (var trans in CardSlots.Where(trans => trans.childCount > 0 && trans.GetChild(0) != card.transform))
        {
            trans.GetChild(0).DOLocalMoveX(0, 0.2f);
        }
        
        var offset = Vector3.Distance(CardSlots[1].position, CardSlots[0].position) * 0.4f;

        for (var i = 0; i < CardSlots.Count; i++)
        {
            if (CardSlots[i] == card.slot || CardSlots[i].childCount == 0)
                continue;

            var otherCard = CardSlots[i].GetChild(0);
            if (i < bestIndex)
                otherCard.DOLocalMoveX(-offset, 0.2f);
            else if (i >= bestIndex)
                otherCard.DOLocalMoveX(offset, 0.2f);
        }
    }

    public void NotifyEndDrag(Card card)
    {
        dragging = false;
        
        foreach (var slot in CardSlots.Where(slot => slot.childCount > 0))
        {
            slot.GetChild(0).DOLocalMoveX(0, 0.2f);
        }
        
        // get all cards in slots, excluding the dragged card
        var cards = CardSlots
            .Select(slot => slot.GetComponentInChildren<Card>())
            .Where(c => c && c != card)
            .ToList();
        
        var insertIndex = 0;
        
        if (_lastBestIndex >= 0)
        {
            insertIndex = Mathf.Clamp(_lastBestIndex, 0, cards.Count);
            cards.Insert(insertIndex, card);
        }
        else
        {
            // if no best index found, insert at the original index of the card
            var originalIndex = card.slot ? card.slot.GetSiblingIndex() : 0;
            insertIndex = Mathf.Clamp(originalIndex, 0, cards.Count);
            cards.Insert(insertIndex, card);
        }
        
        // Reassign all cards to the correct slots
        for (var i = 0; i < cards.Count && i < CardSlots.Count; i++)
        {
            var targetSlot = CardSlots[i];
            var c = cards[i];
            c.transform.SetParent(targetSlot, true);
            c.slot = targetSlot;
            c.SnapToSlot();
        }

        _lastBestIndex = -1;
    }

    public void NotifyRemove(ApplyCard applyCard)
    {
        foreach (var config in applyCard.cardEffect.cardEffectConfig)
        {
            config.DeApplyEffect();
        }
        
        var cards = CardSlots
            .Select(slot => slot.GetComponentInChildren<Card>())
            .Where(c => c)
            .ToList();
        
        for (var i = 0; i < cards.Count; i++)
        {
            var targetSlot = CardSlots[i];
            var c = cards[i];

            c.transform.SetParent(targetSlot, true);
            c.slot = targetSlot;
            c.SnapToSlot();
        }
        
        Destroy(applyCard.gameObject);
        
        ReorderAllCards();
    }

    public void AddRandomCard()
    {
        // 每个卡槽抽一次卡
        foreach (var slot in CardSlots.Where(slot => !slot.GetComponentInChildren<Card>()))
        {
            // 长难句，别管
            Instantiate(cardsSo.GetRandomRarity().CardPrefabList[Random.Range(0, cardsSo.GetRandomRarity().CardPrefabList.Count)], slot, false);
            return;
        }
    }

    public void AddCard(CardEffect dayCardEffect)
    {
        foreach (var slot in CardSlots.Where(slot => !slot.GetComponentInChildren<Card>()))
        {
            foreach (var rarityCardList in cardsSo.CardConfig)
            {
                foreach (var cardPrefab in rarityCardList.CardPrefabList.Where(cardPrefab => cardPrefab.GetComponent<ApplyCard>().cardEffect == dayCardEffect))
                {
                    var dayCard = Instantiate(cardPrefab, slot, false);
                    var dayLocalPos = dayCard.transform.localPosition;
                    dayCard.transform.localPosition = new Vector3(dayLocalPos.x, dayLocalPos.y - 30f, 0);
                    dayCard.transform.DOLocalMoveY(0, 0.3f);
                    return;
                }
            }
        }
    }
    
    public void ReorderAllCards()
    {
        var cards = CardSlots
            .Select(slot => slot.GetComponentInChildren<Card>())
            .Where(c => c)
            .ToList();

        for (var i = 0; i < cards.Count && i < CardSlots.Count; i++)
        {
            var targetSlot = CardSlots[i];
            var c = cards[i];
            c.transform.SetParent(targetSlot, true);
            c.slot = targetSlot;
            c.SnapToSlot();
        }
    }

    private void OnClickAddBasicCard()
    {
        var manager = FindFirstObjectByType<LoopManager>();
        if (manager.turnOperateCount == 1) return;
        switch (GameContext.currentPhase)
        {
            case LoopManager.LoopPhase.Dawn:
                AddCard(goWorkEffect);
                ReorderAllCards();
                manager.turnOperateCount++;
                break;
            case LoopManager.LoopPhase.Day:
                break;
            case LoopManager.LoopPhase.Night:
                FindFirstObjectByType<TableManager>().AddObjectByPrefab(workOverloadPrefab);
                manager.turnOperateCount++;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}