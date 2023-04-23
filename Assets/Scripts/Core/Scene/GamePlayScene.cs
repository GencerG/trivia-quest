using System.Collections;
using TriviaQuest.Core.Services;
using TriviaQuest.Core.ServiceScope;

namespace TriviaQuest.Core.Scenes
{
    public class GamePlayScene : IScene
    {
        public void Destroy()
        {
            ScopeManager.Instance.DestroyScope(Scope.GAMEPLAY);
        }

        public IEnumerator Initialize()
        {
            var scopeManager = ScopeManager.Instance;
            scopeManager.CreateScope(Scope.GAMEPLAY);

            yield return null;

            scopeManager.GetService<GameStrategyService>(Scope.GAMEPLAY).Initialize(GameMode.TRIVIA_QUEST);
            scopeManager.GetService<InputService>(Scope.GAMEPLAY).Initialize();

            yield return scopeManager.GetService<TriviaService>(Scope.GAMEPLAY).Initialize();

            yield return null;
        }
    }
}
