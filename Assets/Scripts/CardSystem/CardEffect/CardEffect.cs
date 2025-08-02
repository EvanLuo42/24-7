using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardEffect", menuName = "Scriptable Objects/CardEffect")]
public class CardEffect : ScriptableObject
{
    public List<BaseEffect> CardEffectConfig;

    public void ApplyEffect()
    {
        foreach (var EffectConfig in CardEffectConfig)
        {
            EffectConfig.ApplyEffect();
        }
    }
}
