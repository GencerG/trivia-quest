using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneService : MonoBehaviour, IService
{
    public Scope ScopeEnum => Scope.APPLICATION;

    private Coroutine _currentSceneChangeRoutine;
    private IScene _activeScene;
    private LoadingScreenDisplayer _loadingScreenDisplayer;

    private void Awake()
    {
        var resourceService = ScopeManager.Instance.GetService<ResourceService>(Scope.APPLICATION);
        _loadingScreenDisplayer = Instantiate(resourceService.GetPrefab<LoadingScreenDisplayer>("LoadingScreenDisplayer"), transform);
        ChangeScene(Scene.MAIN_MENU);
    }

    public void ChangeScene(Scene scene)
    {
        if (_currentSceneChangeRoutine != null)
        {
            Debug.LogWarning("Trying to change scene while there is active transition, killing current transition");

            StopCoroutine(_currentSceneChangeRoutine);
            _currentSceneChangeRoutine = null;
        }

        _currentSceneChangeRoutine = StartCoroutine(SceneChangeRoutine(scene));
    }

    private IEnumerator SceneChangeRoutine(Scene scene)
    {
        yield return _loadingScreenDisplayer.FadeIn();

        _activeScene?.Destroy();
        _activeScene = null;

        yield return null;

        SceneManager.LoadScene(GetSceneName(Scene.EMPTY));
        var asyncLoad = SceneManager.LoadSceneAsync(GetSceneName(scene));

        while(!asyncLoad.isDone)
        {
            yield return null;
        }

        _activeScene = CreateActiveScene(scene);
        yield return _activeScene.Initialize();
        yield return null;

        yield return _loadingScreenDisplayer.FadeOut();
        _currentSceneChangeRoutine = null;
    }

    private IScene CreateActiveScene(Scene scene)
    {
        switch (scene)
        {
            case Scene.GAMEPLAY:
                return new GamePlayScene();

            case Scene.MAIN_MENU:
                return new MainMenuScene();

            default: return null;
        }
    }

    private string GetSceneName(Scene scene)
    {
        switch (scene)
        {
            case Scene.EMPTY:
                return "EmptyScene";

            case Scene.GAMEPLAY:
                return "GamePlayScene";

            case Scene.MAIN_MENU:
                return "MainMenuScene";

            default: return "MainMenuScene";
        }
    }

    public void Destroy()
    {
       
    }
}

public enum Scene
{
    EMPTY,
    MAIN_MENU,
    GAMEPLAY
}
