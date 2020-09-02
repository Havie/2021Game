
using System.Linq;
using UnityEngine;

/// <summary>
/// Any Class dervived from this must have its Instance placed in the resources folder or it will load as NULL
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class SingletonScriptableObject<T> : ScriptableObject where T : ScriptableObject
{
    string filepath;
    static T _instance = null;
    public static T Instance
    {
        get
        {
            if (!_instance)  // TEMP FIX 
                             // _instance =(T) Resources.Load<ScriptableObject>("ScriptableSingletons/All Troops");
                             //_instance = Resources.FindObjectsOfTypeAll<T>().FirstOrDefault();
                _instance = Resources.LoadAll("ScriptableSingletons", typeof(T)).Cast<T>().FirstOrDefault();

            return _instance;
        }

    }
}