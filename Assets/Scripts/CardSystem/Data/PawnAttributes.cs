using UnityEngine;

namespace CardSystem.Data
{
    public class PawnAttributes
    {
        private float _productivity;
    
        public float Productivity
        {
            set => _productivity = Mathf.Clamp(value, 0, 100);
            get => _productivity;
        }

        private float _stress;

        public float Stress
        {
            set => _stress = Mathf.Clamp(value, 0, 100);
            get => _stress;
        }
    
        private float _energy;

        public float Energy
        {
            set => _energy = Mathf.Clamp(value, 0, 100);
            get => _energy;
        }
    
        private float _cook;

        public float Cook
        {
            set => _cook = Mathf.Clamp(value, 0, 100);
            get => _cook;
        }
        
        private float _bonus;

        public float Bonus
        {
            set => _bonus = Mathf.Clamp(value, 0, 100);
            get => _bonus;
        }
    }
}
