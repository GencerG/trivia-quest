using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TriviaQuest.Core.Services;
using TriviaQuest.Core.ServiceScope;
using TriviaQuest.UI;
using UnityEngine;

namespace TriviaQuest.Core.Gameplay
{
    public class TriviaQuestController : MonoBehaviour
    {
        [SerializeField] private List<QuestionChoice> _choices;
        [SerializeField] private QuestionBubble _questionBubble;
        [SerializeField] private TriviaTimerDisplay _timerDisplay;
        [SerializeField] private TriviaScoreDisplay _scoreDisplay;
        [SerializeField] private TriviaCountDownTimer _countDownTimer;

        private QuestionData _currentQuestion;
        private Sequence _currentQuestionSequence;

        private ScoreService _scoreService;
        private TriviaService _triviaService;
        private PopupService _popupService;
        private InputService _inputService;

        private bool _levelEnd;

        public void Initialize(QuestionData questionData)
        {
            var scopeManager = ScopeManager.Instance;

            _scoreService = scopeManager.GetService<ScoreService>(Scope.GAMEPLAY);
            _popupService = scopeManager.GetService<PopupService>(Scope.APPLICATION);
            _triviaService = scopeManager.GetService<TriviaService>(Scope.GAMEPLAY);
            _inputService = scopeManager.GetService<InputService>(Scope.GAMEPLAY);

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

            _scoreService.UpdateScore(state);
            StartNextQuestion();
        }

        public IEnumerator OnTimeOut()
        {
            if (!Enum.TryParse<QuestionChoiceType>(_currentQuestion.answer, out var result))
            {
                yield break;
            }

            _scoreService.UpdateScore(QuestionAnswer.TIME_OUT);
            EnableInput(false);
            HighlightCorrectAnswer(result);

            yield return new WaitForSeconds(1f);

            StartNextQuestion();
        }

        public void EnableInput(bool enable)
        {
            _inputService.InputListener.Enable(enable);
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
            _currentQuestion = _triviaService.RequestNextQuestion();

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

        public void EndLevel(Action onAnimationsComplete, bool animateLast = true)
        {
            _levelEnd = true;
            _countDownTimer.StopTimer();
            PlayLastQuestionAnimation(onAnimationsComplete, animateLast);
        }

        public void OnCloseButtonClicked()
        {
            if (_levelEnd)
            {
                return;
            }

            if (_currentQuestionSequence != null && _currentQuestionSequence.IsPlaying())
            {
                return;
            }

            _countDownTimer.PauseTimer();
            EnableInput(false);

            var warningPopup = _popupService.ShowPopup<QuitWarningPopup>();
            warningPopup.SetCloseCallback(() =>
            {
                EnableInput(true);
                _countDownTimer.UnPauseTimer();
            });
        }

        private void PlayLastQuestionAnimation(Action onComplete, bool animateLast = true)
        {
            if (!animateLast)
            {
                onComplete?.Invoke();
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

            for (var i = _choices.Count - 1; i >= 0; i--)
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
}
