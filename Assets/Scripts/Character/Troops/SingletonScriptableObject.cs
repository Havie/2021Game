
using System.Linq;
using UnityEngine;

/// <summary>
/// Any Class dervived from this must have its Instance placed in the resources folder or it will load as NULL
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class SingletonScriptableObject<T> : ScriptableObject where T : ScriptableObject
{
    static T _instance = null;
    public static T Instance
    {
        get
        {
            if (!_instance)
                _instance = Resources.FindObjectsOfTypeAll<T>().FirstOrDefault();
            return _instance;
        }
    }
}