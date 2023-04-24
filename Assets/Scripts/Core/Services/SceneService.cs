using System.Collections;
using TriviaQuest.Core.Scenes;
using TriviaQuest.Core.ServiceScope;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TriviaQuest.Core.Services
{
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
            ChangeScene(TriviaQuestScene.MAIN_MENU);
        }

        public void ChangeScene(TriviaQuestScene scene, bool force = false)
        {
            if (_currentSceneChangeRoutine != null && force)
            {
                Debug.LogWarning("Trying to change scene while there is active transition, killing current transition");

                StopCoroutine(_currentSceneChangeRoutine);
                _currentSceneChangeRoutine = null;
            }

            if (_currentSceneChangeRoutine != null)
            {
                Debug.LogWarning("Scene transition in progress can not change scene, try force paramater");
                return;
            }

            _currentSceneChangeRoutine = StartCoroutine(SceneChangeRoutine(scene));
        }

        private IEnumerator SceneChangeRoutine(TriviaQuestScene scene)
        {
            yield return _loadingScreenDisplayer.FadeIn();

            _activeScene?.Destroy();
            _activeScene = null;

            yield return null;

            SceneManager.LoadScene(GetSceneName(TriviaQuestScene.EMPTY));
            var asyncLoad = SceneManager.LoadSceneAsync(GetSceneName(scene));

            while (!asyncLoad.isDone)
            {
                yield return null;
            }

            _activeScene = CreateActiveScene(scene);
            yield return _activeScene.Initialize();
            yield return null;

            yield return _loadingScreenDisplayer.FadeOut();
            _currentSceneChangeRoutine = null;
        }

        private IScene CreateActiveScene(TriviaQuestScene scene)
        {
            switch (scene)
            {
                case TriviaQuestScene.GAMEPLAY:
                    return new GamePlayScene();

                case TriviaQuestScene.MAIN_MENU:
                    return new MainMenuScene();

                default: return null;
            }
        }

        private string GetSceneName(TriviaQuestScene scene)
        {
            switch (scene)
            {
                case TriviaQuestScene.EMPTY:
                    return "EmptyScene";

                case TriviaQuestScene.GAMEPLAY:
                    return "GamePlayScene";

                case TriviaQuestScene.MAIN_MENU:
                    return "MainMenuScene";

                default: return "MainMenuScene";
            }
        }

        public void Destroy()
        {

        }
    }
}