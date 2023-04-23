using TriviaQuest.Core.ServiceScope;

namespace TriviaQuest.Core.Services
{
    public interface IService
    {
        Scope ScopeEnum { get; }
        void Destroy();
    }
}
