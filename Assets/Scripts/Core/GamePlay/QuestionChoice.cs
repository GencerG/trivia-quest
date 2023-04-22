using UnityEngine;
using DG.Tweening;

public class QuestionChoice : QuestionComponent
{
    public QuestionChoiceType ChoiceType;

    [SerializeField] private SpriteRenderer _backgroundSpriteRenderer;

    private const float MOVE_ANIMATION_DURATION = 0.4f;
    private const float CHOICE_WIDTH = 1.5f;

    private float _outPosition;
    private float _inPosition;

    private void Awake()
    {
        const int pixelsPerUnit = 100;
        var screenWidth = Screen.width / pixelsPerUnit;
        _outPosition = screenWidth * 0.5f + CHOICE_WIDTH;
        _inPosition = _outPosition * -1f;
        transform.localPosition = new Vector3(_inPosition, transform.localPosition.y, transform.localPosition.z);
    }

    public override void PlayInAnimation()
    {
        _backgroundSpriteRenderer.color = Color.white;
        transform.localPosition = new Vector3(_inPosition, transform.localPosition.y, transform.localPosition.z);
        transform.DOLocalMoveX(0f, MOVE_ANIMATION_DURATION).SetEase(Ease.OutBack);
    }

    public override void PlayOutAnimation()
    {
        transform.DOLocalMoveX(_outPosition, MOVE_ANIMATION_DURATION).SetEase(Ease.InBack);
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
