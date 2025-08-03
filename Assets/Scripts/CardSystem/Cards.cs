using System;
using System.Collections.Generic;
using UnityEngine;

namespace CardSystem
{
    [CreateAssetMenu(fileName = "Cards", menuName = "Scriptable Objects/Cards")]
    public class Cards : ScriptableObject
    {
        [Serializable]
        public class RarityCardList
        {
            public float Possibility;
            public List<GameObject> CardPrefabList;
        }
        
        public List<RarityCardList> CardConfig;
        
        
        public RarityCardList GetRandomRarity()
        {
            float total = 0f;

            // 累加概率（可选检测总和）
            foreach (var entry in CardConfig)
                total += entry.Possibility;

            // scriptable object 只能这样写
            float randomValue = Mathf.Lerp(0f, total,UnityEngine.Random.value); 
            
            float cumulative = 0f;

            foreach (var entry in CardConfig)
            {
                cumulative += entry.Possibility;
                if (randomValue <= cumulative)
                {
                    return entry; // 若在稀有度区间里，判定命中这个稀有度
                }
            }
            return CardConfig[CardConfig.Count - 1];
        }
    }
}