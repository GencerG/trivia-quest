using TriviaQuest.Core.Scenes;
using TriviaQuest.Core.Services;
using TriviaQuest.Core.ServiceScope;
using TriviaQuest.UI;
using UnityEngine;

namespace TriviaQuest.UI
{
    public class MainMenuUI : MonoBehaviour
    {
        public void OnPlayButtonClicked()
        {
            var sceneService = ScopeManager.Instance.GetService<SceneService>(Scope.APPLICATION);
            sceneService.ChangeScene(TriviaQuestScene.GAMEPLAY);
        }

        public void OnLeaderboardButtonClicked()
        {
            var popupService = ScopeManager.Instance.GetService<PopupService>(Scope.APPLICATION);
            popupService.ShowPopup<LeaderboardPopup>();
        }
    }
}
