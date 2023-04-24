namespace TriviaQuest.Core.GameStrategy
{
    public class TriviaQuestStrategy : IGameStrategy
    {
        public int GetCorrectAnswerScore()
        {
            return 10;
        }

        public int GetWrongAnswerScore()
        {
            return -5;
        }

        public int GetStageDuration()
        {
            return 20;
        }

        public int GetTimeoutScore()
        {
            return -3;
        }

        public bool ShouldSelectQuestionRandomly()
        {
            return false;
        }
    }
}
