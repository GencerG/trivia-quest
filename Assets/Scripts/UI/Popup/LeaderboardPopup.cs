using TriviaQuest.Core.Services;
using TriviaQuest.Core.ServiceScope;
using UnityEngine;

namespace TriviaQuest.UI
{
    public class LeaderboardPopup : Popup
    {
        public override string Name => GetType().Name;

        [SerializeField] private LeaderboardEntry _leaderboardEntry;
        [SerializeField] private RectTransform _content;

        public override void OnPopupCreated()
        {
            var webService = ScopeManager.Instance.GetService<WebRequestService>(Scope.APPLICATION);
            StartCoroutine(webService.RequestLeaderboard(CreateLeaderboard));
        }

        private void CreateLeaderboard(LeaderboardPageData data)
        {
            foreach (var player in data.data)
            {
                var entry = Instantiate(_leaderboardEntry, _content);
                entry.Initialize(player);
            }
        }
    }
}
    