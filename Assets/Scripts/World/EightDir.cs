using Animancer.Examples.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class EightDir : Billboard
{
    [SerializeField]
    private AnimationClip[] animationClips = new AnimationClip[8];
    private cAnimator anim;

    public enum eDirection { DOWN, DOWN_RIGHT, RIGHT, UP_RIGHT, UP, UP_LEFT, LEFT, DOWN_LEFT };


    private eDirection currentState;
    private float lastYRot;

    private const int SINGLE_ROT = 360 / 8;

    private void Awake()
    {
        anim = this.GetComponent<cAnimator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        currentState = eDirection.DOWN;
        anim.SetState(currentState);

        lastYRot = this.transform.rotation.eulerAngles.y;
    }

    protected override void UpdateSprite()
    {
        base.UpdateSprite();

        float curRotY = this.transform.rotation.eulerAngles.y;
        lastYRot = curRotY;

        currentState = eDirection.DOWN + Mathf.RoundToInt(curRotY / SINGLE_ROT) % 8;

       // anim.setState(animationClips[(int)currentState].name);
        anim.SetState(currentState);
    }
}
