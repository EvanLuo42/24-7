using System.Collections.Generic;
using UnityEngine;

namespace CardSystem
{
    [CreateAssetMenu(fileName = "CardEffectDatabase", menuName = "Scriptable Objects/Card Effect Database")]
    public class CardEffectDatabase : ScriptableObject
    {
        public List<CardEntry> cardEntries;

        [System.Serializable]
        public class CardEntry
        {
            public string id;
            public CardEffect.CardEffect cardEffect;
            public Sprite cardSprite;
            public string overrideName;
            public string overrideDescription;
        }

        public CardEntry GetEntry(CardEffect.CardEffect effect)
        {
            return cardEntries.Find(entry => entry.cardEffect == effect);
        }
    }
}