
// Animancer // Copyright 2020 Kybernetik //

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Animancer.Examples.Events
{
    /// <summary>
    /// An <see cref="GolfHitController"/> that uses Animancer Events configured entirely in the Inspector.
    /// </summary>
    [AddComponentMenu(Strings.MenuPrefix + "Examples/Golf Events - Animancer")]
    [HelpURL(Strings.APIDocumentationURL + ".Examples.AnimationEvents/GolfHitControllerAnimancer")]
    public sealed class cAnimator : MonoBehaviour
    {

        //https://www.youtube.com/watch?v=nnrOhb5UdRc for state machine logic
        private int _lastId;
        private Character.eDirection _direction= Character.eDirection.DOWN;

        /************************************************************************************************************************/
        #region AnimancerProperties
        // Without Animancer, you would reference an Animator component to control animations.
        // But with Animancer, you reference an AnimancerComponent instead.
        [SerializeField] private AnimancerComponent _Animancer;

        // Without Animancer, you would create an Animator Controller to define animation states and transitions.
        // But with Animancer, you can directly reference the AnimationClips you want to play.
        [SerializeField] private ClipState.Transition[] _idle;
        [SerializeField] private ClipState.Transition[] _walk;

        //Normals
        [SerializeField] private ClipState.Transition[] _S;
        [SerializeField] private ClipState.Transition[] _SS;
        [SerializeField] private ClipState.Transition[] _SSS;
        [SerializeField] private ClipState.Transition[] _SSSS;
        [SerializeField] private ClipState.Transition[] _SSSSS;
        [SerializeField] private ClipState.Transition[] _SSSSSS;
        //Strong
        [SerializeField] private ClipState.Transition _T;
        [SerializeField] private ClipState.Transition _ST;
        [SerializeField] private ClipState.Transition _SST;
        [SerializeField] private ClipState.Transition _SSST;
        [SerializeField] private ClipState.Transition _SSSST;
        [SerializeField] private ClipState.Transition _SSSSST;


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
            _Animancer.Play(_idle[0]).Speed = 1f;
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
            PlayAnim(0);
        }
        public void SetState(Character.eDirection direction)
        {
            //Debug.Log("Set Dir=" + direction);
            _direction = direction;
            PlayAnim(_lastId);
        }

        public void PlayAnim(int id)
        {
           // Debug.Log("PlayAnim" + id);
            switch (id)
            {
                case 0:
                    lastKnown.Clip = _idle[(int)_direction].Clip;
                    _Animancer.Play(lastKnown.Clip, 0.3f).Speed = 1f;
                    break;
                case 1:
                    lastKnown.Clip = _walk[(int)_direction].Clip;
                    _Animancer.Play(lastKnown.Clip, 0.3f).Speed = 1f;
                    break;
                case 2:
                    lastKnown.Clip = _S[(int)_direction].Clip;
                   _Animancer.Play(lastKnown.Clip, 0.3f).Speed = 1f;
                    // Time.timeScale = 0.5f;
                    break;
            }
            _lastId = id;
        }

        /************************************************************************************************************************/

        private void OnActionEnd()
        {

            if (lastKnown.Clip == _walk[(int)_direction].Clip)
                print("TRUE");
            // Now that the action is done, go back to idle. But instead of snapping to the new animation instantly,
            // tell it to fade gradually over 0.25 seconds so that it transitions smoothly.
            _Animancer.Play(_idle[(int)_direction], 0.25f).Speed = 0.15f;
        }

        /************************************************************************************************************************/
    }
}
