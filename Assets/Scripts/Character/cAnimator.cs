
// Animancer // Copyright 2020 Kybernetik //

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.

using Animancer;
using System;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// An <see cref="GolfHitController"/> that uses Animancer Events configured entirely in the Inspector.
/// </summary>
[AddComponentMenu(Strings.MenuPrefix + "Examples/Golf Events - Animancer")]
[HelpURL(Strings.APIDocumentationURL + ".Examples.AnimationEvents/GolfHitControllerAnimancer")]
public sealed class cAnimator : MonoBehaviour
{

    //https://www.youtube.com/watch?v=nnrOhb5UdRc for state machine logic
    private int _lastId;
    private EightDir.eDirection _direction = EightDir.eDirection.DOWN;
    private ArtSet _artset;

    public enum AnimationID { IDLE, WALK, BASICATTACK, HITREACTION , DEATH};

    /************************************************************************************************************************/
    #region AnimancerProperties
    // Without Animancer, you would reference an Animator component to control animations.
    // But with Animancer, you reference an AnimancerComponent instead.
    [SerializeField] private AnimancerComponent _Animancer;

    [SerializeField] private ClipState.Transition lastKnown;


    [SerializeField] private SimpleEventReceiver _EventReceiver;
    #endregion
    /************************************************************************************************************************/

    /// <summary>
    /// On startup, play the idle animation.
    /// </summary>
    private void OnEnable()
    {
        if (_Animancer == null)
            _Animancer = this.GetComponent<AnimancerComponent>();
        // On startup, play the idle animation.
        ReturnToIdle();
    }
    public void SetArtSet(ArtSet a)
    {
        _artset = a;
    }
    private void Update()
    {
        // Every update, check if the user has clicked the left mouse button (mouse button 0).
        /* 
            if (Input.GetMouseButtonDown(0))
            {
                // If they have, then play the action animation.
                lastKnown = _Animancer.Play(_Action, 0.25f);
                lastKnown.Speed = 0.5f;

                // The Play method returns the AnimancerState which manages that animation so you can access and
                // control various details, for example:
                // state.Time = 1;// Skip 1 second into the animation.
                // state.NormalizedTime = 0.5f;// Skip halfway into the animation.
                // state.Speed = 2;// Play the animation twice as fast.

                // In this case, we just want it to call the OnActionEnd method (see below) when the animation ends.
                lastKnown.Events.OnEnd = OnActionEnd;
            } 
            */
    }
    public void ReturnToIdle()
    {
        PlayAnim(AnimationID.IDLE);
    }
    public void SetState(EightDir.eDirection direction)
    {
        //Debug.Log("Set Dir=" + direction);
        _direction = direction;
        PlayAnim(_lastId);
    }
    public float PlayAnim(AnimationID anim)
    {
        float duration=  PlayAnim((int)anim);
        if(anim== AnimationID.DEATH)
        {
            StartCoroutine(DeathDelay(duration));
        }

        return duration;
    }

    private float PlayAnim(int id)
    {
       //Debug.Log("PLAY ID=" + id);
        if (_artset == null)
            return 0;


        _lastId = id;
        lastKnown.Clip = _artset.GetAnimation(_lastId)[(int)_direction].Clip;
        _Animancer.Play(lastKnown.Clip, 0.3f).Speed = 1f;
        return lastKnown.Clip.length;

    }

    /************************************************************************************************************************/
    //Don't think fading works on 2d Sprites, only 3D models
    private void OnActionEnd()
    {

        if (lastKnown.Clip == _artset.GetAnimation(1)[(int)_direction].Clip);
            print("TRUE");
        // Now that the action is done, go back to idle. But instead of snapping to the new animation instantly,
        // tell it to fade gradually over 0.25 seconds so that it transitions smoothly.
        _Animancer.Play(_artset.GetAnimation(0)[(int)_direction], 0.25f).Speed = 0.15f;
    }

    //Turn Let the game know were dead after our animation
    private System.Collections.IEnumerator DeathDelay(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        // this.enabled = false;
        cEventSystem.CallOnCharacterDeath(this.transform.parent.gameObject);
    }

    /************************************************************************************************************************/
}


