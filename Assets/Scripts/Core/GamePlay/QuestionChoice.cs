using UnityEngine;
using DG.Tweening;

public class QuestionChoice : QuestionComponent
{
    [SerializeField] private SpriteRenderer _backgroundSpriteRenderer;

    public void PlayCorrectAnswerAnimation()
    {
        _backgroundSpriteRenderer.DOColor(Color.green, 0.3f);
    }

    public void PlayWrongAnswerAnimation()
    {
        _backgroundSpriteRenderer.DOColor(Color.red, 0.3f);
    }
}
