using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{

    public enum SelectionState { Campaign, Battle, Pause};
    private SelectionState _selectionState = SelectionState.Battle;



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
            case SelectionState.Campaign:
                {
                    break;
                }
            case SelectionState.Battle:
                {
                    BattleClick(mousePos);
                    break;
                }
            case SelectionState.Pause:
                {
                    break;
                }
            default:
                break;
        }
    }

    private void BattleClick(Vector3 mousePos)
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
                    MovementController mc = p.GetComponent<MovementController>();
                    if (mc)
                        mc.DoMovement(mousePos);
                }
            }
        }
    }
}
