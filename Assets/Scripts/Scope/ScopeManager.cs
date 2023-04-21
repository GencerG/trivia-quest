using System;
using UnityEngine;

public class ScopeManager
{
    public static ScopeManager Instance { get; } = new();

    private ServiceScope _applicationScope;
    private ServiceScope _gamePlayScope;
    private ServiceScope _nullScope;

    public ServiceScope CreateScope(Scope scope)
    {
         ref var serviceScope = ref GetScopeByEnum(scope);
        serviceScope = new ServiceScope(scope.ToString());
        return serviceScope;
    }

    public void DestroyScope(Scope scope)
    {
        ref var serviceScope = ref GetScopeByEnum(scope);
        DestroyScopeInternal(ref serviceScope);
    }

    public T GetService<T>(Scope scope) where T : class, IService
    {
        if (!IsScopeActive(scope)) return null;

        ref var serviceScope = ref GetScopeByEnum(scope);
        var targetService = serviceScope.GetService<T>();
        if (targetService == null)
        {
            targetService = (T)Activator.CreateInstance(typeof(T));

            if (targetService.ScopeEnum.Equals(scope))
            {
                RegisterService(targetService);
                return targetService;
            }
        }

        return targetService;
    }

    public void RegisterService<T>(T type) where T : IService
    {
   
        ref var serviceScope = ref GetScopeByEnum(type.ScopeEnum);
        serviceScope.RegisterService(type);
    }

    public void DeregisterService<T>(Scope scope) where T : class, IService
    {
        ref var serviceScope = ref GetScopeByEnum(scope);
        serviceScope.DeregisterService<T>();
    }

    private ref ServiceScope GetScopeByEnum(Scope scope)
    {
        switch (scope)
        {
            case Scope.APPLICATION:
                return ref _applicationScope;

            case Scope.GAMEPLAY:
                return ref _gamePlayScope;

            default: return ref _nullScope;
        }
    }

    private void DestroyScopeInternal(ref ServiceScope scope)
    {
        Debug.Log($"Destroying Scope: {scope.GetType().Name}");
        scope.DestroyScope();
        scope = null;
    }

    private bool IsScopeActive(Scope scope)
    {
        ref var serviceScope = ref GetScopeByEnum(scope);

        return serviceScope != null && !serviceScope.Equals(_nullScope);
    }
}

public enum Scope
{
    APPLICATION,
    GAMEPLAY
}