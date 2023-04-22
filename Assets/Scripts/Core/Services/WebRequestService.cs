using System;
using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;

public class WebRequestService : IService
{
    private const string QUESTION_URL = "https://magegamessite.web.app/case1/questions.json";
    private const string LEADERBOARD_PAGE0_URL = "https://magegamessite.web.app/case1/leaderboard_page_0.json";
    private const string LEADERBOARD_PAGE1_URL = "https://magegamessite.web.app/case1/leaderboard_page_1.json";

    private const int REQUEST_TIMEOUT = 2;

    public Scope ScopeEnum { get => Scope.APPLICATION; }

    public void Destroy()
    {
    }

    public IEnumerator RequestLeaderboard(Action<LeaderboardData> onComplete)
    {
        var leaderboardData = new LeaderboardData();

        yield return SendWebRequest(LEADERBOARD_PAGE0_URL, data =>
        {
            AddPage(data);
        });

        yield return SendWebRequest(LEADERBOARD_PAGE1_URL, data =>
        {
            AddPage(data);
        });

        onComplete?.Invoke(leaderboardData);

        void AddPage(string data)
        {
            try
            {
                var pageData = JsonUtility.FromJson<LeaderboardPageData>(data);
                leaderboardData.LeaderboardPageDatas.Add(pageData);
            }

            catch (Exception e)
            {
                Debug.LogError($"Error ocured while trying parse data with exception: {e}");
            }
        }
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
    }
}
