using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class QuestionChoiceContainer : MonoBehaviour
{
    [SerializeField] private QuestionChoice _questionChoicePrefab;

    private readonly List<QuestionChoice> _choices = new();

    private void Start()
    {
        Initialize();
    }

    public void Test()
    {
        DataService.Instance.RequestLeaderboard(data =>
        {
            Debug.Log(data);
            Debug.Log(data);
        });
    }

    public void Initialize()
    {
        GameStrategyService.Instance.Create(GameMode.TIRIVIA_QUEST);
        var choiceLimit = GameStrategyService.Instance.GetCurrentStrategy().GetChoiceLimit();

        for (var i = 0; i < choiceLimit; i++)
        {
            var choice = Instantiate(_questionChoicePrefab, transform);
            choice.transform.localPosition = -i * Vector3.up;
            choice.UpdateText("ffasf");
            _choices.Add(choice);
        }
    }

    public Sequence StartOutAnimation()
    {
        var sequence = DOTween.Sequence();

        for (var i = _choices.Count - 1; i >= 0; i--)
        {
            var choice = _choices[i];
            sequence.Insert((_choices.Count - 1 - i) * 0.1f, choice.transform.DOLocalMoveX(5f, 0.4f).SetEase(Ease.InQuint));
        }

        return sequence;
    }
}
