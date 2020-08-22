using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectionManager : MonoBehaviour
{
    private static SelectionManager _instance;

    public CameraController _camera;

    public Material _normal;
    public Material _selected;
    public Material _allied;
    public Material _enemy;

    public enum eSelectionState { FREE, MOVE, ATTACK, MENU};
    private eSelectionState _selectionState = eSelectionState.FREE;

    Playable _activeChar;

    public static SelectionManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = GameObject.FindObjectOfType<SelectionManager>();
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else if (_instance != this)
            Destroy(this);

        if (_normal == null)
            Debug.LogError("Normal resource is null, No idea how to load from package folder, assign in inspector");
        if(_selected==null)
            _selected= Resources.Load<Material>("Sprites/SelectionOutline");
        if (_allied == null)
            _allied = Resources.Load<Material>("Sprites/SelectionOutline_Allied");
        if (_enemy == null)
            _enemy = Resources.Load<Material>("Sprites/SelectionOutline_Enemy");

        if (_camera == null)
            _camera = Camera.main.GetComponentInParent<CameraController>();
    }
    void Start()
    {
        if (_camera == null)
            Debug.LogWarning("camera is null, May have gotten moved from parent back to camera");
    }

    // Update is called once per frame
    void Update()
    {
        if (InputController.GetSelectPressDown())
            HandleClick(InputController.GetCursorPosition());

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(_selectionState==eSelectionState.MOVE)
            {
                ShowBattleMenu();
            }
        }
    }
    private void HandleClick(Vector3 mousePos)
    {

        //TellMyUIClick(mousePos);
        switch (_selectionState)
        {
            case eSelectionState.FREE:
                {
                    FreeClick(mousePos);
                    break;
                }
            case eSelectionState.MOVE:
                {
                    MoveClick(mousePos);
                    break;
                }
            case eSelectionState.ATTACK:
                {
                    break;
                }
            case eSelectionState.MENU:
                {
                    break;
                }
            default:
                break;
        }
    }

    public void SetActiveCharacter(Playable character)
    {
        //Clear the old character
        if(_activeChar)
            SetSelected(_activeChar, false);
        //Assign the new 
        if(character)
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
            if(_selectionState == eSelectionState.MOVE)
            {
                CreateMovementLine.Instance.DisablePathPreview();
                //this method doesnt disable the last known path Preview (TODO)
            }

            _selectionState = eSelectionState.MENU;
            UIBattleMenuController.Instance.ShowMenu(true, _activeChar.transform.position, _activeChar.name);
        }
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
                sp.material = _selected;
            else
                sp.material = _normal;
        }
        else
            Debug.LogWarning("Cant find Sprite Render for " + p.gameObject.name);

        //Tell the camera where to look
        if (_camera && cond)
            _camera.MoveCameraToPos(p.transform.position);
    }
    public void EnableMove(bool cond)
    {
        _selectionState = eSelectionState.MOVE;
        CursorController.Instance.ToggleCursor(cond);
        //Turn on/off the Menu
        if (_activeChar)
        {
            UIBattleMenuController.Instance.ShowMenu(false, _activeChar.transform.position);
            CreateMovementLine.Instance.EnablePathPreview(_activeChar.GetComponent<MovementController>());
            _camera.SetFollowTarget(cond, _activeChar.transform);

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
            Debug.DrawRay(Camera.main.transform.position, ray.direction *10, Color.red, 10);
            Playable p = hit.transform.GetComponent<Playable>();
            if(p)
            {
                //This logics all ghetto going to rewrite from menu system
                if(p.IsActive() && _activeChar)
                    SetSelected(_activeChar, _activeChar.IsSelected());
                else if (p.IsActive() && !_activeChar)
                    SetSelected(p, true);
            }
        }
    }
    /**
     * Move active character to a location that isnt a playable
     * Might have to rework logic if we want characters to move to and attack in 1 command
     * */
    private void MoveClick(Vector3 mousePos)
    {
       // Debug.Log("MoveClick");
        if(_activeChar)
        {
            
            MovementController mc = _activeChar.GetComponent<MovementController>();
            if (mc)
            {
                // Hide the cursor and stop drawing a path
                CreateMovementLine.Instance.DisablePathPreview();
                CursorController.Instance.ToggleCursor(false);

                // Follow the character who will move
                CameraController.Instance.BeginFollowingCharacter(mc.transform);
                mc.DoMovement(CursorController.Instance.transform.position);
            }
            else
            {
                Debug.LogError("Character did no have a Movement Controller");
            }

        }
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
        MovementController mc = _activeChar.GetComponent<MovementController>();
        if (mc)
            mc.enabled = cond;
        if(cond)
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
