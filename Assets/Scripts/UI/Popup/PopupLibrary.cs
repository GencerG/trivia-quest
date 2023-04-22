using System;
using System.Collections.Generic;
using UnityEngine;

public class PopupLibrary : MonoBehaviour
{
    [SerializeField] private List<Popup> _popupList;
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
    }

    public T GetPopupFromLibrary<T>() where T : Popup
    {
        return _popupDictionary.TryGetValue(typeof(T).Name, out var popup) ? (T)popup : null;
    }
    
}
