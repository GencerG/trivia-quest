using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;

[RequireComponent(typeof(Image))]
public class AnimatedUIButton : Button
{
    public override void OnPointerClick(PointerEventData eventData)
    {
        // override to mute
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        transform.localScale *= 0.9f;
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        transform.localScale = Vector3.one;
        onClick?.Invoke();
    }
}
