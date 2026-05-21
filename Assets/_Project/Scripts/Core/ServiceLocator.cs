using System;
using System.Collections.Generic;
using UnityEngine;

public static class ServiceLocator
{
    private static readonly Dictionary<Type, object> _services = new Dictionary<Type, object>();

    public static void Register<T>(T service)
    {
        var type = typeof(T);
        if (!_services.ContainsKey(type)) _services.Add(type, service);
        else _services[type] = service;
    }

    public static T Get<T>()
    {
        var type = typeof(T);
        if (_services.TryGetValue(type, out var service)) return (T)service;
        Debug.LogError($"Service {type} not registered.");
        return default;
    }

    public static void Unregister<T>() { var type = typeof(T); if (_services.ContainsKey(type)) _services.Remove(type); }
    public static void Clear() { _services.Clear(); }
}
