using TMPro;
using UnityEngine;
using DG.Tweening;
using System;

public class TriviaScoreDisplay : MonoBehaviour
{
    [SerializeField] private Transform _parent;
    [SerializeField] private TriviaScoreAnimation _scorePrefab;
    [SerializeField] private TMP_Text _scoreText;

    private TriviaScoreAnimation _animation;
    private ScoreService _scoreService;
    private Action _onScoreAnimationsComplete;
    private int _displayedScore;
    private int _winAmount;

    public void Initialize()
    {
        _scoreService = ScopeManager.Instance.GetService<ScoreService>(Scope.GAMEPLAY);
        _scoreText.text = "0";
    }

    public void UpdateDisplay(Action onComplete)
    {
        _onScoreAnimationsComplete = onComplete;
        _winAmount = _scoreService.GetWinAmount();
        _animation = Instantiate(_scorePrefab, _parent);
        _animation.Initialize(_winAmount);
        _animation.StartAnimation(1f, _scoreText.transform, OnScoreAnimationComplete);
    }

    private void OnScoreAnimationComplete()
    {
        Destroy(_animation.gameObject);

        var sequence = DOTween.Sequence();
        sequence.Append(_scoreText.transform.DOPunchScale(Vector3.one * 1.5f, 0.1f));
        sequence.Append(DOTween.To(() => _displayedScore, amount => _displayedScore = amount, _displayedScore + _winAmount, 0.5f).OnUpdate(() =>
        {
            _scoreText.text = _displayedScore.ToString();
        }));
        sequence.OnComplete(() => _onScoreAnimationsComplete?.Invoke());
    }
}
