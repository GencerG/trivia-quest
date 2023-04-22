using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupLibrary : MonoBehaviour
{
    [SerializeField] private List<Popup> _popupList;
    [SerializeField] private Image _popupBackground;

    private const float BACKGROUND_ALPHA = 0.86f;
    private const float ANIMATION_DURATION = 0.1f;

    private Dictionary<string, Popup> _popupDictionary;

    public void Initialize()
    {
        if (_popupList.Count == 0)
        {
            return;
        }

        _popupDictionary = new Dictionary<string, Popup>();

        foreach (var popup in _popupList)
        {
            _popupDictionary.TryAdd(popup.Name, popup);
        }

        _popupList.Clear();
        _popupList = null;

        var color = _popupBackground.color;
        color.a = 0f;
        _popupBackground.color = color;
    }

    public T GetPopupFromLibrary<T>() where T : Popup
    {
        return _popupDictionary.TryGetValue(typeof(T).Name, out var popup) ? (T)popup : null;
    }

    public void ShowPopupBackground()
    {
        _popupBackground.DOFade(BACKGROUND_ALPHA, ANIMATION_DURATION);
        _popupBackground.raycastTarget = true;
    }

    public void HidePopupBackground()
    {
        _popupBackground.DOFade(0f, ANIMATION_DURATION);
        _popupBackground.raycastTarget = false;
    }
}
