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
    public GameObject _backButton;

    public Animator _subpanelAnimator;
    public UIButton[] _menuButtons;

    public  enum eMenuState { DEFAULT, MOVE, ATTACK,BASICATTACK, SKILL, BURST}
    public eMenuState _menuState = eMenuState.DEFAULT;

    //Why cant i make this const?
    public static string[] _defaultText = new string[] { "Move", "Attack" , "End Turn" };
    public static string[] _attackText = new string[] { "Basic", "Skills", "Burst" };

    delegate void DefaultActions();
    List<DefaultActions> _defaultActions;

    delegate void AttackActions();
    List<AttackActions> _attackActions;

    [SerializeField] Vector3 _offsetFromCharacter = new Vector3(75, -75, 0);

    public bool _isOn;
    public Vector3 _lastPos;

    private int _currIndex;

    private Skill[] _skills;

    #endregion


    // Called when the component is enabled
    // Subscribe to events
    private void OnEnable()
    {
        cEventSystem.OnCameraMove += ResetMenu;

        ShowMenu(true, Vector3.zero);
    }
    // Called when the component is disabled
    // Unsubscribe from events
    private void OnDisable()
    {
        cEventSystem.OnCameraMove -= ResetMenu;
    }
    // Called when the gameobject is destroyed
    // Unsubscribe from ALL events
    private void OnDestroy()
    {
        cEventSystem.OnCameraMove -= ResetMenu;
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

    ///Used by event system to reposition menu when camera is moved
    public void ResetMenu()
    {
        this.transform.position = ConvertToScreenSpace(_lastPos);
    }
    /// <summary>
    /// Shows the Battle Menu at character location with parameters
    /// </summary>
    /// <param name="cond"></param>
    /// <param name="worldPos"></param>
    /// <param name="name"></param>
    /// <param name="canMove"></param>
    /// <param name="canAttack"></param>
    /// <param name="skills"></param>
    public void ShowMenu(bool cond, Vector3 worldPos, string name, bool canMove, bool canAttack, Skill[] skills)
    {
        SetName(name);
        ShowMenu(cond, worldPos);
        //Might want to contain this/null checks?
        _menuButtons[0].SetInteractable(canMove);
        _menuButtons[1].SetInteractable(canAttack);
        _skills = skills;
    }
    public void ShowMenu(bool cond, Vector3 worldPos)
    {
        if (worldPos != Vector3.zero)
            this.transform.position = ConvertToScreenSpace(worldPos);

        //Update our buttons
        for (int i = 0; i < _menuButtons.Length; ++i)
        {
            //tmp cache to keep track of if the text is valid or not
            string bText = DetermineButtonText(i);

            if (bText.Equals("") || cond==false)
                _menuButtons[i].gameObject.SetActive(false);
            else
            {
                _menuButtons[i].SetText(bText);
                _menuButtons[i].gameObject.SetActive(true);
            }

            if(_menuState== eMenuState.SKILL && _skills!=null)
            {
                if (_skills.Length > i)
                    _menuButtons[i].AssignSkill(_skills[i]); /// this might need fixing 
            }
        }

        //Turn off and on the old and new 
        SetSelected(false);
        _currIndex = 0;
        SetSelected(true);

        //Turn on or off the Background components
        _subpanel.gameObject.SetActive(cond);
        _name.gameObject.SetActive(cond);
        EnableBackButton(cond);

        //Update state
        _isOn = cond;
        _lastPos = worldPos;

    }
    private Vector3 ConvertToScreenSpace(Vector3 pos)
    {
        return Camera.main.WorldToScreenPoint(pos) + _offsetFromCharacter;
    }

    private void EnableBackButton(bool cond)
    {
        if (_backButton == null)
             { Debug.LogError("BackButtonMissing, return"); return;}

        if(!cond || _menuState == eMenuState.DEFAULT) // Don't show it on default
            _backButton.SetActive(false);
        else if(_menuState!=eMenuState.DEFAULT) // else if were in a sub state, show it
            _backButton.SetActive(cond);

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
            case eMenuState.SKILL:
                {
                    return GetSkillText(index);
                }

        }

        return null;
    }
    ///Helper method for DetermineButtonText , validates indicies and returns string from array
    private string GetButtonText(int index, string[] textArr)
    {
        if (index > -1 && index < textArr.Length)
            return textArr[index];
        else
            return "Invalid";
    }

    //going to need to change
    private string GetSkillText(int index)
    {
        if (_skills == null)
            return "";
        if (_skills.Length <= index)
            return "";

        //TODO make skills change with>3 index
        return _skills[index].GetName();
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
                        ///this will have to change 
                        for (int i = 0; i < _menuButtons.Length; ++i)
                        {
                            if (button == _menuButtons[i].gameObject)
                            {
                                // Get some kind of stored action off the button?
                                Skill sk = _menuButtons[i].GetSkill();
                                if (sk)
                                    SelectionManager.Instance.EnableSkill(true, sk);
                                return;
                            }
                        }
                        break;
                    }

            }
        }
    }
    public void ResetToDefault()
    {
        SwitchState(eMenuState.DEFAULT);
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
        // Advance the character's turn
        cEventSystem.CallOnCharacterTurnEnd();
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

        EnableBackButton(true);
    }
}

