using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Button = UnityEngine.UI.Button;


public class UIBattleMenuController : MonoBehaviour
{
    public Canvas _canvas;

    public TextMeshProUGUI _name;
    public GameObject _subpanel;

    public Animator _subpanelAnimator;
    public Button[] _menuButtons;

    public  enum eMenuState { DEFAULT, MOVE, ATTACK,BASICATTACK, SKILL, BURST}
    public eMenuState _menuState = eMenuState.DEFAULT;

    public static string[] _defaultText = new string[] {"Move", "Attack" , "End Turn" };
    delegate void DefaultActions();
    List<DefaultActions> defaultActions;

    private void CreateDefaultList()
    {
        defaultActions = new List<DefaultActions>();
        defaultActions.Add(DoMove);
        defaultActions.Add(DoAttack);
        defaultActions.Add(DoEndTurn);
    }
    private void Awake()
    {
        if (_canvas == null)
            _canvas = this.transform.GetComponent<Canvas>();
        _canvas.worldCamera = Camera.main;
    }

    // Start is called before the first frame update
    void Start()
    {
        CreateDefaultList();
    }
    private void OnEnable()
    {
        ShowMenu(true,null);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
            ShowMenu(true, null);
        if (Input.GetKeyDown(KeyCode.C))
            ShowMenu(false, null);

 
    }
    public void SetName(string name)  { _name.text = name;}

    public void ShowMenu(bool cond, Transform t)
    {
        if (t)
            this.transform.position = t.position;

        _subpanelAnimator.SetBool("Open", cond);
       for(int i=0; i<_menuButtons.Length; ++i)
        {
            Animator a = _menuButtons[i].GetComponent<Animator>();
            if (a)
                a.SetBool("Show", cond);
            if(cond)
                SetButtonText(_menuButtons[i].GetComponentInChildren<TextMeshProUGUI>(), i );
        }
    }
    private void SetButtonText(TextMeshProUGUI text, int index)
    {
        if (text == null)
            return;

        switch(_menuState)
        {
            case eMenuState.DEFAULT:
                {
                    if(index>-1 && index<_defaultText.Length)
                        text.text = _defaultText[index];
                    break;
                }

        }
    }

    public void ImClicked(GameObject button)
    {
        Debug.Log("IMCLICKED" +button);

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
        Debug.Log("Move");
        SelectionManager.Instance.EnableMove(true);
    }
    private void DoAttack()
    {
        Debug.Log("Attack");
        //Change the Menu to say "Basic , Skills, Burst"
    }
    private void DoEndTurn()
    {
        cEventSystem.Instance.AdvanceCharacterTurn();
    }


}
