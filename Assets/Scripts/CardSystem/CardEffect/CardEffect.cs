using System.Collections.Generic;
using CardSystem.CardEffect.Effect;
using UnityEngine;

namespace CardSystem.CardEffect
{
    [CreateAssetMenu(fileName = "CardEffect", menuName = "Scriptable Objects/CardEffect")]
    public class CardEffect : ScriptableObject
    {
        [Header("Card Information")]
        public string cardName;
        [TextArea(3, 10)]
        public string cardDescription;
        
        [Header("Card Effects")]
        public List<BaseEffect> cardEffectConfig;

        public void ApplyEffect()
        {
            foreach (var effectConfig in cardEffectConfig)
            {
                effectConfig.ApplyEffect();
            }
        }
    }
}
