using UnityEngine;

namespace CardSystem.Data
{
    public class PawnAttributes
    {
        private float _productivity;
    
        public float Productivity
        {
            set => _productivity = Mathf.Clamp(value, 0, 1);
            get => _productivity;
        }

        private float _stress;

        public float Stress
        {
            set => _stress = Mathf.Clamp(value, 0, 1);
            get => _stress;
        }
    
        private float _energy = 1f;

        public float Energy
        {
            set => _energy = Mathf.Clamp(value, 0, 1);
            get => _energy;
        }
    
        private float _cook;

        public float Cook
        {
            set => _cook = Mathf.Clamp(value, 0, 1);
            get => _cook;
        }
        
        private float _bonus = 1;

        public float Bonus
        {
            set => _bonus = Mathf.Max(0, value);
            get => _bonus;
        }
        
        private float _sleepingHours;

        public float SleepingHours
        {
            set => _sleepingHours = Mathf.Clamp(value, 0, 14);
            get => _sleepingHours;
        }
        
    }
}
