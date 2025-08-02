using UnityEngine;

[CreateAssetMenu(fileName = "EffectCustomSample", menuName = "Scriptable Objects/Effect/EffectCustomSample")]
public class EffectCustomSample : BaseEffect
{
    // 自定义的逻辑写在 ApplyEffect()重载方法里
    public override void ApplyEffect()
    {
        // 通过 GameContext.Attributes 静态方法获取 Target对象
        PawnAttributes Target = GameContext.Attributes;
        
        // 通过 Attributes.GetXXX() 获取当前的 XXX属性
        float CurrentPressure = Target.GetPressure();
        float CurrentStamina = Target.GetStamina();
        float CurrentRateOfProgress = Target.GetRateOfProgress();
        
        // 任意计算
        float Factor = CurrentStamina;
        float Pressure = Factor ++;

        // 任意修改属性值
        Target.SetPressure(Factor);
        Target.SetStamina(Pressure);
        Target.SetRateOfProgress(Pressure*Factor);
    }
}
