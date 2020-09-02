using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineManager : MonoBehaviour
{
    public static CoroutineManager Instance { get; private set; }

    private List<Coroutine> _activeCoroutines = new List<Coroutine>();
    private void Awake()
    {
        // Singleton check
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            initError();
    }
    private void initError()
    {
        Debug.Log("Multiple CoroutineManagers, Destroying this" + this.gameObject);
        Destroy(this);
    }

    /// <summary>
    /// Starts a coroutine for ScriptableObjects.
    /// </summary>
    /// <param name="_coroutine_">Corutine to start</param>
    public void StartThread(IEnumerator coroutine)
    {
        //might want to store last known 
        AddToListAndBegin(coroutine, RemoveFromList);
    }
    private void AddToListAndBegin(IEnumerator coroutine, System.Action<IEnumerator> OnComplete)
    {
        Coroutine c=(StartCoroutine(coroutine));
        //_activeCoroutines.Add(c);
    }

    //Dont think this will work , need to brainstorm 
    private void RemoveFromList(IEnumerator coroutine)
    {
        //_activeCoroutines.Remove(coroutine);
    }

}
