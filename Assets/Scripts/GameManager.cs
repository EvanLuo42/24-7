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
    Settle
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
                initEffect.ApplyEffect();
                tableManager.GenerateObjects(3);
                Continue();
                break;
            case GamePhase.Plan:
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    Continue();
                }
                break;
            case GamePhase.Day:
                StartCoroutine(Execute());
                Continue();
                break;
            case GamePhase.Night:
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    Continue();
                }
                break;
            case GamePhase.Settle:
                tableManager.GenerateObjects(Random.Range(1, 3));
                Continue();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void Continue()
    {
        switch (CurrentPhase)
        {
            case GamePhase.Start:
                CurrentPhase = GamePhase.Plan;
                return;
            case GamePhase.Plan:
                CurrentPhase = GamePhase.Day;
                return;
            case GamePhase.Day:
                CurrentPhase = GamePhase.Night;
                break;
            case GamePhase.Night:
                CurrentPhase = GamePhase.Settle;
                break;
            case GamePhase.Settle:
                CurrentPhase = GamePhase.Plan;
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
            yield return new WaitForSeconds(0.5f);
            yield return rect.DOLocalMove(originalPos, 0.2f)
                .SetEase(Ease.InQuad)
                .WaitForCompletion();
            yield return new WaitForSeconds(0.5f);
        }
    }
}