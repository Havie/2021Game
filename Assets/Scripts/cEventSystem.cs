using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class cEventSystem : MonoBehaviour
{
   private static cEventSystem _instance;



    public delegate void BattleRoundEnd();
    public event BattleRoundEnd ABR;

    public delegate void CharacterTurnEnd();
    public event CharacterTurnEnd ACT;


    public delegate void characterDied(Officer officer);
    public event characterDied characterDead; 

                                       
	#region Singleton
    public static cEventSystem Instance
    {
        get
        {
            if (_instance == null)
                _instance = GameObject.FindObjectOfType<cEventSystem>();
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
            return;
        }

    }
	#endregion

    void Start()
    {
       //this.transform.parent = GameManager.Instance.transform;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AdvanceCharacterTurn();
        }
    }
	public void AdvanceBattleRound()
    {
         ABR?.Invoke();
    }
    public void AdvanceCharacterTurn()
    {
         ACT?.Invoke();
    }

}
