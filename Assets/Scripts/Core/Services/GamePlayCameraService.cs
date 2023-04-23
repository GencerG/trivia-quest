using TriviaQuest.Core.ServiceScope;
using UnityEngine;

namespace TriviaQuest.Core.Services
{
    public class GamePlayCameraService : IService
    {
        public float Width { get; private set; }
        public float Height { get; private set; }
        public Camera GamePlayCamera { get; private set; }

        public Scope ScopeEnum => Scope.GAMEPLAY;

        private const float DESIRED_WIDTH = 5.8f;


        public void Initialize()
        {
            GamePlayCamera = Camera.main;

            Height = GamePlayCamera.orthographicSize * 2f;
            Width = GamePlayCamera.aspect * Height;

            if (Width < DESIRED_WIDTH)
            {
                Debug.Log("Recalculating ortographic size");
                GamePlayCamera.orthographicSize = DESIRED_WIDTH / (GamePlayCamera.aspect * 2f);

                Height = GamePlayCamera.orthographicSize * 2f;
                Width = GamePlayCamera.aspect * Height;
            }
        }

        public void Destroy()
        {
            
        }
    }
}
