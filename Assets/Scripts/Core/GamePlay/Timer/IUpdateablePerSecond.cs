namespace TriviaQuest.Core.Gameplay
{
    public interface IUpdateablePerSecond
    {
        void OnUpdate(int secondsLeft);
    }
}
