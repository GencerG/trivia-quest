using System;
using TriviaQuest.Core.Services;
using TriviaQuest.Core.ServiceScope;
using UnityEngine;

namespace TriviaQuest.Core.UserInput
{
    public class InputListener : MonoBehaviour
    {
        public Action<Vector3> TouchStarted;
        public Action<Vector3> TouchEnded;

        private Camera _camera;
        private IUserInput _userInput;
        private bool _enabled;

        private void Awake()
        {
            _camera = ScopeManager.Instance.GetService<GamePlayCameraService>(Scope.GAMEPLAY).GamePlayCamera;

#if UNITY_EDITOR

            _userInput = new MouseInput();
#else
        

            _inputService = new TouchInput();
#endif

        }

        private void Update()
        {
            if (!_enabled) return;

            var touchState = _userInput.GetTouchState();

            switch (touchState)
            {
                case TouchState.NONE:
                    break;
                case TouchState.STARTED:
                    TouchStarted?.Invoke(_camera.ScreenToWorldPoint(GetTouchPosition()));
                    break;
                case TouchState.ENDED:
                    TouchEnded?.Invoke(_camera.ScreenToWorldPoint(GetTouchPosition()));
                    break;
            }
        }

        public Vector2 GetTouchPosition()
        {
            return _userInput.GetTouchPosition();
        }

        public void Initialize()
        {
            _enabled = true;
        }

        public void Enable(bool value)
        {
            _enabled = value;
        }
    }
}
