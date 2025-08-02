using UnityEngine;

namespace CardSystem
{
    public class ApplyCard : MonoBehaviour
    {
        public CardEffect.CardEffect cardEffect;

        public void Apply()
        {
            cardEffect.ApplyEffect();
        }
    }
}
