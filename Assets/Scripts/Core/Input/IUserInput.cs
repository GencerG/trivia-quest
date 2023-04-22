using UnityEngine;

public interface IUserInput
{
    TouchState GetTouchState();
    Vector3 GetTouchPosition();
}
