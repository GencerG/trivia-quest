using System;
using System.Collections;
using System.Text.RegularExpressions;
using TriviaQuest.Core.ServiceScope;
using UnityEngine;
using UnityEngine.Networking;

namespace TriviaQuest.Core.Services
{
    public class WebRequestService : IService
    {
        private const string QUESTION_URL = "https://magegamessite.web.app/case1/questions.json";
        private const string LEADERBOARD_URL = "localhost:8080/leaderboard?page=0";

        private const int REQUEST_TIMEOUT = 2;

        public Scope ScopeEnum { get => Scope.APPLICATION; }

        public void Destroy()
        {
        }

        public IEnumerator RequestLeaderboard(Action<LeaderboardPageData> onComplete)
        {
            yield return SendWebRequest(LEADERBOARD_URL, data =>
            {
                try
                {
                    var leaderboard = JsonUtility.FromJson<LeaderboardPageData>(data);
                    onComplete?.Invoke(leaderboard);
                }

                catch (Exception e)
                {
                    Debug.LogError($"Error ocured while trying parse data with exception: {e}");
                };
            });
        }

        public IEnumerator RequestQuestions(Action<QuestionCollectionData> onComplete)
        {
            yield return SendWebRequest(QUESTION_URL, data =>
            {
                try
                {
                    data = Regex.Replace(data, "\n", "");
                    var questions = JsonUtility.FromJson<QuestionCollectionData>(data);
                    onComplete?.Invoke(questions);
                }

                catch (Exception e)
                {
                    Debug.LogError($"Error ocured while trying parse data with exception: {e}");
                };
            });
        }

        private IEnumerator SendWebRequest(string url, Action<string> onComplete = null)
        {
            using var webData = UnityWebRequest.Get(url);
            webData.timeout = REQUEST_TIMEOUT;
            webData.SendWebRequest();

            while (!webData.isDone)
            {
                yield return null;
            }

            if (webData.result == UnityWebRequest.Result.Success)
            {
                onComplete?.Invoke(webData.downloadHandler.text);
            }
            else
            {
                Debug.LogError($"Web request fail to url: {url}, result: {webData.result}");
            }
        }
    }
}