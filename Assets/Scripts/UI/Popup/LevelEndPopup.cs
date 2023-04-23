using TMPro;
using TriviaQuest.Core.Services;
using TriviaQuest.Core.ServiceScope;
using UnityEngine;

namespace TriviaQuest.UI
{
    public class LevelEndPopup : Popup
    {
        [SerializeField] private TMP_Text _scoreText;

        public override string Name => GetType().Name;

        public override void OnPopupCreated()
        {
            var score = ScopeManager.Instance.GetService<ScoreService>(Scope.GAMEPLAY).GetCurrentScore();
            _scoreText.text = $"Your Score: {score}";
        }
    }
}
