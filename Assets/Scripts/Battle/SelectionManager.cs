using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{

    public enum SelectionState { Free, Move, Attack};
    private SelectionState _selectionState = SelectionState.Free;

    Playable _activeChar;

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
            case SelectionState.Free:
                {
                    FreeClick(mousePos);
                    break;
                }
            case SelectionState.Move:
                {
                    MoveClick(mousePos);
                    break;
                }
            case SelectionState.Attack:
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
                if(p.isActive())
                {
                    Debug.Log("IT's" + p.gameObject.name + "'s  Turn!");
                    //Not sure if its important turning this on
                    MovementController mc = p.GetComponent<MovementController>();
                    if (mc)
                        mc.enabled = true;
                    _activeChar = p;
                    _selectionState = SelectionState.Move;
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
            if (p.isActive())
            {
                _activeChar = null;
                _selectionState = SelectionState.Free;
               
            }
            return true;
        }
        return false;
    }
}
