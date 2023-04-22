using UnityEngine;
using DG.Tweening;

public class QuestionChoice : QuestionComponent
{
    public QuestionChoiceType ChoiceType;

    [SerializeField] private SpriteRenderer _backgroundSpriteRenderer;

    private const float MOVE_ANIMATION_DURATION = 0.4f;

    public override void PlayInAnimation()
    {
        _backgroundSpriteRenderer.color = Color.white;
        transform.localPosition = new Vector3(-5f, transform.localPosition.y, transform.localPosition.z);
        transform.DOLocalMoveX(0f, MOVE_ANIMATION_DURATION).SetEase(Ease.OutBack);
    }

    public override void PlayOutAnimation()
    {
        transform.DOLocalMoveX(5f, MOVE_ANIMATION_DURATION).SetEase(Ease.InBack);
    }

    public void PlayCorrectAnswerAnimation()
    {
        _backgroundSpriteRenderer.DOColor(Color.green, .3f);
    }

    public void PlayWrongAnswerAnimation()
    {
        _backgroundSpriteRenderer.DOColor(Color.red, .3f);
    }
}

public enum QuestionChoiceType
{
    A,
    B,
    C,
    D
}
