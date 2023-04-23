namespace TriviaQuest.Core.Gameplay
{
    public interface ITimerListener
    {
        void OnTimerStart(int duration);

        void OnTimerStop();
    }
}
