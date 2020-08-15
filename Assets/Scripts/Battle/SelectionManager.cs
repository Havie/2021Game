using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    private static SelectionManager _instance;

    public Material _normal;
    public Material _selected;
    public Material _allied;
    public Material _enemy;

    public enum SelectionState { FREE, MOVE, ATTACK, MENU};
    private SelectionState _selectionState = SelectionState.FREE;

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
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (InputController.GetSelectPressDown())
            HandleClick(InputController.GetCursorPosition());
    }
    private void HandleClick(Vector3 mousePos)
    {
        switch (_selectionState)
        {
            case SelectionState.FREE:
                {
                    FreeClick(mousePos);
                    break;
                }
            case SelectionState.MOVE:
                {
                    MoveClick(mousePos);
                    break;
                }
            case SelectionState.ATTACK:
                {
                    break;
                }
            case SelectionState.MENU:
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
        //Turn on the characters Menu
        _selectionState = SelectionState.MENU;
    }
    private void SetSelected(Playable p, bool cond)
    {
        if (p == null)
            return;

        //It will probably already be off in some cases?
        //Might want to handle this inside the PLayable script-we'll see 
        p.ShowBattleMenu(cond);

        SpriteRenderer sp = p.GetComponent<SpriteRenderer>();
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

        //ToDo
        //Tell the camera where to look
    }

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
        Debug.Log("MoveClick");
        if (_activeChar)
        {
            Ray ray = Camera.main.ScreenPointToRay(mousePos);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
               Debug.DrawRay(Camera.main.transform.position, ray.direction * 10, Color.red, 10);
               if(!CheckPlayable(hit))
                {
                    if (hit.transform.gameObject.tag.Equals("Ground"))
                    {
                        var mc = _activeChar.GetComponent<MovementController>();
                        if (mc)
                            mc.DoMovement(InputController.GetCursorRayWorldPosition());
                    }
                }
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
                _selectionState = SelectionState.FREE;
               
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
            _selectionState = SelectionState.MOVE;
        else
            _selectionState = SelectionState.FREE; // might need diff logic
    }
}
