using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum eSelectionState { FREE, MOVE, ATTACK, SKILL, MENU , WAITING};

public class SelectionManager : MonoBehaviour
{
    // Singleton
    public static SelectionManager Instance { get; private set; }


    // Current selection state
    private eSelectionState _selectionState = eSelectionState.FREE;

    // The active character
    private Playable _activeChar;

    //Skill to use assigned by UIBattleMenu
    private Skill _SkillToUse;


    // Called when the component is enabled.
    // Subscribe to events.
    private void OnEnable()
    {
        cEventSystem.OnHasMenuInput += HandleInput;
        cEventSystem.OnSelectPressDown += HandleInput;
    }
    // Called when the component is disabled.
    // Unsubscribe from events.
    private void OnDisable()
    {
        cEventSystem.OnHasMenuInput -= HandleInput;
        cEventSystem.OnSelectPressDown -= HandleInput;
    }
    // Called when the gameobject is destroyed.
    // Unsubscribe from ALL events.
    private void OnDestroy()
    {
        cEventSystem.OnHasMenuInput -= HandleInput;
        cEventSystem.OnSelectPressDown -= HandleInput;
    }

    // Called 0th
    private void Awake()
    {
        // Singleton check
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(this);

    }
    //Ideally get rid of this update 
    private void Update()
    {
        /*
        if (InputController.HasMenuInput())
           HandleInput();
           */

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_selectionState == eSelectionState.MOVE)
            {
                ShowBattleMenu();
            }
        }
    }

    /// <summary>
    /// Handles what to do when the user presses select
    /// </summary>
    private void HandleInput()
    {
        //Debug.Log("SelectionState=" + _selectionState);
        //TellMyUIClick(mousePos);
        switch (_selectionState)
        {
            case eSelectionState.FREE:
                {
                    //TMP- Need control logic from inputcontroller
                    FreeClick(Input.mousePosition);
                    break;
                }
            case eSelectionState.MOVE:
                {
                    ClickToMove();
                    break;
                }
            case eSelectionState.ATTACK:
                {
                    ClickToAttack();
                    break;
                }
            case eSelectionState.SKILL:
                {
                    ClickToUseSkill();
                    break;
                }
            case eSelectionState.MENU:
                {
                    //Will need to break apart later

                    Vector2Int menuAxis = InputController.GetMenuAxis();
                    // If the user inputted to navigate up or down.
                    if (menuAxis.y != 0)
                        UIBattleMenuController.Instance.ChangeSelection(menuAxis.y);
                    // If the user inputted a selection
                    else if (InputController.GetMenuSelectDown())
                        UIBattleMenuController.Instance.ClickSelected();


                    break;
                }
            case eSelectionState.WAITING:
                {
                    break;
                }
            default:
                break;
        }
    }
    /// <summary>
    /// Changes the SelectionManagers knowledge on who's turn it is, then shows its battlemenu
    /// </summary>
    /// <param name="character"></param>
    public void SetActiveCharacter(Playable character)
    {
        //Clear the old character
        if (_activeChar)
            _activeChar.SetSelected(false);
        //Assign the new
        if (character)
            character.SetSelected(true); //Updates the camera 

        _activeChar = character;
        ShowBattleMenu();
    }

    /// <summary>
    /// Shows the BattleMenu for the active character by informing the UIBattleMenu of
    /// the characters current state
    /// </summary>
    public void ShowBattleMenu()
    {

        if (_activeChar)
        {
            //Turn off Cursor/path
            CreateMovementLine.Instance.DisablePathPreview();
            CursorController.Instance.ToggleCursor(false);


            _selectionState = eSelectionState.MENU;

            bool canMove = true; //ToDo figure out AP
            bool canAttack = DetermineValidAttack();
            TroopContainer tc = _activeChar.GetComponent<TroopContainer>();
            Skill[] skills = tc.GetSkills();
            //When we show the menu we should tell the battlemenu if attack or move is an option
            UIBattleMenuController.Instance.ShowMenu(
                true, _activeChar.transform.position,
                _activeChar.name, canMove, canAttack, skills);

            //Give player UI representation of whos attackable
            _activeChar.ShowCollidersInRange();
        }
    }
    #region HelpersForShowBattleMenu
    /**returns true is the character has enemies in range and enough AP */
    private bool DetermineValidAttack()
    {
        bool inrange = false;
        //TODO AP check
        bool enoughAp = true;


        if (_activeChar.EnemiesInRange() != null)
            inrange = _activeChar.EnemiesInRange().Count > 0;

        return inrange && enoughAp;
    }
    #endregion

    #region StateChangers 
    //These Change the State of the SelectionManager
    public void EnableMove(bool cond)
    {
        //Turn on/off the Menu
        if (_activeChar && cond)
        {
            _selectionState = eSelectionState.MOVE;
            UIBattleMenuController.Instance.ShowMenu(false, _activeChar.transform.position);
            CreateMovementLine.Instance.EnablePathPreview(_activeChar.GetComponent<MovementController>());
            // CameraController.Instance.SetFollowTarget(cond, _activeChar.transform);
            CameraController.Instance.BeginFollowingCharacter(_activeChar.transform);
            CursorController.Instance.ToggleCursor(true);

        }
        else // This feels really goofy 
            ShowBattleMenu();

    }
    ///UnderDevelopment
    public void EnableAttack(bool cond)
    {
        _selectionState = eSelectionState.ATTACK;
        CursorController.Instance.ToggleCursor(cond);
        //Turn on/off the Menu
        if (_activeChar)
        {
            UIBattleMenuController.Instance.ShowMenu(false, _activeChar.transform.position);
            CreateMovementLine.Instance.EnablePathPreview(_activeChar.GetComponent<MovementController>());
            CameraController.Instance.BeginFollowingCharacter(_activeChar.transform);

        }

    }
    ///UnderDevelopment
    public void EnableSkill(bool cond, Skill skill)
    {
        _selectionState = eSelectionState.SKILL;
        _SkillToUse = skill;

        //Check if the skill is use immediate , ToDo not sure if this is sound 
        if (_SkillToUse.GetIsUseImmediate())
        {
            List<Playable> attacker = new List<Playable>();
            attacker.Add(_activeChar);
            BattleManager.Instance.ManageSkill(attacker, null, _SkillToUse);
        }
        else //ToDo not sure if this is sound
        {
            CursorController.Instance.ToggleCursor(cond);
            //Turn on/off the Menu
            if (_activeChar)
            {
                UIBattleMenuController.Instance.ShowMenu(false, _activeChar.transform.position);
                CreateMovementLine.Instance.EnablePathPreview(_activeChar.GetComponent<MovementController>());
                CameraController.Instance.BeginFollowingCharacter(_activeChar.transform);

            }
        }
    }
    #endregion



    private void ResetFromAttack()
    {
        cEventSystem.OnAttackFinished -= ResetFromAttack;
        UIBattleMenuController.Instance.ResetToDefault();
        //Tmp? Don't like this solution the method name is awkward
        EnableMove(false);
    }
    /**
     * Not sure what this method will do
     * Currently not used
     */
    private void FreeClick(Vector3 mousePos)
    {
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log("WE HIT: " + hit.transform.gameObject);
            Debug.DrawRay(Camera.main.transform.position, ray.direction * 10, Color.red, 10);
            Playable p = hit.transform.GetComponent<Playable>();
            if (p)
            {

            }
        }
    }


    /** Move active character to a location that isnt occupied by a  playable */
    private void ClickToMove()
    {
        // Debug.Log("MoveClick");
        if (_activeChar)
        {

            //Won't be null because its required
            MovementController mc = _activeChar.GetComponent<MovementController>();

            //TODO make sure no obstacles are there 

            // Hide the cursor and stop drawing a path
            CreateMovementLine.Instance.DisablePathPreview();
            CursorController.Instance.ToggleCursor(false);

            // Follow the character who will move
            CameraController.Instance.BeginFollowingCharacter(mc.transform);
            //Start the movement and pass in a callback function
            mc.DoMovement(CursorController.Instance.transform.position, MoveComplete);
            _activeChar.SetMoving();

        }
    }
    private void ClickToAttack()
    {
        // Debug.Log("MoveClick");
        if (_activeChar)
        {
            //Find Character at location of click:
            GameObject character = CursorController.Instance.GetCharacterAtCursor();
            Skill basic = _activeChar.GetComponent<SkillManager>().GetBasicAttack();
            //BM handles nulls
            bool valid = BattleManager.Instance.ManageSkill(_activeChar, character.GetComponent<Playable>(), basic);

            if (valid) //Prevent them from clicking anything else while executing combat
            {
                _selectionState = eSelectionState.WAITING;
                cEventSystem.OnAttackFinished += ResetFromAttack;
            }


            }
    }
    private void ClickToUseSkill()
    {
        if (!_SkillToUse || !_activeChar)
            return;

        bool valid = false;  //Let the battleManager Verify the passed in data is valid for the skill
       
        //This logic may need to change when we get/add to this list.
        List<Playable> _skillUsers = new List<Playable>();
        _skillUsers.Add(_activeChar);

        //if needs target
        if (_SkillToUse.GetRequiresTarget())
        {
            //See if theres a character at selection
            GameObject character = CursorController.Instance.GetCharacterAtCursor();
            if(character) //BM will handle other nulls 
                valid = BattleManager.Instance.ManageSkill(_skillUsers, character.GetComponent<Playable>(), _SkillToUse);

        }
        else
        {
            Vector3 location;
            if (_SkillToUse.GetRange()==0) //Melee
                location= _activeChar.transform.position; //self loc
            else //Ranged
                location = CursorController.Instance.transform.position; // cursor loc
            valid = BattleManager.Instance.ManageSkill(_skillUsers, location, _SkillToUse);
        }



        if (valid) //Prevent them from clicking anything else while executing combat
        {
            _selectionState = eSelectionState.WAITING;
            cEventSystem.OnAttackFinished += ResetFromAttack;
        }
        //else
            //Display some kind of response to player


    }
    private void MoveComplete()
    {
        ShowBattleMenu();
        CameraController.Instance.StopFollowingCharacter();
        _activeChar.SetIdle();
    }



    private void SetMoveable(bool cond)
    {
        //Wont be null, required
        _activeChar.GetComponent<MovementController>().enabled = cond;

        if (cond)
            _selectionState = eSelectionState.MOVE;
        else
            _selectionState = eSelectionState.FREE; // might need diff logic
    }




    ///---------------------------------------------------------------------///
    ///
    //DEBUGGING TMP STUFF // UNUSED 
    //UI - TMP Debugging
    ///
    ///---------------------------------------------------------------------///
    private void TellMyUIClick(Vector3 mousePos)
    {
        if (_activeChar)
        {
            GraphicRaycaster raycaster = _activeChar.transform.GetComponentInChildren<GraphicRaycaster>();
            if (raycaster)
            {
                //Set up the new Pointer Event
                PointerEventData pointerData = new PointerEventData(EventSystem.current);
                List<RaycastResult> results = new List<RaycastResult>();

                //Raycast using the Graphics Raycaster and mouse click position
                pointerData.position = Input.mousePosition;
                raycaster.Raycast(pointerData, results);

                //For every result returned, output the name of the GameObject on the Canvas hit by the Ray
                foreach (RaycastResult result in results)
                {
                    Debug.Log("Hit " + result.gameObject.name);
                }
            }

        }

    }

    /** Check if the Raycast Hit hit a playable object or not */
    private bool CheckPlayable(RaycastHit hit)
    {
        Playable p = hit.transform.GetComponent<Playable>();
        return p != null;
    }


}
