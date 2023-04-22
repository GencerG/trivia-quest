using System;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class TriviaScoreAnimation : MonoBehaviour
{
    [SerializeField] private TMP_Text _scoreText;

    private Vector3 _initialScale;

    public void Initialize(int amount)
    {
        _scoreText.text = amount.ToString();
        _initialScale = transform.localScale;
        transform.localScale = Vector3.zero;
    }

    public void StartAnimation(float duration, Transform target, Action onComplete = null)
    {
        var offset = Vector3.up * -250f;

        var sequence = DOTween.Sequence();

        sequence.Append(transform.DOScale(_initialScale, duration / 3).SetEase(Ease.OutBack));
        sequence.AppendInterval(0.2f);
        sequence.Append(transform.DOMove(target.transform.position + offset, duration * 0.5f).SetEase(Ease.InBack));
        sequence.OnComplete(() =>
        {
            onComplete?.Invoke();
        });
    }
}
