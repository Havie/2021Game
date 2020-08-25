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

    public static string[] _defaultText = new string[] {"Move", "Attack" , "End Turn" };
    delegate void DefaultActions();
    List<DefaultActions> defaultActions;

    [SerializeField] Vector3 _offsetFromCharacter = new Vector3(75, -75, 0);

    public bool _isOn;
    public Vector3 _lastPos;

    private int _currIndex;

    #endregion

    private void CreateDefaultList()
    {
        defaultActions = new List<DefaultActions>();
        defaultActions.Add(DoMove);
        defaultActions.Add(DoAttack);
        defaultActions.Add(DoEndTurn);
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
        ShowMenu(false,Vector3.zero);
    }
    private void OnEnable()
    {
       ShowMenu(true, Vector3.zero);
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
            ShowMenu(true, Vector3.zero);
        if (Input.GetKeyDown(KeyCode.C))
            ShowMenu(false, Vector3.zero);
    }
    public void SetName(string name)  { _name.text = name;}

    public void ResetMenu()
    {
        this.transform.position = ConvertToScreenSpace(_lastPos);
    }
    public void ShowMenu(bool cond, Vector3 worldPos, string name)
    {
        SetName(name);
        ShowMenu(cond, worldPos);
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
    private string DetermineButtonText(int index)
    {
        switch(_menuState)
        {
            case eMenuState.DEFAULT:
                {
                    if(index>-1 && index<_defaultText.Length)
                        return _defaultText[index];
                    break;
                }

        }

        return null;
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
        ImClicked(_menuButtons[_currIndex].gameObject);
    }

    public void ImClicked(GameObject button)
    {
        //Debug.Log("IMCLICKED" +button);

        switch (_menuState)
        {
            case eMenuState.DEFAULT:
                {   
                    //Figure out who was clicked and what action to perform
                    for (int i = 0; i < _menuButtons.Length; ++i)
                    {
                        if(button==_menuButtons[i].gameObject)
                        {
                            if(i<defaultActions.Count)
                                defaultActions[i]();
                            return;
                        }
                    }
                  break;
                }

        }
    }

    private void DoMove()
    {
        SelectionManager.Instance.EnableMove(true);
    }
    private void DoAttack()
    {
        Debug.Log("Attack");
        //Change the Menu to say "Basic , Skills, Burst"
    }
    private void DoEndTurn()
    {
        ShowMenu(false, Vector3.zero);
        cEventSystem.Instance.AdvanceCharacterTurn();
    }


}
