using System.Collections;
using System.Collections.Generic;
using TriviaQuest.Core.Gameplay;
using TriviaQuest.Core.Scenes;
using TriviaQuest.Core.ServiceScope;
using TriviaQuest.UI;
using UnityEngine;

namespace TriviaQuest.Core.Services
{
    public class TriviaService : IService
    {
        public Scope ScopeEnum => Scope.GAMEPLAY;

        private List<QuestionData> _questionDataList;
        private TriviaQuestController _questionContainer;

        private int _currentQuestionIndex;
        private bool _shouldRandom;

        public IEnumerator Initialize()
        {
            var scopeManager = ScopeManager.Instance;

            yield return scopeManager.GetService<WebRequestService>(Scope.APPLICATION).RequestQuestions(data =>
            {
                _questionDataList = data.questions;
            });

            if (_questionDataList == null || _questionDataList.Count == 0)
            {
                scopeManager.GetService<SceneService>(Scope.APPLICATION).ChangeScene(TriviaQuestScene.MAIN_MENU, true);
                yield break;
            }

            _shouldRandom = scopeManager.GetService<GameStrategyService>(Scope.GAMEPLAY).ShouldSelectQuestionRandomly();

            var questionContainerPrefab = scopeManager.GetService<ResourceService>(Scope.APPLICATION).GetPrefab<TriviaQuestController>("QuestionContainer");
            _questionContainer = Object.Instantiate(questionContainerPrefab);

            _questionContainer.Initialize(GetQuestionData());
        }

        public QuestionData RequestNextQuestion()
        {
            return GetQuestionData();
        }

        public void EndLevel(bool animate = true)
        {
            var scopeManager = ScopeManager.Instance;
            scopeManager.GetService<InputService>(Scope.GAMEPLAY).InputListener.Enable(false);
            _questionContainer.EndLevel(() =>
            {
                var popupService = scopeManager.GetService<PopupService>(Scope.APPLICATION);
                var levelEndPopup = popupService.ShowPopup<LevelEndPopup>();
                levelEndPopup.SetCloseCallback(() =>
                {
                    scopeManager.GetService<SceneService>(Scope.APPLICATION).ChangeScene(TriviaQuestScene.MAIN_MENU);
                });
            }, animate);
        }

        private QuestionData GetQuestionData()
        {
            if (_questionDataList == null || _questionDataList.Count == 0)
            {
                return null;
            }

            if (_shouldRandom)
            {
                if (_questionDataList.Count == 0)
                {
                    EndLevel();
                    return null;
                }

                var question = _questionDataList[Random.Range(0, _questionDataList.Count)];
                _questionDataList.Remove(question);
                return question;
            }
            else
            {
                if (_currentQuestionIndex >= _questionDataList.Count - 1)
                {
                    EndLevel();
                    return null;
                }

                _currentQuestionIndex++;
                return _questionDataList[Mathf.Min(_currentQuestionIndex, _questionDataList.Count - 1)];
            }
        }

        public void Destroy()
        {

        }
    }
}
