using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Skill : ScriptableObject
{
    
    [SerializeField] protected Vector3 _camOffset; //Half implemented, might need different views for different skills 

    [SerializeField] protected bool _cameraStarted;
    [SerializeField] protected bool _cameraDone;

    [SerializeField] protected int _apCost=1; 
    [SerializeField] protected float _power; //Damage Modifier

    [SerializeField] protected bool _requiresTarget=true; //AOE or not 
    [SerializeField] protected int _numTargets=1;  //If Poleaxe can AOE on strikes etc
    [SerializeField] protected int _range=0; //0 is melee, else ranged
    [SerializeField] protected int _radius=1; //area of effect 

    [SerializeField] protected bool _isFriendly;

    public int GetAPCost() => _apCost;
    public float GetSkillPower() => _power;
    public bool GetRequiresTarget() => _requiresTarget;
    public int GetNumTargets() => _numTargets;
    /// <summary>
    /// If 0 Skill is Melee, Else Ranged
    /// </summary>
    /// <returns> int </returns>
    public int GetRange() => _range;
    /// <summary>
    /// The area of effect, Should be 1 by default.
    /// </summary>
    /// <returns></returns>
    public int GetRadius() => _radius;

    /// <summary>
    /// Whether the skill targets allies or enemies?
    /// </summary>
    /// <returns></returns>
    public bool GetIsFriendly() => _isFriendly;

    public virtual  IEnumerator Perform(GameObject self, List<GameObject> targets)
    {
        yield return null;
        Debug.LogWarning("DEFAULT implementation");
    }

    protected IEnumerator CameraMovement(int option, Vector3 location)
    {

        yield return new WaitForSeconds(1);
     
        //Save starting position somewhere? to return to it?

        //Write own lerp and manually move Camera in this coroutine
        //To Do Wyatt? <3
        CameraController.Instance.MoveCameraToPos(location);

        //Let whatever is waiting on us know were done
        _cameraDone = true;

    }

    protected void PushBack() //Might need to be an Ienumerator 
    {

    }
}
