using System.Collections;

namespace TriviaQuest.Core.Scenes
{
    public interface IScene
    {
        IEnumerator Initialize();
        void Destroy();
    }
}
