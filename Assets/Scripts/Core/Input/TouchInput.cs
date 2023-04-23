using UnityEngine;

namespace TriviaQuest.Core.UserInput
{
    public class TouchInput : IUserInput
    {
        public Vector3 GetTouchPosition()
        {
            return Input.GetTouch(0).position;
        }

        public TouchState GetTouchState()
        {
            if (Input.touchCount <= 0) return TouchState.NONE;

            var touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    return TouchState.STARTED;

                case TouchPhase.Ended:
                    return TouchState.ENDED;
            }

            return TouchState.NONE;
        }
    }
}
