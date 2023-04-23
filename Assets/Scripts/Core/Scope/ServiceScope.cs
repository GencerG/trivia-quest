using System;
using System.Collections.Generic;
using TriviaQuest.Core.Services;
using UnityEngine;

namespace TriviaQuest.Core.ServiceScope
{
    public class ServiceScope
    {
        public GameObject ScopeObject;

        private readonly Dictionary<Type, IService> _serviceDictionary = new();

        public ServiceScope(string name)
        {
            ScopeObject = new GameObject(name);
            Debug.Log($"Scope created with name: {name}");
        }

        public T GetService<T>() where T : class, IService
        {
            return _serviceDictionary.TryGetValue(typeof(T), out var value) ? (T)value : null;
        }

        public void RegisterService<T>(T type) where T : IService
        {
            _serviceDictionary.Add(typeof(T), type);
        }

        public void DeregisterService<T>() where T : class, IService
        {
            var targetService = GetService<T>();
            if (targetService != null)
            {
                targetService.Destroy();
                _serviceDictionary.Remove(typeof(T));
            }
        }

        public void DestroyScope()
        {
            foreach (var service in _serviceDictionary.Values)
            {
                service.Destroy();
            }

            UnityEngine.Object.Destroy(ScopeObject);
        }
    }
}

