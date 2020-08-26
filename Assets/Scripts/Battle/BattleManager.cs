using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    //Singleton
    static BattleManager _instance;
    public static BattleManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = GameObject.FindObjectOfType<BattleManager>();
            return _instance;
        }
    }

    #region Variables
    TurnManager _turnManager;

    //Change these to a smaller obj type once we know use
    private List<GameObject> _darkelves;
    private List<GameObject> _orcs;


    #endregion

    #region ArtificalTestVars
    public GameObject[] _chars;
    #endregion


    void Awake()
    {
        if (_instance == null)
            _instance = this;
        else if(_instance!=this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }
    
    void Start()
    {
        _darkelves = new List<GameObject>();
        _orcs = new List<GameObject>();
        foreach (GameObject go in _chars)
        {
            if (go)
            {
                iniCharacter initChar = go.GetComponent<iniCharacter>();
                if (initChar)
                    initChar.Init();
                else
                    Debug.LogError("Could not find iniCharacter attached to " + go.name);

                Faction f = go.GetComponent<Faction>();
                if(f)
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

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.N))
            _turnManager.Next();
    }

    private void TEST()
    {
        StartBattle();
    }

    public void StartBattle()
    {
        foreach (GameObject go in _chars)
            _turnManager.AddToList(go);

        if (_chars.Length != 0)
            _turnManager.BeginNewTurn();
        _turnManager.Subscribe(true);
    }

     void OnDestroy()
    {
        _turnManager.Subscribe(false);
    }

    public List<GameObject> GetForceList(bool isHuman)
    {
        if (isHuman)
            return _darkelves;
        else
            return _orcs;
    }
}
