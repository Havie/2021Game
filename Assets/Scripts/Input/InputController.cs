﻿using UnityEngine;

public class InputController
{
    // Holds the input that has already been asked for so that we do not have to query
    // for the input multiple times in the same frame.
    private static InputHolder _inpHolder;

    /// <summary>
    /// Creates the input holder.
    /// </summary>
    public static void Initialize() { _inpHolder = new InputHolder(); }
    /// <summary>
    /// Resets the input holder. Should be called every frame.
    /// </summary>
    public static void RefreshHolder() { _inpHolder.Reset(); }

    /// <summary>
    /// Returns true the frame that the select input was pressed.
    /// </summary>
    /// <returns>bool</returns>
    public static bool GetSelectPressDown()
    {
        if (_inpHolder.HasSelPress == NullBool.NULL)
        {
            if (Input.GetMouseButtonDown(0))
                _inpHolder.HasSelPress = NullBool.TRUE;
            else
                _inpHolder.HasSelPress = NullBool.FALSE;
        }

        if (_inpHolder.HasSelPress == NullBool.TRUE)
            return true;
        else
            return false;
    }

    /// <summary>
    /// Returns a Vector2 that represents how the cursor should move.
    /// </summary>
    /// <returns>Vector2</returns>
    public static Vector2 GetCursorMoveAxis()
    {
        if (_inpHolder.CursorMoveAxis ==InputHolder.DEFAULT_VECTOR2)
        {
            Vector2 arrowVect = Vector2.zero;

            if (Input.GetKey(KeyCode.LeftArrow))
                arrowVect.x = -1;
            else if (Input.GetKey(KeyCode.RightArrow))
                arrowVect.x = 1;
            if (Input.GetKey(KeyCode.DownArrow))
                arrowVect.y = -1;
            else if (Input.GetKey(KeyCode.UpArrow))
                arrowVect.y = 1;

            _inpHolder.CursorMoveAxis = arrowVect;
        }

        return _inpHolder.CursorMoveAxis;
    }

    /// <summary>
    /// DEPRECATED
    /// Returns the cursor's position on the screen.
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

    /// <summary>
    /// Casts a ray from the cursor to the world using Physics.Raycast.
    /// If Physics.Raycast returns true, it returns the position of the hit, otherwise returns Vector3.negativeInfinitiy.
    /// Only casts the ray on a specific layermask
    /// </summary>
    /// <param name="_layerMask_">LayerMask to do the Physics.Raycast on.</param>
    /// <returns>Vector3</returns>
    public static Vector3 GetCursorRayWorldPosition(int _layerMask_)
    {
        RaycastHit hit;
        return GetCursorRayWorldPosition(out hit, _layerMask_);
    }

    /// <summary>
    /// Casts a ray from the cursor to the world using Physics.Raycast.
    /// If Physics.Raycast returns true, it returns the position of the hit, otherwise returns Vector3.negativeInfinitiy.
    /// Accepts a RaycastHit as output.
    /// Only casts the ray on a specific layermask
    /// </summary>
    /// <param name="_hit_">RaycastHit that will be used for the Physics.Raycast call.</param>
    /// <param name="_layerMask_">LayerMask to do the Physics.Raycast on.</param>
    /// <returns>Vector3</returns>
    public static Vector3 GetCursorRayWorldPosition(out RaycastHit _hit_, int _layerMask_)
    {
        // Create a ray
        Ray ray = Camera.main.ScreenPointToRay(GetCursorPosition());

        if (Physics.Raycast(ray, out _hit_, float.PositiveInfinity, _layerMask_))
        {
            return _hit_.point;
        }

        return Vector3.negativeInfinity;
    }

    /// <summary>
    /// Returns true when the cursor changes position.
    /// </summary>
    /// <returns>bool</returns>
    public static bool GetHasMouseMoved()
    {
        if (_inpHolder.HasMouseMoved == NullBool.NULL)
        {
            if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
                _inpHolder.HasMouseMoved = NullBool.TRUE;
            else
                _inpHolder.HasMouseMoved = NullBool.FALSE;
        }

        if (_inpHolder.HasMouseMoved == NullBool.TRUE)
            return true;
        else
            return false;
    }

    /// <summary>
    /// Returns the amount the camera rotate buttons have been pushed.
    /// </summary>
    /// <returns>Vector2</returns>
    public static Vector2 GetCameraRotateAxis()
    {
        if (_inpHolder.CameraRotAxis == InputHolder.DEFAULT_VECTOR2)
        {
            Vector2 rtnVect = Vector2.zero;

            if (Input.GetKey(KeyCode.A))
            {
                rtnVect.y = 1;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                rtnVect.y = -1;
            }

            if (Input.GetKey(KeyCode.S))
            {
                rtnVect.x = -1;
            }
            else if (Input.GetKey(KeyCode.W))
            {
                rtnVect.x = 1;
            }

            _inpHolder.CameraRotAxis = rtnVect;
        }

        return _inpHolder.CameraRotAxis;
    }

    /// <summary>
    /// Returns a Vector2Int that holds input information about how to navigate, up, down, left, and right in a menu.
    /// X represents left and right. Y represents up and down.
    /// </summary>
    /// <returns>Vector2Int</returns>
    public static Vector2Int GetMenuAxis()
    {
        if (_inpHolder.MenuAxis == InputHolder.DEFAULT_VECTOR2INT)
        {
            Vector2Int rtnVect = new Vector2Int();

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                rtnVect.x = 1;
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                rtnVect.x = -1;
            }

            if (Input.GetKey(KeyCode.DownArrow))
            {
                rtnVect.y = -1;
            }
            else if (Input.GetKey(KeyCode.UpArrow))
            {
                rtnVect.y = 1;
            }

            _inpHolder.MenuAxis = rtnVect;
        }

        return _inpHolder.MenuAxis;
    }
}
