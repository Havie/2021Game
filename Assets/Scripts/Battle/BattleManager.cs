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



    #endregion

    #region ArtificalTestVars
    public GameObject[] chars;
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
        foreach (GameObject go in chars)
            _turnManager.AddToList(go);

        _turnManager.BeginTurn();
    }
}
