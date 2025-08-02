using UnityEngine;
using UnityEngine.InputSystem.Controls;

[CreateAssetMenu(fileName = "PawnAttributes", menuName = "Scriptable Objects/PawnAttributes")]
public class PawnAttributes : ScriptableObject
{
    private float RateOfProgress;
    private float Pressure;
    private float Stamina;

    public void SetRateOfProgress(float NewValue)
    {
        RateOfProgress = NewValue;
        RateOfProgress = Mathf.Clamp(RateOfProgress,0,100);
        if (RateOfProgress == 100)
        {
            Debug.Log("游戏胜利啦！");
            // 游戏胜利
        }
    }

    public float GetRateOfProgress()
    {
        return RateOfProgress;
    }
    
    public void SetPressure(float NewValue)
    {
        Pressure = NewValue;
        Pressure = Mathf.Clamp(Pressure,0,100);
    }
    public float GetPressure()
    {
        return Pressure;
    }

    public void SetStamina(float NewValue)
    {
        Stamina = NewValue;
        Stamina = Mathf.Clamp(Stamina,0,100);
    }
    
    public float GetStamina()
    {
        return Stamina;
    }
}
