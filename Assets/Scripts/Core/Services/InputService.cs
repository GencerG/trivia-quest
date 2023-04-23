using TriviaQuest.Core.ServiceScope;
using TriviaQuest.Core.UserInput;
using UnityEngine;

namespace TriviaQuest.Core.Services
{
    public class InputService : IService
    {
        public InputListener InputListener { get; private set; }
        private GamePlayInputManager _inputManager;

        public Scope ScopeEnum => Scope.GAMEPLAY;

        public void Destroy()
        {
            _inputManager.Destroy();
        }

        public void Initialize()
        {
            var inputObject = new GameObject("InputListener");
            InputListener = inputObject.AddComponent<InputListener>();
            InputListener.Initialize();

            _inputManager = new GamePlayInputManager();
        }
    }
}
