using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum eSelectionState { FREE, MOVE, ATTACK, SKILL, MENU };

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

    // Called 0th
    private void Awake()
    {
        // Singleton check
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(this);

    }

    private void Update()
    {
        /*
         * Ideally this all needs to move out of update and the InputManager Tells
         * Selection Manager somethins occurred
         */

        //I need some type of enum or control logic from the InputController
        if (InputController.GetSelectPressDown())
            HandleInput();

        //TMP fix
        if (Input.GetKeyDown(KeyCode.DownArrow) ||
            Input.GetKeyDown(KeyCode.UpArrow) ||
            Input.GetKeyDown(KeyCode.Return))
            HandleInput();

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
                    FreeClick(InputController.GetCursorPosition());
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

                    //ToDo Read from InputManager
                    if (Input.GetKeyDown(KeyCode.DownArrow))
                        UIBattleMenuController.Instance.ChangeSelection(-1);
                    else if (Input.GetKeyDown(KeyCode.UpArrow))
                        UIBattleMenuController.Instance.ChangeSelection(1);
                    else if (Input.GetKeyDown(KeyCode.Return))
                        UIBattleMenuController.Instance.ClickSelected();


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
            //When we show the menu we should tell the battlemenu if attack or move is an option
            UIBattleMenuController.Instance.ShowMenu(
                true, _activeChar.transform.position,
                _activeChar.name, canMove, canAttack);

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
    public void EnableSkill(bool cond, Skill skill)
    {
        _selectionState = eSelectionState.SKILL;
        _SkillToUse = skill;

        CursorController.Instance.ToggleCursor(cond);
        //Turn on/off the Menu
        if (_activeChar)
        {
            UIBattleMenuController.Instance.ShowMenu(false, _activeChar.transform.position);
            CreateMovementLine.Instance.EnablePathPreview(_activeChar.GetComponent<MovementController>());
            CameraController.Instance.BeginFollowingCharacter(_activeChar.transform);

        }

    }
    #endregion

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


        }
    }
    private void ClickToAttack()
    {
        // Debug.Log("MoveClick");
        if (_activeChar)
        {
            //Won't be null because its required
            MovementController mc = _activeChar.GetComponent<MovementController>();

            //First check if anyone is at location of click:

            GameObject character = CursorController.Instance.GetCharacterAtCursor();
            if (character != null) //Might want to create a Verify Hit method that returns the gameobject?
            {
                //Check if Enemy (or ally depending on skill?)

                //Check if enough AP (1?)


                Skill basic = _activeChar.GetComponent<SkillManager>().GetBasicAttack();
                List<GameObject> targets = new List<GameObject>();
                targets.Add(character.transform.gameObject);
                //Check if in Range 
                if (!_activeChar.EnemiesInRange().Contains(character.transform.gameObject.GetComponent<Playable>()))
                {
                    //If not in range move to location then attack?
                    mc.DoMovement(CursorController.Instance.transform.position, basic.Perform(_activeChar.transform.gameObject, targets));

                    //Subtract AP now?
                }
                else
                {



                    StartCoroutine(basic.Perform(_activeChar.transform.gameObject, targets));
                }

            }
            else
                Debug.LogError("Character did not have a Movement Controller");

        }
    }
    private void ClickToUseSkill()
    {

    }
    private void MoveComplete()
    {
        ShowBattleMenu();
        CameraController.Instance.StopFollowingCharacter();
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
