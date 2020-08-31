using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Button = UnityEngine.UI.Button;


public class UIBattleMenuController : MonoBehaviour
{

    public static UIBattleMenuController Instance { get; private set; }
    #region Variables
    public Canvas _canvas;

    public TextMeshProUGUI _name;
    public GameObject _subpanel;

    public Animator _subpanelAnimator;
    public UIButton[] _menuButtons;

    public  enum eMenuState { DEFAULT, MOVE, ATTACK,BASICATTACK, SKILL, BURST}
    public eMenuState _menuState = eMenuState.DEFAULT;

    //Why cant i make this const?
    public static string[] _defaultText = new string[] {"Move", "Attack" , "End Turn" };
    public static string[] _attackText = new string[] { "Basic", "Skills", "Burst" };

    delegate void DefaultActions();
    List<DefaultActions> _defaultActions;

    delegate void AttackActions();
    List<AttackActions> _attackActions;

    [SerializeField] Vector3 _offsetFromCharacter = new Vector3(75, -75, 0);

    public bool _isOn;
    public Vector3 _lastPos;

    private int _currIndex;

    #endregion


    // Called when the component is enabled
    // Subscribe to events
    private void OnEnable()
    {
        CameraController.OnCameraMove += ResetMenu;

        ShowMenu(true, Vector3.zero);
    }
    // Called when the component is disabled
    // Unsubscribe from events
    private void OnDisable()
    {
        CameraController.OnCameraMove -= ResetMenu;
    }
    // Called when the gameobject is destroyed
    // Unsubscribe from ALL events
    private void OnDestroy()
    {
        CameraController.OnCameraMove -= ResetMenu;
    }

    private void CreateDefaultList()
    {
        _defaultActions = new List<DefaultActions>();
        _defaultActions.Add(DoMove);
        _defaultActions.Add(SwitchToAttack);
        _defaultActions.Add(DoEndTurn);
    }
    private void CreateAttackList()
    {
        _attackActions = new List<AttackActions>();
        _attackActions.Add(BasicAttack);
        _attackActions.Add(SwitchToSkills);
        _attackActions.Add(DoBurst);
    }
    private void CreateSkillList()
    {

    }
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Debug.LogWarning("Multiple UIBattleMenuController in scene, should be a singleton");

