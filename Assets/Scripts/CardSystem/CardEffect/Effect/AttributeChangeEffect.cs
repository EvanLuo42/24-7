using System;
using System.Collections.Generic;
using CardSystem.Data;
using UnityEngine;
using UnityEngine.Serialization;

namespace CardSystem.CardEffect.Effect
{
    [CreateAssetMenu(fileName = "AttributeChangeEffect", menuName = "Scriptable Objects/Effect/Attribute Change Effect")]
    public class AttributeChangeEffect : BaseEffect
    {
        [Serializable]
        public struct LuckyAttributeChange
        {
            public float Possibility;
            public AttributeChangeConfig AttributeOperationIfLucky;
            public AttributeChangeConfig AttributeOperationElse;
        }
        
        public List<LuckyAttributeChange> LuckyAttributeChangeConfigs;
        
#region Standard Attribute Logic
        [Serializable]
        public enum Attribute
        {
            Productivity,
            Energy,
            Stress,
            Cook,
            Bonus
        }
        [Serializable]
        public enum CalculationType
        {
            Add,
            Minus,
            Multiply,
            Divide,
            Set
        }
        
        [Serializable]
        public struct AttributeChangeConfig
        {
            public Attribute attributeToOperate; 
            public CalculationType operationToDo;
            public float value;
        }

        public List<AttributeChangeConfig> attributeChangeConfigs;
        
        public override void ApplyEffect()
        {
            foreach (var LuckyAttributeChange in LuckyAttributeChangeConfigs)
            {

                if (UnityEngine.Random.value <= LuckyAttributeChange.Possibility)
                {
                    attributeChangeConfigs.Add(LuckyAttributeChange.AttributeOperationIfLucky);
                }
                else
                {
                    attributeChangeConfigs.Add(LuckyAttributeChange.AttributeOperationElse);
                }
            }
            
            foreach (var changeConfig in attributeChangeConfigs)
            {
                var valueToOperate = changeConfig.attributeToOperate switch
                {
                    Attribute.Productivity => GameContext.Attributes.Productivity,
                    Attribute.Stress => GameContext.Attributes.Stress,
                    Attribute.Energy => GameContext.Attributes.Energy,
                    Attribute.Cook => GameContext.Attributes.Cook,
                    Attribute.Bonus => GameContext.Attributes.Bonus,
                    _ => throw new ArgumentOutOfRangeException()
                };

                if (changeConfig.attributeToOperate is Attribute.Bonus or not Attribute.Productivity)
                {
                    switch (changeConfig.operationToDo)
                    {
                        case CalculationType.Add:
                            valueToOperate += changeConfig.value;
                            break;
                        case CalculationType.Minus:
                            valueToOperate -= changeConfig.value;
                            break;
                        case CalculationType.Multiply:
                            valueToOperate *= changeConfig.value;
                            break;
                        case CalculationType.Divide:
                            valueToOperate /= changeConfig.value;
                            break;
                        case CalculationType.Set:
                            valueToOperate = changeConfig.value;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
                else
                {
                    var bonus = GameContext.Attributes.Bonus;
                    switch (changeConfig.operationToDo)
                    {
                        case CalculationType.Add:
                            valueToOperate += changeConfig.value * bonus;
                            break;
                        case CalculationType.Minus:
                            valueToOperate -= changeConfig.value * bonus;
                            break;
                        case CalculationType.Multiply:
                            valueToOperate *= changeConfig.value * bonus;
                            break;
                        case CalculationType.Divide:
                            valueToOperate /= changeConfig.value * bonus;
                            break;
                        case CalculationType.Set:
                            valueToOperate = changeConfig.value;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }

                switch (changeConfig.attributeToOperate)
                {
                    case Attribute.Productivity:
                        GameContext.Attributes.Productivity = valueToOperate;
                        break;
                    case Attribute.Energy:
                        GameContext.Attributes.Energy = valueToOperate;
                        break;
                    case Attribute.Cook:
                        GameContext.Attributes.Cook = valueToOperate;
                        break;
                    case Attribute.Stress:
                        GameContext.Attributes.Stress = valueToOperate;
                        break;
                    case Attribute.Bonus:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public override void DeApplyEffect()
        {
            foreach (var changeConfig in attributeChangeConfigs)
            {
                if (changeConfig.attributeToOperate == Attribute.Bonus)
                {
                    switch (changeConfig.operationToDo)
                    {
                        case CalculationType.Add:
                            GameContext.Attributes.Bonus -= changeConfig.value;
                            break;
                        case CalculationType.Minus:
                            GameContext.Attributes.Bonus += changeConfig.value;
                            break;
                        case CalculationType.Multiply:
                            GameContext.Attributes.Bonus /= changeConfig.value;
                            break;
                        case CalculationType.Divide:
                            GameContext.Attributes.Bonus *= changeConfig.value;
                            break;
                        case CalculationType.Set:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }

                GameContext.Attributes.Bonus = Mathf.Max(GameContext.Attributes.Bonus, 0);
            }
        }
#endregion

    }
}
