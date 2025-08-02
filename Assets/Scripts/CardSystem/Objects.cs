using System.Collections.Generic;
using UnityEngine;

namespace CardSystem
{
    [CreateAssetMenu(fileName = "Objects", menuName = "Scriptable Objects/Objects")]
    public class Objects : ScriptableObject
    {
        public List<GameObject> objectPrefabs;
    }
}