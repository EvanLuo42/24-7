using System.Collections.Generic;
using UnityEngine;

namespace CardSystem
{
    public class ApplyCard : MonoBehaviour
    {
        public GameObject sourceObject;
        public CardEffect.CardEffect cardEffect;
        
        public void Apply()
        {
            cardEffect.ApplyEffect();
        }
    }
}
