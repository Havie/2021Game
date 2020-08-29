using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnController : MonoBehaviour
{
    // Singleton
    public static TurnController Instance { get; private set; }

    // Parent of all the characters in the combat
    [SerializeField]
    private Transform _characterParent = null;

    // List of all the characters in the combat sorted by their morale
    private List<TurnCharacter> _characterTurns = new List<TurnCharacter>();

    // Called 0th
    private void Awake()
    {
        // Set the singleton
        if (Instance == null)
            Instance = this;
        else
        {
            Debug.LogError("There should never be more than one TurnController in the scene. Original is on " + Instance.name +
                " Extra is on " + this.name);
            Destroy(this.gameObject);
        }
    }

    private void BeginCombat()
    {

    }
}
