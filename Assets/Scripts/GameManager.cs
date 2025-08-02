using UnityEngine;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    public PawnAttributes MyAttributeSet;
    public BaseEffect CE_Initialize;
    public ClipboardManager ClipboardManager;
    void Start()
    {
        // 向静态方法库分配静态变量
        GameContext.Attributes = MyAttributeSet;
        // 应用初始化效果
        CE_Initialize.ApplyEffect();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            Execute();
        }
    }

    void Execute()
    {
        foreach (var CardSlot in ClipboardManager.GetCardSlots())
        {
            CardSlot.GetComponent<ApplyCardInterface>().Apply();
        }
    }
}