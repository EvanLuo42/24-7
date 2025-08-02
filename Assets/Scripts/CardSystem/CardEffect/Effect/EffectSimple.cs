using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(fileName = "EffectSimple", menuName = "Scriptable Objects/Effect/EffectSimple")]
public class EffectSimple : BaseEffect
{
    [Serializable]
    public enum Attribute
    {
    RateOfProgress,
    Pressure,
    Stamina
    }
    [Serializable]
    public enum CalculationType
    {
        add,
        minus,
        multiply,
        divide,
        set
    }
    [Serializable]
    public struct AttributeChangeConfig
    {
        public Attribute AttributeToChange;
        public CalculationType OperationToDo;
        public float Value;
    }

    public List<AttributeChangeConfig> AttributeChangeConfigs;
    
    public override void ApplyEffect()
    {
        foreach (var ChangeConfig in AttributeChangeConfigs)
        {
            float ValueToOperate = 0;
            switch (ChangeConfig.AttributeToChange)
            {
                case Attribute.RateOfProgress:
                    ValueToOperate = GameContext.Attributes.GetRateOfProgress();
                    break;
                case Attribute.Pressure:
                    ValueToOperate = GameContext.Attributes.GetPressure();
                    break;
                case Attribute.Stamina:
                    ValueToOperate = GameContext.Attributes.GetStamina();
                    break;
            }
            switch (ChangeConfig.OperationToDo)
            {
                case CalculationType.add:
                    ValueToOperate += ChangeConfig.Value;
                    break;
                case CalculationType.minus:
                    ValueToOperate -= ChangeConfig.Value;
                    break;
                case CalculationType.multiply:
                    ValueToOperate *= ChangeConfig.Value;
                    break;
                case CalculationType.divide:
                    ValueToOperate /= ChangeConfig.Value;
                    break;
                case CalculationType.set:
                    ValueToOperate = ChangeConfig.Value;
                    break;
            }
            switch (ChangeConfig.AttributeToChange)
            {
                case Attribute.RateOfProgress:
                    GameContext.Attributes.SetRateOfProgress(ValueToOperate);
                    break;
                case Attribute.Pressure:
                    GameContext.Attributes.SetPressure(ValueToOperate);
                    break;
                case Attribute.Stamina:
                    GameContext.Attributes.SetStamina(ValueToOperate);
                    break;
            }
        }
    }
}
