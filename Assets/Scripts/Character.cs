using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum AnimationState { DOWN, DOWN_RIGHT, RIGHT, UP_RIGHT, UP, UP_LEFT, LEFT, DOWN_LEFT};

public class Character : WorldObj
{
    [SerializeField]
    private AnimationClip[] animationClips = new AnimationClip[8];
    private Animator anim;

    private AnimationState currentState;
    private float lastYRot;

    private const int SINGLE_ROT = 360 / 8;

    private void Awake()
    {
        anim = this.GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        currentState = AnimationState.DOWN;
        anim.Play(animationClips[(int)currentState].name);

        lastYRot = this.transform.rotation.eulerAngles.y;
    }

    protected override void UpdateSprite()
    {
        base.UpdateSprite();

        float curRotY = this.transform.rotation.eulerAngles.y;
        lastYRot = curRotY;

        currentState = AnimationState.DOWN + Mathf.RoundToInt(curRotY / SINGLE_ROT) % 8;

        anim.Play(animationClips[(int)currentState].name);
    }
}
