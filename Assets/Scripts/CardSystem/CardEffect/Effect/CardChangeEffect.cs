using System;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CardSystem.CardEffect.Effect
{
    public enum CardChangeType
    {
        Add,
        Remove
    }
    
    [CreateAssetMenu(fileName = "CardChangeEffect", menuName = "Scriptable Objects/Effect/Card Change Effect")]
    public class CardChangeEffect : BaseEffect
    {
        public CardChangeType type;
        public int amount;
        
        public override void ApplyEffect()
        {
            var clipboard = FindFirstObjectByType<ClipboardManager>();
            
            switch (type)
            {
                case CardChangeType.Add:
                    for (var i = 0; i < amount; i++)
                    {
                        clipboard.AddRandomCard();
                    }
                    break;
                case CardChangeType.Remove:
                    var cards = clipboard.CardSlots
                        .Select(slot => slot.GetComponentInChildren<Card>())
                        .Where(card => card)
                        .ToList();
                    if (cards.Count == 0) return;
                    for (var i = 0; i < amount; i++)
                    {
                        if (cards.Count == 0) return;
                        var index = Random.Range(0, cards.Count - 1);
                        var card = cards[index];
                        cards.Remove(card);
                        card.Remove();
                        clipboard.ReorderAllCards();
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override void DeApplyEffect()
        {
        }
    }
}