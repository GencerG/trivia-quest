using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriviaService :  IService
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
            scopeManager.GetService<SceneService>(Scope.APPLICATION).ChangeScene(Scene.MAIN_MENU);
            yield break;
        }
        
        _shouldRandom = scopeManager.GetService<GameStrategyService>(Scope.GAMEPLAY).ShouldSelectQuestionRandomly();

        var questionContainerPrefab = scopeManager.GetService<ResourceService>(Scope.APPLICATION).GetPrefab<TriviaQuestController>("QuestionContainer");
        _questionContainer = UnityEngine.Object.Instantiate(questionContainerPrefab);

        _questionContainer.Initialize(GetQuestionData());
    }

    public QuestionData RequestNextQuestion()
    {
        return GetQuestionData();
    }

    private QuestionData GetQuestionData()
    {
        if (_questionDataList == null || _questionDataList.Count == 0)
        {
            return null;
        }

        if (_shouldRandom)
        {
            var question = _questionDataList[UnityEngine.Random.Range(0, _questionDataList.Count)];
            _questionDataList.Remove(question);
            return question;
        }
        else
        {
            return _questionDataList[Mathf.Min(_currentQuestionIndex++, _questionDataList.Count - 1)];
        }
    }

    public void Destroy()
    {

    }
}
