using System;
using System.Threading.Tasks;
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

    public async Task RequestLeaderboard(Action<LeaderboardData> onComplete)
    {
        var leaderboardData = new LeaderboardData();

        await SendWebRequest(LEADERBOARD_PAGE0_URL, data =>
        {
            AddPage(data);
        });

        await SendWebRequest(LEADERBOARD_PAGE1_URL, data =>
        {
            AddPage(data);
        });

        onComplete?.Invoke(leaderboardData);

        void AddPage(string data)
        {
            var pageData = JsonUtility.FromJson<LeaderboardPageData>(data);
            leaderboardData.LeaderboardPageDatas.Add(pageData);
        }
    }

    public async Task RequestQuestions(Action<QuestionCollectionData> onComplete)
    {
        await SendWebRequest(QUESTION_URL, data =>
        {
            var questions = JsonUtility.FromJson<QuestionCollectionData>(data);
            onComplete?.Invoke(questions);
        });
    }

    private async Task SendWebRequest(string url, Action<string> onComplete = null)
    {
        try
        {
            using var webData = UnityWebRequest.Get(url);
            webData.timeout = REQUEST_TIMEOUT;
            webData.SendWebRequest();

            while (!webData.isDone)
            {
                await Task.Yield();
            }

            if (webData.result == UnityWebRequest.Result.Success)
            {
                onComplete?.Invoke(webData.downloadHandler.text);
            }
        }

        catch(Exception e)
        {
            Debug.LogError($"Error while trying to send web request with exception: {e}");
        }
    }
}
