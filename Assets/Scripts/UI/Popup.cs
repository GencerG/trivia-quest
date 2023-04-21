using UnityEngine;
using DG.Tweening;
using System;

public abstract class Popup : MonoBehaviour
{

    public virtual void OnPopupClosed()
    {

    }

    public virtual void OnPopupCreated()
    {

    }

    public virtual void Close()
    {

    }

    public virtual void Hide()
    {
        gameObject.SetActive(false);
    }

    public virtual void Appear()
    {
        gameObject.SetActive(true);
    }

    public virtual void PlayOutAnimation(Action onComplete)
    {
        onComplete?.Invoke();
    }

    public virtual void PlayInAnimation()
    {
        transform.localScale = Vector3.zero;
        transform.DOScale(1f, 0.3f).SetEase(Ease.OutBack);
    }
}
