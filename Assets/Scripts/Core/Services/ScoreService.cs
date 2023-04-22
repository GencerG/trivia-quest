public class ScoreService : IService
{
    public Scope ScopeEnum => Scope.GAMEPLAY;

    private int _currentScore;
    private int _winAmount;

    public void UpdateScore(QuestionAnswer answer)
    {
        var gameStrategyService = ScopeManager.Instance.GetService<GameStrategyService>(Scope.GAMEPLAY);

        switch (answer)
        {
            case QuestionAnswer.CORRECT:
                _winAmount = gameStrategyService.GetCorrectAnswerScore();
                break;

            case QuestionAnswer.WRONG:
                _winAmount = gameStrategyService.GetWrongAnswerScore();
                break;

            case QuestionAnswer.TIME_OUT:
                _winAmount = gameStrategyService.GetTimeoutScore();
                break;

            default:
                break;
        }

        _currentScore += _winAmount;
    }

    public int GetCurrentScore()
    {
        return _currentScore;
    }

    public int GetWinAmount()
    {
        return _winAmount;
    }

    public void Destroy()
    {
       
    }
}
