using System.Collections.Generic;
using UnityEngine;

public class PopupService : IService
{
    private Popup _currentPopup;
    private readonly Stack<Popup> _popupStack = new();
    private PopupLibrary _popupLibrary;

    public Scope ScopeEnum => Scope.APPLICATION;

    public PopupService()
    {
        var popupLibraryPrefab = ScopeManager.Instance.GetService<ResourceService>(Scope.APPLICATION).GetPrefab<PopupLibrary>("PopupLibrary");
        _popupLibrary = Object.Instantiate(popupLibraryPrefab);
        _popupLibrary.Initialize();
        Object.DontDestroyOnLoad(_popupLibrary.gameObject);
    }

    public T ShowPopup<T>() where T : Popup
    {
        if (_currentPopup != null && _currentPopup.GetType() != typeof(T))
        {
            _currentPopup.Hide();
        }

        var popupPrefab = _popupLibrary.GetPopupFromLibrary<T>();
        if (popupPrefab == null)
        {
            Debug.Log("Can not display popup, could not find in library");
            return null;
        }

        var popup = Object.Instantiate(popupPrefab);
        _currentPopup = popup;
        _popupStack.Push(popup);

        popup.OnPopupCreated();
        popup.PlayInAnimation();

        return popup as T;
    }

    public void ClosePopup(Popup popup)
    {
        if (popup == null || popup.Equals(_currentPopup))
        {
            Debug.Log("Can not close popup, not the latest popup");
            return;
        }

        popup.PlayOutAnimation(() =>
        {
            popup.OnPopupClosed();
            Object.Destroy(popup.gameObject);

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

    public void Destroy()
    {
    }
}
