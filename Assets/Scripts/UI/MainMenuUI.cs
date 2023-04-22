using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    public void OnPlayButtonClicked()
    {
          var sceneService =  ScopeManager.Instance.GetService<SceneService>(Scope.APPLICATION);
           sceneService.ChangeScene(Scene.GAMEPLAY);
        
        /*
        var popupService = ScopeManager.Instance.GetService<PopupService>(Scope.APPLICATION);
        popupService.ShowPopup<QuitWarningPopup>();
        */
    }

    public void OnLeaderboardButtonClicked()
    {
        var popupService = ScopeManager.Instance.GetService<PopupService>(Scope.APPLICATION);
        popupService.ShowPopup<LeaderboardPopup>();
    }
}
