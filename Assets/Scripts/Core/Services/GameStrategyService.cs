using TriviaQuest.Core.GameStrategy;
using TriviaQuest.Core.ServiceScope;

namespace TriviaQuest.Core.Services
{
    public class GameStrategyService : IService
    {
        public Scope ScopeEnum => Scope.GAMEPLAY;

        private IGameStrategy _currentStrategy;

        public void Initialize(GameMode gameMode)
        {
            switch (gameMode)
            {
                default:
                case GameMode.TRIVIA_QUEST:
                    _currentStrategy = new TriviaQuestStrategy();
                    break;
            }
        }

        public int GetCorrectAnswerScore()
        {
            return _currentStrategy.GetCorrectAnswerScore();
        }

        public int GetWrongAnswerScore()
        {
            return _currentStrategy.GetWrongAnswerScore();
        }

        public bool ShouldUseTimer()
        {
            return _currentStrategy.ShouldUseTimer();
        }

        public int GetStageDuration()
        {
            return _currentStrategy.GetStageDuration();
        }

        public int GetTimeoutScore()
        {
            return _currentStrategy.GetTimeoutScore();
        }

        public bool ShouldSelectQuestionRandomly()
        {
            return _currentStrategy.ShouldSelectQuestionRandomly();
        }

        public int GetQuestionLimit()
        {
            return _currentStrategy.GetQuestionLimit();
        }


        public void Destroy()
        {

        }
    }
}

public enum GameMode
{
    TRIVIA_QUEST,
}
