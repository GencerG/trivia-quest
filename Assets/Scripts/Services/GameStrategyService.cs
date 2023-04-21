public class GameStrategyService
{
    public static GameStrategyService Instance { get; } = new();

    private IGameStrategy _currentStrategy;

    public void Create(GameMode gameMode)
    {
        switch (gameMode)
        {
            case GameMode.TIRIVIA_QUEST:
                _currentStrategy = new TriviaQuestStrategy();
                break;
        }

    }

    public IGameStrategy GetCurrentStrategy()
    {
        return _currentStrategy;
    }
}

public enum GameMode
{
    TIRIVIA_QUEST,
}
