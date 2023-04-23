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
    private Sequence _currentQuestionSequence;

    private bool _sequencePaused;
    private bool _levelEnd;

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
        const float scoreAnimationDuration = 1.76f;

        EnableInput(false);

        _currentQuestionSequence = DOTween.Sequence();

        currentTime += startDelay;
        _currentQuestionSequence.InsertCallback(currentTime, PlayOutAnimation);

        currentTime += inOutAnimationDuration;
        _currentQuestionSequence.InsertCallback(currentTime, () => 
        {
            UpdateTexts(_currentQuestion);
            _scoreDisplay.UpdateDisplay(null);
        });

        currentTime += inAnimationDelay + scoreAnimationDuration;
        _currentQuestionSequence.InsertCallback(currentTime, PlayInAnimation);

        currentTime += inOutAnimationDuration;
        _currentQuestionSequence.InsertCallback(currentTime, () => 
        {
            _countDownTimer.StartTimer();
            EnableInput(true);
        });

        _currentQuestionSequence.OnComplete(() => _currentQuestionSequence = null);
    }

    public void EndLevel(Action onAnimationsComplete)
    {
        _levelEnd = true;
        _countDownTimer.StopTimer();
        PlayLastQuestionAnimation(onAnimationsComplete);
    }

    public void OnCloseButtonClicked()
    {
        if (_levelEnd)
        {
            return;
        }

        _countDownTimer.PauseTimer();
        EnableInput(false);

        if (_currentQuestionSequence != null && _currentQuestionSequence.IsPlaying())
        {
            _sequencePaused = true;
            _currentQuestionSequence.Pause();
        }

        var warningPopup = _scopManager.GetService<PopupService>(Scope.APPLICATION).ShowPopup<QuitWarningPopup>();
        warningPopup.SetCloseCallback(() => 
        {
            _countDownTimer.UnPauseTimer();
            if (_sequencePaused)
            {
                _currentQuestionSequence.Play();
                _sequencePaused = false;
            }
            else
            {
                EnableInput(true);
            }
        });
    }

    private void PlayLastQuestionAnimation(Action onComplete)
    {
        if (_currentQuestionSequence != null && _sequencePaused)
        {
            _currentQuestionSequence.Play();
            _sequencePaused = false;
            _currentQuestionSequence.OnComplete(() => onComplete?.Invoke());
            return;
        }

        var currentTime = 0f;
        const float inOutAnimationDuration = 1.1f;

        var sequence = DOTween.Sequence();
        sequence.InsertCallback(currentTime, PlayOutAnimation);

        currentTime += inOutAnimationDuration;
        sequence.InsertCallback(currentTime, () =>
        {
            _scoreDisplay.UpdateDisplay(onComplete);
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
        if (_levelEnd)
        {
            return;
        }

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
