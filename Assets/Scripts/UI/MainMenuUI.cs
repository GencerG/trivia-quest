using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    public void OnPlayButtonClicked()
    {
       var sceneService =  ScopeManager.Instance.GetService<SceneService>(Scope.APPLICATION);
        sceneService.ChangeScene(Scene.GAMEPLAY);
    }

    public void OnLeaderboardButtonClicked()
    {

    }
}
