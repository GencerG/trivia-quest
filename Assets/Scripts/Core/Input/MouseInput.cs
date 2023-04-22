using UnityEngine;

public class MouseInput : IUserInput
{
    public Vector3 GetTouchPosition()
    {
        return Input.mousePosition;
    }

    public TouchState GetTouchState()
    {
        if (Input.GetMouseButtonDown(0))
            return TouchState.STARTED;

        if (Input.GetMouseButtonUp(0))
            return TouchState.ENDED;

        return TouchState.NONE;
    }
}
