using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Reference to the camera's center of rotation
    [SerializeField]
    private Transform camRotCenterTrans;

    // How fast the camera should rotate from reading input.
    private float ROT_SPEED = 1f;

    private float MAX_X_ROT = 90f;
    private float MIN_X_ROT = 15f;

    // Update is called once per frame
    private void Update()
    {
        // Read for Camera move input
        Vector2 camInp = InputController.GetCameraRotateAxis();
        if (camInp != Vector2.zero)
        {
            RotateCamera(camInp * ROT_SPEED);
        }

        if (Input.GetMouseButtonDown(1))
        {
            MoveCameraToPos(InputController.GetCursorRayWorldPosition());
        }
    }

    /// <summary>
    /// Rotates the camera by the given amount
    /// </summary>
    /// <param name="_rotAmount_">Amount to rotate the camera by on the x and y axes. Euler angles.</param>
    private void RotateCamera(Vector2 _rotAmount_)
    {
        Vector3 newAngles = camRotCenterTrans.rotation.eulerAngles + new Vector3(_rotAmount_.x, _rotAmount_.y);
        if (newAngles.x < MIN_X_ROT)
            newAngles.x = MIN_X_ROT;
        else if (newAngles.x > MAX_X_ROT)
            newAngles.x = MAX_X_ROT;

        Quaternion newRot = Quaternion.Euler(newAngles);
        camRotCenterTrans.rotation = newRot;
    }

    /// <summary>
    /// Moves the camera such that the given position is the center of the camera view.
    /// </summary>
    /// <param name="_newPos_">New position for the camera view's center.</param>
    public void MoveCameraToPos(Vector3 _newPos_)
    {
        camRotCenterTrans.position = _newPos_;
    }
}
