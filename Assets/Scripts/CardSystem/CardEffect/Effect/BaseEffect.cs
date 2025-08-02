using UnityEngine;

namespace CardSystem.CardEffect.Effect
{
    [CreateAssetMenu(fileName = "BaseEffect", menuName = "Scriptable Objects/Effect/BaseEffect")]
    public abstract class BaseEffect : ScriptableObject
    {
        public abstract void ApplyEffect();
        public abstract void DeApplyEffect();
    }
}
