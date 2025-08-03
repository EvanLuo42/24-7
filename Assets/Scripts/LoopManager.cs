using System;
using System.Collections;
using System.Linq;
using CardSystem;
using CardSystem.CardEffect.Effect;
using CardSystem.Data;
using DG.Tweening;
using UnityEngine;

// 她说我是流程控制专家
public class LoopManager : MonoBehaviour
{
    // “睡觉”等 intermediate被放在 Dawn,Day和 Night里了
    public enum LoopPhase
    {
        Dawn,
        Day,
        Night,
    }
    
    public BaseEffect initEffect;
    public ClipboardManager clipboardManager;
    public TableManager tableManager;

    private AudioSource _audioSource;

    // Phase写在 Game Contex里面
    // private LoopPhase _currentPhase;
    // private LoopPhase _lastPhase;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // 游戏从Dawn开始
        GameContext.currentPhase = LoopPhase.Dawn;
        // LastPhase需要和CurrentPhase不同，以在游戏开始时触发 Dawn的逻辑
        GameContext.lastPhase = LoopPhase.Night;
        // 初始化
        initEffect.ApplyEffect();
        // initEffect的补丁，暂时这么写因为不知道在哪改 initEffect
        GameContext.Attributes.SleepingHours = 14;
    }

    // Update is called once per frame
    void Update()
    {            
        // 偷懒写在这里了
        if (GameContext.currentPhase == LoopPhase.Night)
        {
            if (GameContext.Attributes.SleepingHours == 0)
            {
                StopAllCoroutines();
                GameContext.currentPhase = LoopPhase.Dawn;
            }
        }

        Debug.Log(GameContext.currentPhase);
        // 状态无改变则返回
        if (GameContext.currentPhase == GameContext.lastPhase){return;}
        
        // 若发生改变，则广播 EnterPhase事件
        switch (GameContext.currentPhase)
        {
            case LoopPhase.Dawn:
                StartCoroutine(Dawn());
                break;
            case LoopPhase.Day:
                StartCoroutine(Day());
                break;
            case LoopPhase.Night:
                StartCoroutine(Night());
                break;
        }
        
        // 同步状态
        GameContext.lastPhase = GameContext.currentPhase;
    }

    IEnumerator Dawn()
    {
        // bgm
        OstManager.Instance.Play("Planning");
        
        Debug.Log("TimeSlept:" + GameContext.Attributes.SleepingHours);
        Debug.Log("Cards Drawn" + Mathf.FloorToInt(GameContext.Attributes.SleepingHours/4));
        
        // 按睡眠时间抽牌
        tableManager.GenerateObjects(Mathf.FloorToInt(GameContext.Attributes.SleepingHours/4));
        
        // 按睡眠时间回精力，一个脱离 Card System 架构的操作，不推荐。
        float Refresh = Mathf.Lerp(0, 100, Mathf.Clamp(GameContext.Attributes.SleepingHours / 8, 0, 1));
        GameContext.Attributes.SleepingHours += Refresh;
        Debug.Log("Refresh:" + Refresh);
        
        // 按下空格结束Dawn，行动轴锁定，进入 Day
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        GameContext.currentPhase = LoopPhase.Day;
    }
    
    IEnumerator Day()
    {
        // bgm
        OstManager.Instance.Play("Daytime");

        // 播放行动轴，运用 Effect. 由ExecuteCards决定何时进入黑夜
        yield return StartCoroutine(ExecuteCards());
    }
    
    IEnumerator Night()
    {
        // 又是一个脱离 Card System 架构的操作。严格来说，每个对Attribute的修改都应该通过 CE，因为这样好拓展，好做网络同步，只用做 CE的就行。
        GameContext.Attributes.SleepingHours = 14;
        
        // bgm
        OstManager.Instance.Play("Nighttime");
        
        // 按下空格结束 Night，进入 Dawn
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        GameContext.currentPhase = LoopPhase.Dawn;
    }

    private IEnumerator ExecuteCards()
    {
        var cardsToExecute = clipboardManager.CardSlots
            .Select(slot => slot.GetComponentInChildren<ApplyCard>())
            .Where(card => card != null)
            .ToList();

        foreach (var card in cardsToExecute)
        {
            var rect = card.GetComponent<RectTransform>();
            var originalPos = rect.localPosition;
            yield return rect.DOLocalMoveY(originalPos.y + 50f, 0.2f)
                .SetEase(Ease.OutQuad)
                .WaitForCompletion();

            card.Apply();
            yield return new WaitForSeconds(1f);

            var cardController = card.GetComponent<Card>();
            cardController.passed++;

            if (cardController.passed < cardController.duration)
            {
                yield return rect.DOLocalMove(originalPos, 0.2f)
                    .SetEase(Ease.InQuad)
                    .WaitForCompletion();
            }
            else
            {
                SfxManager.Instance.Play("Remove Card");
                yield return rect.DOLocalMoveY(originalPos.y - 100f, 0.2f)
                    .SetEase(Ease.InQuad)
                    .OnComplete(() => cardController.Remove())
                    .WaitForCompletion();
            }

            clipboardManager.ReorderAllCards();
            yield return new WaitForSeconds(1f);
        }

        yield return new WaitForSeconds(1f);
        GameContext.currentPhase = LoopPhase.Night;
    }
}
