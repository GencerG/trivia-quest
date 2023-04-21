using System.Collections.Generic;
using UnityEngine;

public class PopupService
{
    public static PopupService Instance { get; } = new();

    private Popup _currentPopup;
    private readonly Stack<Popup> _popupStack = new();

    public T ShowPopup<T>() where T : Popup
    {
        if (_currentPopup != null && _currentPopup.GetType() != typeof(T))
        {
            _currentPopup.Hide();
        }

        var temp = new GameObject();
        var temp2 = temp.GetComponent<Popup>();
        _currentPopup = temp2;
        _popupStack.Push(temp2);

        temp2.OnPopupCreated();
        temp2.PlayInAnimation();

        return temp2 as T;
    }

    public void CloseCurrentPopup()
    {
        _currentPopup.PlayOutAnimation(() =>
        {
            _currentPopup.OnPopupClosed();
            Object.Destroy(_currentPopup.gameObject);

            Popup previousPopup = null;

            if (_popupStack.Count > 0)
            {
                previousPopup = _popupStack.Pop();
            }

            if (previousPopup != null)
            {
                previousPopup.Appear();
            }

            _currentPopup = previousPopup;
        });
    }

    public void KillAllPopups()
    {
        foreach (var popup in _popupStack)
        {
            Object.Destroy(popup);
        }

        _popupStack.Clear();
    }
}
