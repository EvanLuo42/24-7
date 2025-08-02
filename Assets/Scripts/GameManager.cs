using System;
using System.Collections;
using CardSystem;
using CardSystem.CardEffect.Effect;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public enum GamePhase
{
    Start,
    Plan,
    Day,
    Night,
    Settle,
    Running
}

public class GameManager : MonoBehaviour
{
    public BaseEffect initEffect;
    public ClipboardManager clipboardManager;
    public TableManager tableManager;

    private AudioSource _audioSource;

    public GamePhase CurrentPhase { get; private set; } = GamePhase.Start;

    private void Update()
    {
        switch (CurrentPhase)
        {
            case GamePhase.Start:
                Debug.Log("Start");
                initEffect.ApplyEffect();
                tableManager.GenerateObjects(3);
                Continue();
                break;
            case GamePhase.Plan:
                Debug.Log("Plan");
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    Continue();
                }
                break;
            case GamePhase.Day:
                Debug.Log("Day");
                StartCoroutine(Execute());
                CurrentPhase = GamePhase.Running;
                break;
            case GamePhase.Night:
                Debug.Log("Night");
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    Continue();
                }
                break;
            case GamePhase.Settle:
                Debug.Log("Settle");
                tableManager.GenerateObjects(Random.Range(1, 3));
                Continue();
                break;
            case GamePhase.Running:
                Debug.Log("Settle");
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void Continue(GamePhase phase = GamePhase.Start)
    {
        switch (CurrentPhase)
        {
            case GamePhase.Start:
                CurrentPhase = GamePhase.Plan;
                OstManager.Instance.Play("Planning");
                return;
            case GamePhase.Plan:
                CurrentPhase = GamePhase.Day;
                OstManager.Instance.Play("Daytime");
                return;
            case GamePhase.Day:
                CurrentPhase = GamePhase.Night;
                OstManager.Instance.Play("Nighttime");
                break;
            case GamePhase.Night:
                CurrentPhase = GamePhase.Settle;
                break;
            case GamePhase.Settle:
                CurrentPhase = GamePhase.Plan;
                OstManager.Instance.Play("Planning");
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private IEnumerator Execute()
    {
        foreach (var slot in clipboardManager.CardSlots)
        {
            var card = slot.GetComponentInChildren<ApplyCard>();
            if (!card) continue;
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
            yield return new WaitForSeconds(1f);
        }

        OstManager.Instance.Play("Nighttime");
        CurrentPhase = GamePhase.Night;
    }
}