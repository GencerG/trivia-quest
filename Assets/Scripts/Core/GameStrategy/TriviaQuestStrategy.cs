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

    public bool ShouldUseTimer()
    {
        return true;
    }

    public int GetStageDuration()
    {
        return 20;
    }

    public int GetTimeoutScore()
    {
        return -3;
    }

    public int GetQuestionLimit()
    {
        return 10;
    }

    public bool ShouldSelectQuestionRandomly()
    {
        return false;
    }  
}
