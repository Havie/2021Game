using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Skill : ScriptableObject
{
    [SerializeField] protected Vector3 _camOffset;

    [SerializeField] protected bool _cameraStarted;
    [SerializeField] protected bool _cameraDone;

    [SerializeField] protected int _skillCost;
    [SerializeField] protected float _power;


    public int GetSkillCost() => _skillCost;
    public float GetSkillPower() => _power;

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
