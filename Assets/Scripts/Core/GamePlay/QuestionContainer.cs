using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

public class QuestionContainer : MonoBehaviour
{
    [SerializeField] private List<QuestionChoice> _choices;
    [SerializeField] private QuestionBubble _questionBubble;

    private QuestionData _currentQuestion;
    private ScopeManager _scopManager;

    public void Initialize(QuestionData questionData)
    {
        _scopManager = ScopeManager.Instance;
        _currentQuestion = questionData;
        UpdateTexts(questionData);
    }

    public void OnChoiceClicked(QuestionChoice clickedChoice)
    {
        if (Enum.TryParse<QuestionChoiceType>(_currentQuestion.answer, out var result))
        {
            var isCorrect = false;

            if (result.Equals(clickedChoice.ChoiceType))
            {
                isCorrect = true;
                clickedChoice.PlayCorrectAnswerAnimation();
            }
            else
            {
                isCorrect = false;
                clickedChoice.PlayWrongAnswerAnimation();
                var correctChoice = FindCorrectChoice(result);
                if (correctChoice != null)
                {
                    correctChoice.PlayCorrectAnswerAnimation();
                }
            }

            StartNextQuestion();
        }
    }

    private void StartNextQuestion()
    {
        _currentQuestion = _scopManager.GetService<TriviaService>(Scope.GAMEPLAY).RequestNextQuestion();

        if (_currentQuestion == null)
        {
            return;
        }

        var currentTime = 0f;
        const float startDelay = 0.5f;
        const float inOutAnimationDuration = 0.8f;
        const float inAnimationDelay = 0.1f;

        var inputService = ScopeManager.Instance.GetService<InputService>(Scope.GAMEPLAY);
        inputService.InputListener.Enable(false);

        var sequence = DOTween.Sequence();

        currentTime += startDelay;
        sequence.InsertCallback(currentTime, PlayOutAnimation);

        currentTime += inOutAnimationDuration;
        sequence.InsertCallback(currentTime, () => UpdateTexts(_currentQuestion));

        currentTime += inAnimationDelay;
        sequence.InsertCallback(currentTime, PlayInAnimation);

        currentTime += inOutAnimationDuration;

        sequence.InsertCallback(currentTime, () => inputService.InputListener.Enable(true));
    }

    private QuestionChoice FindCorrectChoice(QuestionChoiceType correctAnswer)
    {
        foreach (var choice in _choices)
        {
            if (choice.ChoiceType == correctAnswer)
            {
                return choice;
            }
        }

        return null;
    }

    private void UpdateTexts(QuestionData data)
    {
        _questionBubble.UpdateText(data.question);

        for (var i = 0; i < _choices.Count; i++)
        {
            _choices[i].UpdateText(data.choices[i]);
        }
    }

    private void PlayOutAnimation()
    {
        const float animationInterval = 0.1f;
        const float moveAnimationDuration = 0.4f;

        var sequence = DOTween.Sequence();
        sequence.InsertCallback(_choices.Count * animationInterval + moveAnimationDuration, () => _questionBubble.PlayOutAnimation());

        for (var i = _choices.Count -1 ; i >= 0; i--)
        {
            sequence.InsertCallback((_choices.Count - i) * animationInterval, () => _choices[i].PlayOutAnimation());
        }
    }

    private void PlayInAnimation()
    {
        const float animationInterval = 0.1f;

        var sequence = DOTween.Sequence();
        sequence.InsertCallback(0f, () => _questionBubble.PlayInAnimation());

        for (var i = 0; i < _choices.Count; i++)
        {
            sequence.InsertCallback(i * animationInterval, () => _choices[i].PlayInAnimation());
        }
    }
}
