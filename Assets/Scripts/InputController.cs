using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController
{
    /// <summary>
    /// Returns true the frame that the select input was pressed.
    /// </summary>
    /// <returns>bool</returns>
    public static bool GetSelectPressDown()
    {
        if (Input.GetMouseButtonDown(0))
            return true;

        return false;
    }

    /// <summary>
    /// Returns the cursor's position in pixel coordinates.
    /// </summary>
    /// <returns>Vector3</returns>
    public static Vector3 GetCursorPosition()
    {
        return Input.mousePosition;
    }

    /// <summary>
    /// Casts a ray from the cursor to the world using Physics.Raycast.
    /// If it hits something, it returns the position of the hit, otherwise returns Vector3.negativeInfinitiy;
    /// </summary>
    /// <returns>Vector3</returns>
    public static Vector3 GetCursorRayWorldPosition()
    {
        RaycastHit hit;
        return GetCursorRayWorldPosition(out hit);
    }

    /// <summary>
    /// Casts a ray from the cursor to the world using Physics.Raycast.
    /// If Physics.Raycast returns true, it returns the position of the hit, otherwise returns Vector3.negativeInfinitiy.
    /// Accepts a RaycastHit as output.
    /// </summary>
    /// <param name="_hit_">RaycastHit that will be used for the Physics.Raycast call.</param>
    /// <returns>Vector3</returns>
    public static Vector3 GetCursorRayWorldPosition(out RaycastHit _hit_)
    {
        // Create a ray
        Ray ray = Camera.main.ScreenPointToRay(GetCursorPosition());

        if (Physics.Raycast(ray, out _hit_))
        {
            return _hit_.point;
        }

        return Vector3.negativeInfinity;
    }
}
