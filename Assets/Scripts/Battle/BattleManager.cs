using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    // Singleton
    public static BattleManager Instance { get; private set; }

    #region Variables
    // Reference to the TurnManager
    private TurnManager _turnManager;

    // Change these to a smaller obj type once we know use
    private List<GameObject> _darkelves;
    private List<GameObject> _orcs;
    #endregion

    #region ArtificalTestVars
    public GameObject[] _chars;
    #endregion

    // Called 0th
    private void Awake()
    {
        // Get Singleton
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
        {
            Debug.LogError("There should never be two BattleManagers in the scene. Original is " 
                + Instance.name + ". A second one was found on " + this.name);
            Destroy(this.gameObject);
        }

        // Make this object persist
        DontDestroyOnLoad(this.gameObject);
    }
    
    // Called 1st
    private void Start()
    {
        // Create lists
        _darkelves = new List<GameObject>();
        _orcs = new List<GameObject>();

        // Iterate over the gameobjects to sort into orcs and elves
        foreach (GameObject go in _chars)
        {
            // Make sure its not null
            if (go)
            {
                iniCharacter initChar = go.GetComponent<iniCharacter>();
                // Initialize the character is they exist
                if (initChar)
                    initChar.Init();
                else
                    Debug.LogError("Could not find iniCharacter attached to " + go.name);

                // Get teh faction
                Faction f = go.GetComponent<Faction>();
                if (f)
                {
                    if (f.IsHuman())
                        _darkelves.Add(go);
                    else
                        _orcs.Add(go);
                }
            }
            else
                Debug.LogError("Null GameObject in BattleManager");
        }

        _turnManager = new TurnManager(true);
        TEST();
    }

    // Called when the component is destroyed.
    private void OnDestroy()
    {
        _turnManager.Subscribe(false);
    }

    // Called every frame
    private void Update()
    {
        // TEMP for testing
        if (Input.GetKeyDown(KeyCode.N))
            _turnManager.Next();
    }

    /// <summary>
    /// Starts the battle as a test
    /// </summary>
    private void TEST()
    {
        StartBattle();
    }

    /// <summary>
    /// Starts the battle by adding the charactes to the turn manager.
    /// </summary>
    public void StartBattle()
    {
        // Add all characters to the turn manager.
        foreach (GameObject go in _chars)
            _turnManager.AddToList(go);

        // If there are characters, start the first turn.
        if (_chars.Length != 0)
            _turnManager.BeginNewTurn();

        _turnManager.Subscribe(true);
    }

    /// <summary>
    /// Returns the list of forces depending on the bool. Return Elves is true, Orcs if false.
    /// </summary>
    /// <param name="isHuman">Which force to return.</param>
    /// <returns>List<GameObject></returns>
    public List<GameObject> GetForceList(bool isHuman)
    {
        if (isHuman)
            return _darkelves;
        else
            return _orcs;
    }
}
