using System.IO;
using UnityEngine;

public class ResourceService : IService
{
    public Scope ScopeEnum => Scope.APPLICATION;

    private const string PREFAB_PREFIX = "Prefabs";

    public T GetPrefab<T>(string suffix) where T : Object
    {
        return Resources.Load<T>(Path.Combine(PREFAB_PREFIX, suffix));
    }

    public void Destroy()
    {
        Resources.UnloadUnusedAssets();
    }
}
