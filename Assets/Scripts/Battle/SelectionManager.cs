using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{

    public Material _normal;
    public Material _selected;
    public Material _allied;
    public Material _enemy;

    public enum SelectionState { FREE, MOVE, ATTACK};
    private SelectionState _selectionState = SelectionState.FREE;

    Playable _activeChar;


    private void Awake()
    {
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
            default:
                break;
        }
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
                if(p.IsActive())
                {
                    Debug.Log("IT's" + p.gameObject.name + "'s  Turn!");
                    //Not sure if its important turning this on
                    SpriteRenderer sp = _activeChar.GetComponent<SpriteRenderer>();
                    if (sp)
                    {
                        if(_activeChar.IsSelected())
                        {
                            _activeChar.SetSelected(false);
                            sp.material = _normal;
                        }
                        else
                        {
                            _activeChar.SetSelected(true);
                            sp.material = _selected;
                        }
                    }
                    else
                        Debug.LogWarning("cant find sprite renderer for "+_activeChar.gameObject);

                }
            }
        }
    }
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
