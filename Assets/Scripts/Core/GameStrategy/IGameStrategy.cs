namespace TriviaQuest.Core.GameStrategy
{
    public interface IGameStrategy
    {
        int GetCorrectAnswerScore();

        int GetWrongAnswerScore();

        int GetStageDuration();

        int GetTimeoutScore();

        bool ShouldSelectQuestionRandomly();
    }
}
