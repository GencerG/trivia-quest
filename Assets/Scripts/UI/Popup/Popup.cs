using UnityEngine;
using DG.Tweening;
using System;

public abstract class Popup : MonoBehaviour
{
    public RectTransform Container;
    protected Action _closeCallback;

    public abstract string Name { get; }

    public abstract void OnPopupCreated();

    public virtual void Close()
    {
        ScopeManager.Instance.GetService<PopupService>(Scope.APPLICATION).ClosePopup(this);
    }

    public virtual void Hide()
    {
        gameObject.SetActive(false);
    }

    public virtual void Appear()
    {
        gameObject.SetActive(true);
    }

    public void SetCloseCallback(Action closeCallback)
    {
        _closeCallback = closeCallback;
    }

    public virtual void PlayOutAnimation(Action onComplete)
    {
        onComplete?.Invoke();
    }

    public virtual void OnPopupClosed()
    {
        _closeCallback?.Invoke();
    }

    public virtual void PlayInAnimation()
    {
        var sequence = DOTween.Sequence();

        var intialScale = Container.localScale.x;
        sequence.Append(Container.DOScale(intialScale + 0.02f, 0.12f));
        sequence.Append(Container.DOScale(intialScale, 0.1f));
    }

    private void OnValidate()
    {
        transform.name = Name;
    }
}
