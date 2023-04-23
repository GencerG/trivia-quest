using System.Collections;
using TriviaQuest.Core.Services;
using TriviaQuest.Core.ServiceScope;
using UnityEngine;

namespace TriviaQuest.Core.Scenes
{
    public class MainMenuScene : IScene
    {
        public void Destroy()
        {

        }

        public IEnumerator Initialize()
        {
            var resourceService = ScopeManager.Instance.GetService<ResourceService>(Scope.APPLICATION);
            var mainMenuUI = resourceService.GetPrefab<GameObject>("MainMenuUI");
            Object.Instantiate(mainMenuUI, Vector3.zero, Quaternion.identity);
            yield return null;
        }
    }
}
