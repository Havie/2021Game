using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum eSelectionState { FREE, MOVE, ATTACK, MENU };

public class SelectionManager : MonoBehaviour
{
    // Singleton
    public static SelectionManager Instance { get; private set; }


    // Current selection state
    private eSelectionState _selectionState = eSelectionState.FREE;

    // The active character
    private Playable _activeChar;

    // Called 0th
    private void Awake()
    {
        // Singleton check
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(this);

    }

    // Update is called once per frame
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

    public void SetActiveCharacter(Playable character)
    {
        //Clear the old character
        if (_activeChar)
            SetSelected(_activeChar, false);
        //Assign the new
        if (character)
        {
            _activeChar = character;
            SetSelected(_activeChar, true);
        }
        ShowBattleMenu();
    }
    public void ShowBattleMenu()
    {
        if (_activeChar)
        {
            if (_selectionState == eSelectionState.MOVE)
            {
                CreateMovementLine.Instance.DisablePathPreview();
                CursorController.Instance.ToggleCursor(false);
                //this method doesnt disable the last known path Preview (TODO)
            }

            _selectionState = eSelectionState.MENU;

            bool canMove = true; //ToDo figure out AP
            bool canAttack = DetermineValidAttack();
            //When we show the menu we should tell the battlemenu if attack or move is an option
            UIBattleMenuController.Instance.ShowMenu(
                true, _activeChar.transform.position,
                _activeChar.name, canMove, canAttack);

            ShowCollidersInRange();
        }
    }
    private bool DetermineValidAttack()
    {
        bool inrange = false;
        //TODO AP check
        bool enoughAp = true;


        if (_activeChar.EnemiesInRange() != null)
            inrange = _activeChar.EnemiesInRange().Count > 0;

        return inrange && enoughAp;
    }
    private void ShowCollidersInRange()
    {
        foreach (Playable p in _activeChar.EnemiesInRange())
            p.SetSpriteOutline(Playable.eSpriteColor.ENEMY);  //Move to Color manager
        foreach (Playable p in _activeChar.EnemiesNotInRange())
            p.SetSpriteOutline(Playable.eSpriteColor.NEUTRAL);
    }
    private void SetSelected(Playable p, bool cond)
    {
        if (p == null)
            return;

        //It will probably already be off in some cases?
        //Might want to handle this inside the PLayable script-we'll see
        //p.ShowBattleMenu(cond);

        SpriteRenderer sp = p.GetSpriteRenderer();
        if (sp)
        {
            p.SetSelected(cond);
            if (cond)
                sp.material = AllMaterials.Instance._outlineSelected;
            else
                sp.material = AllMaterials.Instance._outlineNormal;
        }
        else
            Debug.LogWarning("Cant find Sprite Render for " + p.gameObject.name);

        //Tell the camera where to look
        if (cond)
            CameraController.Instance.MoveCameraToPos(p.transform.position);
    }
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

            GameObject hit = CursorController.Instance.GetCharacterAtCursor();
            Debug.Log("FOUND=" + hit);
            if (hit != null) //Might want to create a Verify Hit method that returns the gameobject?
            {
                Skill basic = _activeChar.GetComponent<SkillManager>().GetBasicAttack();
                List<GameObject> targets = new List<GameObject>();
                targets.Add(hit.transform.gameObject);
                //Check if in Range 
                if (!_activeChar.EnemiesInRange().Contains(hit.transform.gameObject.GetComponent<Playable>()))
                {
                    //If not in range move to location then attack?
                    mc.DoMovement(CursorController.Instance.transform.position, basic.Perform(_activeChar.transform.gameObject, targets));
                }
                else
                {


                    //Check if Enemy (or ally depending on skill?)

                    //Check if enough AP (1?)

                    StartCoroutine(basic.Perform(_activeChar.transform.gameObject, targets));
                }

            }
            else
                Debug.LogError("Character did not have a Movement Controller");

        }
    }
    private void MoveComplete()
    {
        ShowBattleMenu();
        CameraController.Instance.StopFollowingCharacter();
    }
    /**
     * Check if the Raycast Hit hit a playable object or not
     * Logic is temp and will be reworked
     * */
    private bool CheckPlayable(RaycastHit hit)
    {
        Playable p = hit.transform.GetComponent<Playable>();
        if (p)
        {
            if (p.IsActive())
            {
                _activeChar = null;
                _selectionState = eSelectionState.FREE;

            }
            return true;
        }
        return false;
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




    //DEBUGGING TMP STUFF
    //UI - TMP Debugging
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
}
