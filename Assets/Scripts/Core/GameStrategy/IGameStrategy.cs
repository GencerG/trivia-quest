public interface IGameStrategy
{
    int GetCorrectAnswerScore();

    int GetWrongAnswerScore();

    bool ShouldUseTimer();

    int GetStageDuration();

    int GetTimeoutScore();

    bool ShouldSelectQuestionRandomly();

    int GetQuestionLimit();

    int GetChoiceLimit();
}
