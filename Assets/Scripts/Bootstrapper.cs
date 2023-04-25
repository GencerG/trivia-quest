using TriviaQuest.Core.Services;
using TriviaQuest.Core.ServiceScope;
using UnityEngine;

public static class Bootstrapper
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Initiailze()
    {
        var scopeManagerInstance = ScopeManager.Instance;
        var applicationScope = scopeManagerInstance.CreateScope(Scope.APPLICATION);
        Object.DontDestroyOnLoad(applicationScope.ScopeObject);
        var sceneService = applicationScope.ScopeObject.AddComponent<SceneService>();
        applicationScope.RegisterService(sceneService);

        Application.targetFrameRate = 60;
    }
}
