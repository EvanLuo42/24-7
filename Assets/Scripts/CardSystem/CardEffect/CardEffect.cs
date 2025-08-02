using System.Collections.Generic;
using CardSystem.CardEffect.Effect;
using UnityEngine;

namespace CardSystem.CardEffect
{
    [CreateAssetMenu(fileName = "CardEffect", menuName = "Scriptable Objects/CardEffect")]
    public class CardEffect : ScriptableObject
    {
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
