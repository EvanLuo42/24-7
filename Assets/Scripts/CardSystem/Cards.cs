using System.Collections.Generic;
using UnityEngine;

namespace CardSystem
{
    [CreateAssetMenu(fileName = "Cards", menuName = "Scriptable Objects/Cards")]
    public class Cards : ScriptableObject
    {
        public List<GameObject> cardPrefabs;
    }
}