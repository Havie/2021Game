using UnityEngine;

[RequireComponent(typeof(cAnimator))]
public class EightDir : Billboard
{
    public enum eDirection { DOWN, DOWN_RIGHT, RIGHT, UP_RIGHT, UP, UP_LEFT, LEFT, DOWN_LEFT };


    // Constant for how much to divide the rotation by to convert to a eDirecton
    private const int SINGLE_ROT = 360 / 8;

    // Reference to the animator
    private cAnimator _anim;

    // The looking rotation of the character to offset
    private Vector3 _lookingAngles;
    // The current state of which way we are looking
    private eDirection _currentState;

    // Called 0th
    private void Awake()
    { 
        _anim = this.GetComponent<cAnimator>();
    }

    // Called 1st
    private new void Start()
    {
        base.Start();

        _lookingAngles = new Vector3();
        _currentState = eDirection.DOWN;
        _anim.SetState(_currentState);
    }

    /// <summary>
    /// Updates the sprite of the eight directional object.
    /// </summary>
    protected override void UpdateSprite()
    {
        // Call the base
        base.UpdateSprite();

        // Update the current state of the direction
        float curRotY = this.transform.rotation.eulerAngles.y + _lookingAngles.y;
        while (curRotY < 0)
            curRotY += 360;
        _currentState = eDirection.DOWN + Mathf.RoundToInt(curRotY / SINGLE_ROT) % 8;

        // Set the state of the animation based on the current state
        _anim.SetState(_currentState);
    }

    /// <summary>
    /// Set the looking angle to the given vector.
    /// </summary>
    /// <param name="_newLookAngles_">New angles to look at</param>
    public void ChangeLookingAngle(Vector3 _newLookAngles_)
    {
        // TODO Probably have to add an event call, or some animator stuff in here eventually

        _lookingAngles = _newLookAngles_;
    }

    /// <summary>
    /// Sets the looking angles of the EightDir to be facing the given transform.
    /// </summary>
    /// <param name="_targetToLookAt_">Transform for this EightDir to look at.</param>
    public void LookAt(Transform _targetToLookAt_)
    {
        // Get the positions of the two points.
        Vector3 oriPos = this.transform.position;
        Vector3 targetPos = _targetToLookAt_.position;

        // Get the hypotenuse of the two points make.
        Vector3 triVect = targetPos - oriPos;

        // Use Atan2 and an offset of 90 to get the angle we want the eight dir to be facing
        Vector3 newLookAngles = new Vector3();
        newLookAngles.y = Mathf.Atan2(triVect.z, triVect.x) * Mathf.Rad2Deg + 90;

        // Change the looking angle.
        ChangeLookingAngle(newLookAngles);
    }
}
