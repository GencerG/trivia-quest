using UnityEngine;
using UnityEngine.Events;

public class SpriteButton : MonoBehaviour
{
    public UnityEvent OnClick;

    public void ButtonDown()
    {
        transform.localScale = new Vector3(0.94f, 0.94f, 0.94f);
    }

    public void ButtonUp(bool shouldTrigger)
    {
        transform.localScale = Vector3.one;
        if (shouldTrigger)
        {
            OnClick?.Invoke();
        }
    }
}
