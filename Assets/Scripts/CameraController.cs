using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Singleton
    public static CameraController Instance { get; private set; }

    // Reference to the camera's center of rotation
    [SerializeField]
    private Transform _camRotCenterTrans;

    // How fast the camera should rotate from reading input.
    private const float ROT_SPEED = 1f;
    // The max and min tilt up and down values in Euler angles.
    private const float MAX_X_ROT = 90f;
    private const float MIN_X_ROT = 15f;
    // How fast the camera should move (per sec)
    private const float MOVE_SPEED = 0.07f;
    // How fast the camera should revolve
    private const float REV_SPEED = 130f;

    // The distance away to snap from
    private const float SNAP_DIST = 0f;
    // The rotation before snapping
    private const float ROT_SNAP_DIST = 1f;


    // Called when the component is enabled.
    // Subscribe to events.
    private void OnEnable()
    {
        cEventSystem.OnHasCameraRotInput += RotateCameraInputBased;
    }
    // Called when the component is disabled.
    // Unsubscribe from events.
    private void OnDisable()
    {
        cEventSystem.OnHasCameraRotInput -= RotateCameraInputBased;
    }
    // Called when the gameobject is destroyed.
    // Unsubscribe from ALL events.
    private void OnDestroy()
    {
        cEventSystem.OnHasCameraRotInput -= RotateCameraInputBased;
    }

    // Called 0th
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Debug.LogError("There should never be two CameraControllers in the scene");
            Destroy(this.gameObject);
        }
    }

    // Update is called once per frame
    private void Update()
    {
        /*
        // Read for Camera move input
        Vector2 camInp = InputController.GetCameraRotateAxis();
        if (camInp != Vector2.zero)
        {
            RotateCamera(camInp * ROT_SPEED);
        }
        */

        if (Input.GetKeyDown(KeyCode.Alpha1))
            RevolveFaceDirection(new Vector3(15, 90), true);
        if (Input.GetKeyDown(KeyCode.Alpha2))
            RevolveFaceDirection(new Vector3(15, 90), false);
        if (Input.GetKeyDown(KeyCode.Alpha3))
            SetCameraRotation(new Vector2(30, 0));
    }

    /// <summary>
    /// Calls RotateCamera using the input from InputController.
    /// </summary>
    private void RotateCameraInputBased()
    {
        // Read for Camera move input
        Vector2 camInp = InputController.GetCameraRotateAxis();
        if (camInp != Vector2.zero)
            RotateCamera(camInp * ROT_SPEED);
    }

    /// <summary>
    /// Rotates the camera by the given amount
    /// </summary>
    /// <param name="_rotAmount_">Amount to rotate the camera by on the x and y axes. Euler angles.</param>
    private void RotateCamera(Vector2 _rotAmount_)
    {
        // Set the rotation of the camera
        SetCameraRotation(GetAnglesIfRotated(_rotAmount_));
    }

    /// <summary>
    /// Returns the angles of the camera center if it was rotated the amount given.
    /// </summary>
    /// <param name="_rotAmount_">Amount to rotate the camera by on the x and y axes. Euler angles.</param>
    /// <returns>Vector3 Euler Angles of the potential rotation.</returns>
    private Vector3 GetAnglesIfRotated(Vector2 _rotAmount_)
    {
        Vector3 newAngles = _camRotCenterTrans.rotation.eulerAngles + new Vector3(_rotAmount_.x, _rotAmount_.y);
        if (newAngles.x < MIN_X_ROT)
            newAngles.x = MIN_X_ROT;
        else if (newAngles.x > MAX_X_ROT)
            newAngles.x = MAX_X_ROT;

        return newAngles;
    }

    /// <summary>
    /// Sets the rotation of the camera and calls the rotation event.
    /// </summary>
    /// <param name="_newRot_">Rotation to set the camera to in Euler angles.</param>
    private void SetCameraRotation(Vector2 _newRot_)
    {
        Quaternion newRotQuat = Quaternion.Euler(_newRot_);
        _camRotCenterTrans.rotation = newRotQuat;

        // Call the event for the camera rotating
        cEventSystem.CallOnCameraRotate();
    }

    /// <summary>
    /// Moves the camera such that the given position is the center of the camera view.
    /// </summary>
    /// <param name="_newPos_">New position for the camera view's center.</param>
    public void MoveCameraToPos(Vector3 _newPos_)
    {
        if (_newPos_.x != float.NegativeInfinity &&
            _newPos_.y != float.NegativeInfinity &&
            _newPos_.z != float.NegativeInfinity)
        {
            StopAllCoroutines();
            StartCoroutine(MoveCamera(_newPos_));
        }
    }


    /// <summary>
    /// Used with a coroutine to lerp the camera to the given position until it reaches it.
    /// </summary>
    /// <param name="_destPos_">Position for the camera to move to.</param>
    /// <returns>IEnumerator</returns>
    private IEnumerator MoveCamera(Vector3 _destPos_)
    {
        float dist = (_destPos_ - _camRotCenterTrans.position).magnitude;
        float tInc = MOVE_SPEED / dist;
        int timer = 0;
        while ((_camRotCenterTrans.position - _destPos_).magnitude > SNAP_DIST)
        {
            float t = Mathf.Min(tInc * ++timer, 1);
            _camRotCenterTrans.position = Vector3.Lerp(_camRotCenterTrans.position, _destPos_, t);

            /* This should now be done by subscribing to the OnCameraMove event
            //Decent solution ?
            if (UIBattleMenuController.Instance._isOn)
                UIBattleMenuController.Instance.ResetMenu();
                */

            // Call the event for the camera moving
            cEventSystem.CallOnCameraMove();

            yield return null;
        }

        _camRotCenterTrans.position = _destPos_;
        /*  if (UIBattleMenuController.Instance._isOn)
              UIBattleMenuController.Instance.ResetMenu();
              */

        // Call the event for the camera moving
        cEventSystem.CallOnCameraMove();

        yield return null;
    }

    /// <summary>
    /// Used with a coroutine to lerp the camera to the given position until it reaches it.
    /// </summary>
    /// <param name="_destTrans_">Transform of the object for the camera to move to.</param>
    /// <returns>IEnumerator</returns>
    private IEnumerator MoveCamera(Transform _destTrans_)
    {
        float dist = (_destTrans_.position - _camRotCenterTrans.position).magnitude;
        float tInc = MOVE_SPEED / dist;
        int timer = 0;
        while ((_camRotCenterTrans.position - _destTrans_.position).magnitude > SNAP_DIST)
        {
            float t = Mathf.Min(tInc * ++timer, 1);
            _camRotCenterTrans.position = Vector3.Lerp(_camRotCenterTrans.position, _destTrans_.position, t);

            // Call the event for the camera moving
            cEventSystem.CallOnCameraMove();

            yield return null;
        }

        _camRotCenterTrans.position = _destTrans_.position;

        // Call the event for the camera moving
        cEventSystem.CallOnCameraMove();

        yield return null;
    }

    /// <summary>
    /// Makes the camera start following a character.
    /// </summary>
    /// <param name="_charTrans_">Transform of the character.</param>
    public void BeginFollowingCharacter(Transform _charTrans_)
    {
        StopAllCoroutines();
        StartCoroutine(FollowCharacter(_charTrans_));
    }

    /// <summary>
    /// Stops the camera from following a character (ends the follow character coroutine).
    /// </summary>
    public void StopFollowingCharacter()
    {
        StopAllCoroutines();
    }

    /// <summary>
    /// Called via coroutine to follow the current character.
    /// Keeps following this character until StopFollowingCharacter is called or some other movement command is issued to the camera.
    /// </summary>
    /// <returns>IEnumerator</returns>
    private IEnumerator FollowCharacter(Transform _charToFollow_)
    {
        yield return MoveCamera(_charToFollow_);

        while (true)
        {
            _camRotCenterTrans.transform.position = _charToFollow_.position;

            // Call the event for the camera moving
            cEventSystem.CallOnCameraMove();

            yield return null;
        }
    }

    /// <summary>
    /// Recenters the camera on the cursor.
    /// </summary>
    public void RecenterOnCursor()
    {
        BeginFollowingCharacter(CursorController.Instance.transform);
    }

    /// <summary>
    /// Starts the coroutine to revolve the camera.
    /// </summary>
    /// <param name="_targetRot_">The rotation the camera will move to look to.</param>
    /// <param name="_longRotation_">If the camrea should take the long path or the shortest path.
    /// Basically changes if the camera spins left or right.</param>
    public void RevolveFaceDirection(Vector3 _targetRot_, bool _longRotation_)
    {
        StartCoroutine(RevolveCoroutine(_targetRot_, _longRotation_));
    }

    /// <summary>
    /// Coroutine to revolve the camera until it is facing the given direction.
    /// </summary>
    /// <param name="_targetRot_">The rotation the camera will move to look to.</param>
    /// <param name="_longWayRound_">If the camrea should take the long path or the shortest path.
    /// Basically changes if the camera spins left or right.</param>
    /// <returns>IEnumerator</returns>
    public IEnumerator RevolveCoroutine(Vector3 _targetRot_, bool _longWayRound_)
    {
        Debug.Log("Target Rotation: " + _targetRot_ + ". Current Rotation: " + _camRotCenterTrans.rotation.eulerAngles);
        Debug.Log("Is Long? " + _longWayRound_);
        // The direction the camera will rotation.
        Vector3 moveDir = new Vector3();
        // If we are taking the long way around, the camera will rotate in the opposite y direction.
        if (_longWayRound_)
        {
            Vector3 newTarget = _targetRot_ + new Vector3(0, -360, 0);
            moveDir = (newTarget - _camRotCenterTrans.rotation.eulerAngles).normalized;
        }
        // Otherwise, its just normal
        else
        {
            moveDir = (_targetRot_ - _camRotCenterTrans.transform.rotation.eulerAngles).normalized;
        }

        // Precalculate the amount we will be rotating each time.
        Vector3 rotDir = moveDir * REV_SPEED;
        // Rotate the camera until it starts getting farther away.
        float lastMag = float.MaxValue;
        float nextMag = lastMag;
        while (lastMag >= nextMag)
        {
            // Rotate the camera a small bit.
            RotateCamera(rotDir * Time.deltaTime);

            // Update these values to keep track of if the magnitude is increasing or decreasing.
            //lastMag = nextMag;
            //nextMag = (_targetRot_ - GetAnglesIfRotated(rotDir * Time.deltaTime)).magnitude;

            lastMag = nextMag;
            nextMag = (_targetRot_ - _camRotCenterTrans.eulerAngles).magnitude;

            //Debug.Log("Last: " + lastMag + ". Next: " + nextMag);

            yield return null;
        }

        // Set the camera's rotation to be exactly the target rotation.
        SetCameraRotation(_targetRot_);

        // Call the finish revolving event
        cEventSystem.CallOnCameraFinishRevolution();
    }
}
