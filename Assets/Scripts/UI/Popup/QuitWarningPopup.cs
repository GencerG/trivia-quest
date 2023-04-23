using TriviaQuest.Core.Services;
using TriviaQuest.Core.ServiceScope;

namespace TriviaQuest.UI
{
    public class QuitWarningPopup : Popup
    {
        public override string Name => GetType().Name;

        public override void OnPopupCreated()
        {

        }

        public void OnContinueClicked()
        {
            _closeCallback = null;
            Close();
            ScopeManager.Instance.GetService<TriviaService>(Scope.GAMEPLAY).EndLevel(false);
        }
    }
}
