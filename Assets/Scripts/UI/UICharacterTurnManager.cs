using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICharacterTurnManager : MonoBehaviour
{
    #region Vars
    static UICharacterTurnManager _instance;
    public GameObject _PREFABTurnOrder;
    public GameObject _PREFABTurnOrderPast;
    public GameObject _PREFABTurnOrderCurrent;
    private int _offset = 50; //How far we space out the portraits from one another
    private Button[] _UIIcons;
    private List<CharacterHolder> _oldTurns;
    private List<CharacterHolder> _newTurns;
    private CharacterHolder _currentChar;
    private int _indexL = 0;
    private int _indexR = 0;

    private enum eSelectionState { LEFT, NONE, RIGHT };
    private eSelectionState _state = eSelectionState.NONE;
    #endregion

    public static UICharacterTurnManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = GameObject.FindObjectOfType<UICharacterTurnManager>();
            return _instance;
        }
    }

    private void Awake()
    {

        if (_instance == null)
            _instance = this;
        else if (_instance != this)
            Destroy(this);

        IniScript();
    }
    private void IniScript()
    {
        _PREFABTurnOrder = Resources.Load<GameObject>("Prefabs/UI/TurnOrderFrame");
        if (_PREFABTurnOrder == null)
            Debug.LogError("(UICharacterTurnManager) Prefab Future is Missing ");
        _PREFABTurnOrderPast = Resources.Load<GameObject>("Prefabs/UI/TurnOrderFramePast");
        if (_PREFABTurnOrderPast == null)
            Debug.LogError("(UICharacterTurnManager) Prefab Past is Missing ");
        _PREFABTurnOrderCurrent = Resources.Load<GameObject>("Prefabs/UI/TurnOrderFrameCurrent");
        if (_PREFABTurnOrderCurrent == null)
            Debug.LogError("(UICharacterTurnManager) Prefab Current is Missing ");
    }
    void OnEnable()
    {
        //Subscribe AdvanceTurn() 
        cEventSystem.Instance.ACT += AdvanceTurn;
    }
    void OnDisable() //does fire on destroy
    {
        //UnSubscribe AdvanceTurn() 
        cEventSystem.Instance.ACT -= AdvanceTurn;
    }
    private void Start()
    {
        _UIIcons = new Button[5];
        _oldTurns = new List<CharacterHolder>();
        _newTurns = new List<CharacterHolder>();

    }
    private void Update()
    {
        //TMP
        if (Input.GetKeyDown(KeyCode.L))
            ShuffleLeft();
        if (Input.GetKeyDown(KeyCode.R))
            ShuffleRight();
    }
    /**
	* Sets up the UI elements and character portraits for the battle
	*/
    public void StartBattle()
    {
        int index = 0;
        //Set up our UI elements 
        for (int i = 0; i < 5; ++i)
        {
            GameObject button=null;
            if (i < 2)
                button = Instantiate(_PREFABTurnOrderPast);
            else if (i == 2)
                button = Instantiate(_PREFABTurnOrderCurrent);
            if (i > 2)
                button = Instantiate(_PREFABTurnOrder);
            button.gameObject.transform.SetParent(this.transform);
            button.transform.localPosition = new Vector3(-120 + (_offset * index++), 50, 0);
            //Debug.Log(button.transform.localPosition);
            button.transform.localRotation = Quaternion.identity;
            button.transform.localScale = new Vector3(1, 1, 1);
            PortraitHolder ph = button.GetComponent<PortraitHolder>();
            if (ph == null)
                button.AddComponent<PortraitHolder>();
            _UIIcons[i] = button.GetComponent<Button>();
        }
    }
    public void SetUpTurn(List<GameObject> characters)
    {
        foreach (GameObject g in characters)
        {
            //Get the Portrait, the transform and faction from characters
            cGeneral general = g.GetComponent<cGeneral>();
            if (general)
            {
                CharacterHolder ch = new CharacterHolder(general.GetPortrait(), g.transform, general.GetFaction().IsHuman(), general.GetName());
                _newTurns.Add(ch);
            }
        }
        _currentChar = _newTurns[0];
        SetPortrait(2, _currentChar);
        ResetBothSides();
    }
    public void AdvanceTurn()
    {
        _oldTurns.Add(_currentChar);
        if (_newTurns.Count!=0)
        {
            _currentChar = _newTurns[0];
            _newTurns.Remove(_currentChar);
            SetPortrait(2, _currentChar);
            ResetBothSides();
        }
        else
        {
            //Tell Event Manager round is over;
            cEventSystem.Instance.AdvanceBattleRound();
        }
    }
    private void ShuffleLeft()
    {
        if (_state == eSelectionState.LEFT)
        {
            if (_indexL - 1 >= 0)
            {
                //_UIIcons[1] = _oldTurns[_indexL];
                //_UIIcons[0] = _oldTurns[--_indexL];
                SetPortrait(1, _oldTurns[_indexL]);
                SetPortrait(0, _oldTurns[--_indexL]);
            }
        }
        else if (_state == eSelectionState.RIGHT)
        {
            if (_indexR - 1 >= 0)
            {
                SetPortrait(3, _newTurns[_indexR]);
                SetPortrait(4, _newTurns[--_indexR]);
            }
            else
                _state = eSelectionState.NONE;
        }
        else
            _state = eSelectionState.LEFT;
    }
    private void ShuffleRight()
    {
        if (_state == eSelectionState.LEFT)
        {
            if (_indexL + 1 < _oldTurns.Count - 1)
            {
                SetPortrait(1, _oldTurns[_indexL++]);
                SetPortrait(0, _oldTurns[_indexL]);
            }
            else
                _state = eSelectionState.NONE;
        }
        else if (_state == eSelectionState.RIGHT)
        {
            if (_indexR + 1 < _newTurns.Count - 1)
            {
                SetPortrait(3, _newTurns[_indexR]);
                SetPortrait(4, _newTurns[++_indexR]);
            }
        }
        else
            _state = eSelectionState.RIGHT;
    }
    private void ResetBothSides()
    {
        if (_oldTurns.Count > 1)
        {
            SetPortrait(0, _oldTurns[_oldTurns.Count - 2]);
            SetPortrait(1, _oldTurns[_oldTurns.Count - 1]);
        }
        else if (_oldTurns.Count > 0)
        {
            SetPortrait(1, _oldTurns[_oldTurns.Count - 1]);
            //Set 0 to empty?? how
        }

        if (_newTurns.Count > 1)
        {
            SetPortrait(3, _newTurns[0]);
            SetPortrait(4, _newTurns[1]);
        }
        else if (_newTurns.Count > 0)
        {
            SetPortrait(3, _newTurns[0]);
            //Set 4 to Empty somehow
        }

        _indexL = _oldTurns.Count - 1;
        _indexR = 0;
    }

    private void SetPortrait(int index, CharacterHolder ch)
    {
        PortraitHolder ph = _UIIcons[index].GetComponent<PortraitHolder>();
        if (ph)
            ph.SetPortait(ch._portrait, ch._isFriendly, index==2, ch._name);
    }
}