        if (_canvas == null)
            _canvas = this.transform.GetComponentInParent<Canvas>();
        _canvas.worldCamera = Camera.main;
    }

    // Start is called before the first frame update
    void Start()
    {
        CreateDefaultList();
        CreateAttackList();
        //ShowMenu(false,Vector3.zero);
    }

    void Update()
    {
        //TMP
        if (Input.GetKeyDown(KeyCode.O))
            ShowMenu(true, Vector3.zero);
        if (Input.GetKeyDown(KeyCode.C))
            ShowMenu(false, Vector3.zero);

        //ToDo Get this from InputController
        if (Input.GetKeyDown(KeyCode.Backspace) && _isOn)
            GoUpALevel();
    }
    public void SetName(string name)  { _name.text = name;}

    //Used by event system to reposition menu when camera is moved
    public void ResetMenu()
    {
        this.transform.position = ConvertToScreenSpace(_lastPos);
    }
    public void ShowMenu(bool cond, Vector3 worldPos, string name, bool canMove, bool canAttack)
    {
        SetName(name);
        ShowMenu(cond, worldPos);
        //Might want to contain this/null checks?
        _menuButtons[0].SetInteractable(canMove);
        _menuButtons[1].SetInteractable(canAttack);
    }
    public void ShowMenu(bool cond, Vector3 worldPos)
    {

        if (worldPos != Vector3.zero)
            this.transform.position = ConvertToScreenSpace(worldPos);

        for (int i = 0; i < _menuButtons.Length; ++i)
        {
            //need a way to keep track of if the text is valid or not
            _menuButtons[i].SetText(DetermineButtonText(i));
        }
        //Turn off and on the old and new 
        SetSelected(false);
        _currIndex = 0;
        SetSelected(true);

        _subpanel.gameObject.SetActive(cond);
        _name.gameObject.SetActive(cond);
        //It feels like this shouldn't work for SetSelected when they are turned back on, but it does
        foreach (UIButton b in _menuButtons)
            b.gameObject.SetActive(cond);

        _isOn = cond;
        _lastPos = worldPos;

    }
    private Vector3 ConvertToScreenSpace(Vector3 pos)
    {
        return Camera.main.WorldToScreenPoint(pos) + _offsetFromCharacter;
    }

    //returns the string for the button at a index based on menu state
    private string DetermineButtonText(int index)
    {
        switch(_menuState)
        {
            case eMenuState.DEFAULT:
                {
                    return GetButtonText(index, _defaultText);
                }
            case eMenuState.ATTACK: 
                {
                   return  GetButtonText(index, _attackText);
                }

        }

        return null;
    }
    //Helper method for DetermineButtonText , validates indicies and returns string from array
    private string GetButtonText(int index, string[] textArr)
    {
        if (index > -1 && index < textArr.Length)
            return textArr[index];
        else
            return "Invalid";
    }

    public void ChangeSelection(int incrementAmount)
    {

        //Turn off our old selection
        SetSelected(false);

        _currIndex -= incrementAmount;
        if (_currIndex > _menuButtons.Length-1)
            _currIndex = 0;
        else if (_currIndex < 0)
            _currIndex = _menuButtons.Length-1;

        //Turn on our new selection
        SetSelected(true);
    }
    public void ChangeSelection(GameObject button)
    {
        for (int i = 0; i < _menuButtons.Length; ++i)
        {
            if (button == _menuButtons[i].gameObject)
            {
                SetSelected(false);
                _currIndex = i;
                SetSelected(true);
                return;
            }
        }
    }
    private void SetSelected(bool cond)
    {
        _menuButtons[_currIndex].SetSelected(cond);
    }
    public void ClickSelected()
    {
        //There is a bug in here, when ending turn going between MouseClickHovers vs ArrowKeyEnter
        ImClicked(_menuButtons[_currIndex].gameObject);
    }

    public void ImClicked(GameObject button)
    {
        //Debug.Log("IMCLICKED" +button);
        Button b= button.GetComponentInChildren<Button>();
        if (b == null)
            return;

        if (b.interactable)
         {

            switch (_menuState)
            {
                case eMenuState.DEFAULT:
                    {
                        //Figure out who was clicked and what action to perform
                        for (int i = 0; i < _menuButtons.Length; ++i)
                        {
                            if (button == _menuButtons[i].gameObject)
                            {
                                if (i < _defaultActions.Count)
                                    _defaultActions[i]();
                                return;
                            }
                        }
                        break;
                    }
                case eMenuState.ATTACK:
                    {
                        //Figure out who was clicked and what action to perform
                        for (int i = 0; i < _menuButtons.Length; ++i)
                        {
                            if (button == _menuButtons[i].gameObject)
                            {
                                if (i < _attackActions.Count)
                                    _attackActions[i](); //cant make a generic helper method cuz of type?
                               //possibly reset menu state to default?
                                return;
                            }
                        }
                        break;
                    }
                case eMenuState.SKILL:
                    {
                        //Figure out who was clicked and what action to perform
                        for (int i = 0; i < _menuButtons.Length; ++i)
                        {
                            if (button == _menuButtons[i].gameObject)
                            {
                                // Get some kind of stored action off the button?
                                return;
                            }
                        }
                        break;
                    }

            }
        }
    }

    private void SwitchState(eMenuState state)
    {
        //If we need to do anything special before switching this be done in switch
        switch(state)
        {
            case eMenuState.DEFAULT:
                {
                    break;
                }
        }

        _menuState = state;
        //Will display the proper button text
        ShowMenu(true, _lastPos);
    }

    private void DoMove()
    {
        SelectionManager.Instance.EnableMove(true);
    }
    private void SwitchToAttack()
    {
        //Change the Menu to say "Basic , Skills, Burst"
        SwitchState(eMenuState.ATTACK);
    }
    private void DoEndTurn()
    {
        ShowMenu(false, Vector3.zero);
        cEventSystem.Instance.AdvanceCharacterTurn();
    }
    private void BasicAttack()
    {
        SelectionManager.Instance.EnableAttack(true);

    }
    private void SwitchToSkills()
    {
        //Change the Menu to List Skills"
        SwitchState(eMenuState.SKILL);

    }
    private void DoBurst()
    {
        Debug.Log("DoBurst");

    }
    //Returns menu to previous options
    public void GoUpALevel()
    {
        switch (_menuState)
        {
            case eMenuState.DEFAULT:
                {
                  
                    break;
                }
            case eMenuState.ATTACK:
                {
                    SwitchState(eMenuState.DEFAULT);
                    break;
                }
            case eMenuState.SKILL:
                {
                    SwitchState(eMenuState.ATTACK);
                    break;
                }

        }
    }
}

