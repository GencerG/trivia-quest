using System.Collections;
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

        var strategyService = scopeManager.GetService<GameStrategyService>(Scope.GAMEPLAY);
        var currentStrategy = strategyService.Create(GameMode.TIRIVIA_QUEST);
        currentStrategy.CreateGamePlayUI();
        currentStrategy.CreateGamePlay();


        yield return null;
    }
}
