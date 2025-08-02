using System.Collections.Generic;
using CardSystem.CardEffect.Effect;
using UnityEngine;

namespace CardSystem
{
    [CreateAssetMenu(fileName = "Effects", menuName = "Scriptable Objects/Effects")]
    public class Effects : ScriptableObject
    {
        public List<BaseEffect> effects;
    }
}
