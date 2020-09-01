
using System.Collections.Generic;
using UnityEngine;


//Responsible for turning on the animator attached to General[0] 
//manages campaign movement ??

public class Army : MonoBehaviour
{
    Faction _faction;
    [SerializeField] Officer[] _generals = new Officer[5];
    int _movementPoints;


    void OnEnable()
    {
        //Show our commanders avatar on the campaign map
        for (int i = 0; i < _generals.Length; ++i)
        {
            Officer general = _generals[i];
            if (general != null)
            {
                cAnimator animator= general.GetComponent<cAnimator>();
                if (animator)
                    animator.enabled = (i == 0);
                //We might want to turn off the capsule collider too so we dont click
            }
        }
    }

    //Average the movement points of army 
    public int GetMovementPointsMax()
    {
        int totalMovement = 0;
        int count = 0;
        for (int i = 0; i < _generals.Length; ++i)
        {
            Officer general = _generals[i];
            if (general != null)
            {
                //totalMovement += general.GetAP();
                //Find New Solution
                ++count;
            }
        }

        return count != 0 ? totalMovement / count : 0;
    }
    public int GetMovementPointsRemaining() => _movementPoints;
    public void SetMovementPointsRemaining(int points) { _movementPoints = points; }


    //Make this subscribe to an event system
    public void NewTurn()
    {
        _movementPoints = GetMovementPointsMax();
    }


}
