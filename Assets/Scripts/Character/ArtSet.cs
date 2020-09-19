using Animancer;
using System.Collections.Generic;
using UnityEngine;


public class ArtSet : MonoBehaviour
{

    public Sprite _portrait;


    #region animations
    [SerializeField] AnimSet _idle;
    [SerializeField] AnimSet _walk;
    [SerializeField] AnimSet _basicAttack;
    [SerializeField] AnimSet _hit;
    [SerializeField] AnimSet _death;
    [SerializeField] AnimSet _burst;

    #endregion


    [SerializeField] private ClipState.Transition[][] _animations;

    private void Awake()
    {
        Init();
    }

    public void Init()
    {
        //Does this work???
        _animations = new ClipState.Transition[][] { _idle.GetAnimSet(), _walk.GetAnimSet(), _basicAttack.GetAnimSet(), _hit.GetAnimSet(), _death.GetAnimSet(), _burst.GetAnimSet() };
        //Tell the animator about us 
        cAnimator animator = this.GetComponent<cAnimator>();
        if (animator)
        {
            animator.SetArtSet(this);
        }

    }


    public Sprite GetPortrait() => _portrait;

    /// <summary>
    /// Returns the array of clips for the animset at an index
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public ClipState.Transition[] GetAnimation(int index)
    {
        if ((int)index > _animations.Length)
            return null;

        return _animations[(int)index];
    }

}
