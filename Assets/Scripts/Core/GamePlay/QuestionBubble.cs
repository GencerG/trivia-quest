using UnityEngine;
using DG.Tweening;

namespace TriviaQuest.Core.Gameplay
{
    public class QuestionBubble : QuestionComponent
    {
        public SpriteRenderer BubbleSpriteRenderer;
        private Vector3 _intialScale;

        private void Awake()
        {
            _intialScale = transform.localScale;
            transform.localScale = Vector3.zero;
        }

        public override void PlayOutAnimation()
        {
            transform.DOScale(0f, 0.3f).SetEase(Ease.InBack);
        }

        public override void PlayInAnimation()
        {
            transform.DOScale(_intialScale, 0.3f).SetEase(Ease.OutBack);
        }
    }
}
