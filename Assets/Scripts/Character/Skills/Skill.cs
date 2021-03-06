﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Skill : ScriptableObject
{
    [SerializeField] protected string _name;

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
    [SerializeField] protected bool _useImmediate;

    [SerializeField] protected cAnimator.AnimationID _animationID;
    public enum eSkillEffect { NONE, PUSHBACK ,SWITCH, PRESS };
    [SerializeField] protected eSkillEffect[] _effects;

    public string GetName() => _name;
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
    /// <returns>int </returns>
    public int GetRadius() => _radius;

    /// <summary>
    /// Whether the skill targets allies or enemies
    /// </summary>
    /// <returns>bool </returns>
    public bool GetIsFriendly() => _isFriendly;

    /// <summary>
    /// Tells the selection manager if the skill requires further input
    /// </summary>
    /// <returns>bool</returns>
    public bool GetIsUseImmediate() => _useImmediate;

    /// <summary>
    /// Main Skill logic to perform the action
    /// Functions off of its derived assests variables
    /// </summary>
    /// <param name="self"></param>
    /// <param name="targets"></param>
    /// <returns></returns>
    public virtual  IEnumerator Perform(GameObject self, List<GameObject> targets)
    {
        Debug.Log("Perform " + _name);
        

        if (targets == null)
            Debug.LogWarning("targets null");

        if (targets[0] == null)
            Debug.LogWarning("targets[0] null");


        //Save Camera initial position
        Vector3 cameraStartPos = CameraController.Instance.transform.position;
        Vector3 cameraStartRot = CameraController.Instance.transform.rotation.eulerAngles;
        float cameraStartFOV = CameraController.Instance.GetCamera().fieldOfView;
        //Play Camera and wait till its done 

        //Make chars face eachother
        self.GetComponentInChildren<EightDir>().LookAt(targets[0].transform);
        if(_range==0) //Melee 
             targets[0].GetComponentInChildren<EightDir>().LookAt(self.transform);

        //Play Camera and wait till its done 
        //Rotate the camera to focal point for combat  //Will need tweaking if no target
        OpeningCameraAnimation(self.GetComponentInChildren<EightDir>(), targets[0].GetComponentInChildren<EightDir>());

        while (!_cameraDone) 
            {yield return new WaitForEndOfFrame();}

        //Play Animations and wait till they are ready for damage
        cAnimator sAnimator = self.GetComponentInChildren<cAnimator>();
        List<cAnimator> returnToIdles = new List<cAnimator>(); // not sure this is ideal
        float hitAnimTime = 0;
        if (sAnimator)
        {
            //75% thru the animation begin to apply damage
            yield return new WaitForSeconds
                (sAnimator.PlayAnim(_animationID) * 0.75f);
            //Apply Damage and any effects
            TroopContainer attacker = self.GetComponent<TroopContainer>();
            foreach (GameObject g in targets)
            {
                TroopContainer defender = g.GetComponent<TroopContainer>();

                if (defender && attacker)
                    defender.IncrementTroops(0-CalculateDamage(attacker, defender));
                else
                    Debug.Log("Missing defender" + g.name  + " , a:" + self.name);

                cAnimator dAnimator = g.GetComponentInChildren<cAnimator>();
                returnToIdles.Add(dAnimator);
                float time= dAnimator.PlayAnim(cAnimator.AnimationID.HITREACTION);

                if (time > hitAnimTime)
                    hitAnimTime = time;

                HandleEffects(self, g);
            }
            sAnimator.ReturnToIdle();
        }

        //TODO wait for damage to be applied and anims finish
        yield return new WaitForSeconds(hitAnimTime);
        foreach (var animator in returnToIdles)
            animator.ReturnToIdle();

        //TODO wait for some UI to clear 
        yield return new WaitForSeconds(1);

        //Play Closing Camera animation to reset back to where player had camera 
        ClosingCameraAnimation(cameraStartPos, cameraStartRot, false, cameraStartFOV);

        while (!_cameraDone)
            { yield return new WaitForEndOfFrame(); }

        //Let someone know we're done 
        cEventSystem.CallOnAttackFinished();
    }
    protected int CalculateDamage(TroopContainer attacker, TroopContainer defender)
    {
        float dmg = attacker.GetAttack();
        float def = defender.GetDefense();


        int atkWeight = attacker.GetHP();
        int defWeight = defender.GetHP();

        //TMP idk 
        float result = Mathf.Max(((_power * dmg) + atkWeight) - (def + defWeight/2), 0);

        Debug.Log("DAMGE= " + result);
        return (int)result;
    }


    protected void OpeningCameraAnimation(EightDir selfDir, EightDir targetDir)
    {
        //TypeOfRotation unused but will hopefully dictate the different camera views 

        _cameraDone = false;
        cEventSystem.OnCameraFinishRevolution += CameraFinished;
        CameraController.Instance.SideFrameTwoCharacters(selfDir, targetDir);

        //Old way
        /* CoroutineManager.Instance.StartThread(
              CameraController.Instance.RevolveCoroutine(destination, false)
              ); */
    }

    protected void ClosingCameraAnimation(Vector3 cameraStartPos, Vector3 cameraStartRot, bool cond,  float cameraStartFOV)
    {
        _cameraDone = false;
        cEventSystem.OnCameraFinishRevolution += CameraFinished;
        CameraController.Instance.MoveRevolveAndZoomCamera(cameraStartPos, cameraStartRot, false, cameraStartFOV);
    }

    protected void CameraFinished()
    {
        _cameraDone = true;
        cEventSystem.OnCameraFinishRevolution -= CameraFinished;
    }

    protected void HandleEffects(GameObject self, GameObject target)
    {
        if (_effects == null)
            return;
        foreach (eSkillEffect effect in _effects)
        {
            switch(effect)
            {
                case eSkillEffect.PRESS:
                    {
                        CoroutineManager.Instance.StartThread(Press(target, self));
                        break;
                    }
                case eSkillEffect.PUSHBACK:
                    {
                        CoroutineManager.Instance.StartThread(PushBack(target, self.transform));
                        break;
                    }
                case eSkillEffect.SWITCH:
                    {
                        break;
                    }
            }
        }
    }
    protected IEnumerator Press(GameObject targetToPush, GameObject Presser) //Might need to be an Ienumerator 
    {
        yield return null;
        Debug.LogWarning("TODO Make this lerp or look good with an animation");
        float disToPush = 2.75f;
        Vector3 EndPoint = (targetToPush.transform.position - Presser.transform.position).normalized;

        // Presser.transform.position = targetToPush.transform.position;
        // targetToPush.transform.position = targetToPush.transform.position+ (EndPoint * disToPush);

        Presser.GetComponentInParent<MovementController>().DoMovement(targetToPush.transform.position, AttackDone);
        targetToPush.GetComponentInParent<MovementController>().DoMovement(targetToPush.transform.position + (EndPoint * disToPush), AttackDone);
    }

    protected IEnumerator PushBack(GameObject targetToPush, Transform LocationFrom) //Might need to be an Ienumerator 
    {
        yield return null;
        Debug.LogWarning("TODO Make this lerp or look good with an animation");
        float disToPush = 2.85f;
        Vector3 EndPoint = (targetToPush.transform.position - LocationFrom.position).normalized;

        targetToPush.GetComponentInParent<MovementController>().DoMovement(targetToPush.transform.position + (EndPoint * disToPush), AttackDone);

       // targetToPush.transform.position = targetToPush.transform.position + (EndPoint * disToPush);
    }

    private void AttackDone()
    {
        //TMP
    }
}
