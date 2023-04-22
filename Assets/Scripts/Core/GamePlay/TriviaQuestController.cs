using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriviaQuestController : MonoBehaviour
{
    [SerializeField] private List<QuestionChoice> _choices;
    [SerializeField] private QuestionBubble _questionBubble;
    [SerializeField] private TriviaTimerDisplay _timerDisplay;
    [SerializeField] private TriviaScoreDisplay _scoreDisplay;
    [SerializeField] private TriviaCountDownTimer _countDownTimer;

    private QuestionData _currentQuestion;
    private ScopeManager _scopManager;

    public void Initialize(QuestionData questionData)
    {
        _scopManager = ScopeManager.Instance;
        _currentQuestion = questionData;

        _countDownTimer.Initialize(this);
        _countDownTimer.AddUpdateable(_timerDisplay);
        _countDownTimer.AddListener(_timerDisplay);
        _countDownTimer.StartTimer();
        _scoreDisplay.Initialize();

        UpdateTexts(questionData);
        PlayInAnimation();
    }

    public void OnChoiceClicked(QuestionChoice clickedChoice)
    {
        if (!Enum.TryParse<QuestionChoiceType>(_currentQuestion.answer, out var result))
        {
            return;
        }

        QuestionAnswer state;

        if (result.Equals(clickedChoice.ChoiceType))
        {
            state = QuestionAnswer.CORRECT;
            clickedChoice.PlayCorrectAnswerAnimation();
        }
        else
        {
            state = QuestionAnswer.WRONG;
            clickedChoice.PlayWrongAnswerAnimation();
            HighlightCorrectAnswer(result);
        }

        _scopManager.GetService<ScoreService>(Scope.GAMEPLAY).UpdateScore(state);
        StartNextQuestion();
    }

    public IEnumerator OnTimeOut()
    {
        if (!Enum.TryParse<QuestionChoiceType>(_currentQuestion.answer, out var result))
        {
            yield break;
        }

        _scopManager.GetService<ScoreService>(Scope.GAMEPLAY).UpdateScore(QuestionAnswer.TIME_OUT);
        EnableInput(false);
        HighlightCorrectAnswer(result);

        yield return new WaitForSeconds(1f);

        StartNextQuestion();
    }

    public void EnableInput(bool enable)
    {
        var inputService = _scopManager.GetService<InputService>(Scope.GAMEPLAY);
        inputService.InputListener.Enable(enable);
    }

    private void HighlightCorrectAnswer(QuestionChoiceType result)
    {
        var correctChoice = FindCorrectChoice(result);
        if (correctChoice != null)
        {
            correctChoice.PlayCorrectAnswerAnimation();
        }
    }

    private void StartNextQuestion()
    {
        _currentQuestion = _scopManager.GetService<TriviaService>(Scope.GAMEPLAY).RequestNextQuestion();

        if (_currentQuestion == null)
        {
            return;
        }

        _countDownTimer.StopTimer();

        var currentTime = 0f;
        const float startDelay = 0.8f;
        const float inOutAnimationDuration = 1.1f;
        const float inAnimationDelay = 0.1f;
        const float scoreAnimationDuration = 2.3f;

        EnableInput(false);

        var sequence = DOTween.Sequence();

        currentTime += startDelay;
        sequence.InsertCallback(currentTime, PlayOutAnimation);

        currentTime += inOutAnimationDuration;
        sequence.InsertCallback(currentTime, () => 
        {
            UpdateTexts(_currentQuestion);
            _scoreDisplay.UpdateDisplay();
        });

        currentTime += inAnimationDelay + scoreAnimationDuration;
        sequence.InsertCallback(currentTime, PlayInAnimation);

        currentTime += inOutAnimationDuration;

        sequence.InsertCallback(currentTime, () => 
        {
            _countDownTimer.StartTimer();
            EnableInput(true);
        });
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

        for (var i = _choices.Count - 1 ; i >= 0; i--)
        {
            var choice = _choices[i];
            sequence.InsertCallback((_choices.Count - i) * animationInterval, () => choice.PlayOutAnimation());
        }
    }

    private void PlayInAnimation()
    {
        const float animationInterval = 0.1f;

        var sequence = DOTween.Sequence();
        sequence.InsertCallback(0f, () => _questionBubble.PlayInAnimation());

        for (var i = 0; i < _choices.Count; i++)
        {
            var choice = _choices[i];
            sequence.InsertCallback(i * animationInterval, () => choice.PlayInAnimation());
        }
    }
}

public enum QuestionAnswer
{
    NONE,
    CORRECT,
    WRONG,
    TIME_OUT
}
