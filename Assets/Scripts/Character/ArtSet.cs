using Animancer;
using Animancer.Examples.Events;
using System.Collections.Generic;
using UnityEngine;


public class ArtSet : MonoBehaviour
{

    public Sprite _portrait;


    #region animations
    [SerializeField] ClipState.Transition[] _idle;
    [SerializeField] ClipState.Transition[] _walk;
    [SerializeField] ClipState.Transition[] _attack;
    [SerializeField] ClipState.Transition[] _death;
    [SerializeField] ClipState.Transition[] _burst;

    #endregion

    public enum eAnimType { IDLE, WALK, ATTACK , DEATH, BURST
}
    //If this mess doesn't work use a HM
    [SerializeField] private ClipState.Transition[][] _animations ;



    public void Init()
    {
        //Does this work???
        _animations = new ClipState.Transition[][]{ _idle, _walk, _attack, _death, _burst };
        //Tell the animator about us 
        cAnimator animator = this.GetComponent<cAnimator>();
        if (animator)
        {
            animator.SetArtSet(this);
        }

    }   


public Sprite GetPortrait() => _portrait;

public ClipState.Transition[] GetAnimation(eAnimType index)
{
    if ((int)index > _animations.Length)
        return null;

    return _animations[(int)index];
}

}
