using UnityEngine;

public class Playable : MonoBehaviour
{
    public bool _isActive;
    public bool _isSelected;
    private TurnManager _turnManager;


    private void Awake()
    {
        //This could be on a faction OR a Character so we have to operate on GameObject?
        //Could try to getComponent character vs faction to set a bool that will tell this script how to operate.
    }

    public bool IsActive() => _isActive;
    public bool IsSelected() => _isSelected;

    public void SetActive(bool cond) { _isActive = cond; }
    public void SetSelected(bool cond) { _isSelected = cond; }

    public void YourTurn(TurnManager t)
    {
        _isActive = true;
        //Tell the UI its your turn 

        //Tell the Camera to look here 

        //subscribe to  event system that interacts with an end turn button
        //-- could base WHICH event u sub to based on faction vs char
    }

    public void EndTurn()
    {
        _isActive = false;

        //unsubscribe from event system 

        _turnManager.Next();
    }

}
