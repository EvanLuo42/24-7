using UnityEngine;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    public PawnAttributes MyAttributeSet;
    public BaseEffect CE_Initialize;
    public ClipboardManager ClipboardManager;
    public ApplyCardInterface CardTest1;
    void Start()
    {
        // 向静态方法库分配静态变量
        GameContext.Attributes = MyAttributeSet;
        // 应用初始化效果
        CE_Initialize.ApplyEffect();
    }

    private bool DoOnce = true;
    // Update is called once per frame
    void Update()
    {
        Debug.Log( "RateOfProgress:" + MyAttributeSet.GetRateOfProgress());
        Debug.Log( "Stamina:" + MyAttributeSet.GetStamina());
        Debug.Log( "Pressure:" + MyAttributeSet.GetPressure());
        
        if (DoOnce == false)return;
        if (Input.GetKey(KeyCode.Space))
        {
            Execute();
            DoOnce = false;
        }
    }

    void Execute()
    {
        // Debug.Log("CardSlot list:"+ClipboardManager.GetCardSlots()==null);
        
        foreach (var CardSlot in ClipboardManager.GetCardSlots())
        {
            // ApplyCardInterface ACI = CardSlot.GetComponentInChildren<ApplyCardInterface>();
            // if (ACI)
            // {
            //     print(ACI.name);
            //     ACI.Apply();
            // }
        }
        CardTest1.Apply();
    }   
}