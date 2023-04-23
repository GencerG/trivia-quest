using UnityEngine;

namespace TriviaQuest.Core.UserInput
{
    public interface IUserInput
    {
        TouchState GetTouchState();
        Vector3 GetTouchPosition();
    }
}
