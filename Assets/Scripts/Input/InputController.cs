using UnityEngine;

public class InputController
{
    // Holds the input that has already been asked for so that we do not have to query
    // for the input multiple times in the same frame.
    private static InputHolder _inpHolder;

    // Current menu time
    private static float _menuActiveTime = 0;
    // Time to allow for menu input again
    private static readonly float MENU_INPUT_DELAY = 0.2f;

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
            if (Input.GetMouseButtonDown(0) || PS4ControllerInput.GetXButtonDown())
                _inpHolder.HasSelPress = NullBool.TRUE;
            else
                _inpHolder.HasSelPress = NullBool.FALSE;
        }

        return _inpHolder.HasSelPress == NullBool.TRUE;
    }

    /// <summary>
    /// Returns a Vector2 that represents how the cursor should move.
    /// </summary>
    /// <returns>Vector2</returns>
    public static Vector2 GetCursorMoveAxis()
    {
        if (_inpHolder.CursorMoveAxis == InputHolder.DEFAULT_VECTOR2)
        {
            Vector2 arrowVect = Vector2.zero;

            Vector2 contVect = PS4ControllerInput.GetLeftJoystick();

            if (Input.GetKey(KeyCode.LeftArrow) || contVect.x < 0)
                arrowVect.x = -1;
            else if (Input.GetKey(KeyCode.RightArrow) || contVect.x > 0)
                arrowVect.x = 1;
            if (Input.GetKey(KeyCode.DownArrow) || contVect.y < 0)
                arrowVect.y = -1;
            else if (Input.GetKey(KeyCode.UpArrow) || contVect.y > 0)
                arrowVect.y = 1;

            _inpHolder.CursorMoveAxis = arrowVect;
        }

        return _inpHolder.CursorMoveAxis;
    }

    /// <summary>
    /// Returns the mouse's position on the screen.
    /// </summary>
    /// <returns>Vector3</returns>
    private static Vector3 GetMouseScreenPosition()
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
        Ray ray = Camera.main.ScreenPointToRay(GetMouseScreenPosition());

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
        Ray ray = Camera.main.ScreenPointToRay(GetMouseScreenPosition());

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

        return _inpHolder.HasMouseMoved == NullBool.TRUE;
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
    /// Returns true if there is camera rotation input.
    /// </summary>
    /// <returns>bool</returns>
    public static bool HasCameraRotateInput()
    {
        if (_inpHolder.HasCameraRotInput == NullBool.NULL)
        {
            if (GetCameraRotateAxis() != Vector2.zero)
                _inpHolder.HasCameraRotInput = NullBool.TRUE;
            else
                _inpHolder.HasCameraRotInput = NullBool.FALSE;
        }

        return _inpHolder.HasCameraRotInput == NullBool.TRUE;
    }

    /// <summary>
    /// Returns a Vector2Int that holds input information about how to navigate, up, down, left, and right in a menu.
    /// X represents left and right. Y represents up and down.
    /// </summary>
    /// <returns>Vector2Int</returns>
    public static Vector2Int GetMenuAxis()
    {
        // KeyCodes for up, down, left right
        const KeyCode UP_KEYCODE = KeyCode.UpArrow;
        const KeyCode DOWN_KEYCODE = KeyCode.DownArrow;
        const KeyCode LEFT_KEYCODE = KeyCode.LeftArrow;
        const KeyCode RIGHT_KEYCODE = KeyCode.RightArrow;

        if (_inpHolder.MenuAxis.x == int.MinValue ||
            _inpHolder.MenuAxis.y == int.MinValue)
        {
            Vector2Int rtnVect = new Vector2Int();
            Vector2Int readInp = new Vector2Int();

            // Read controller input
            Vector2 ps4DPad = PS4ControllerInput.GetDPad();

            // Read x input
            if (Input.GetKey(LEFT_KEYCODE) || ps4DPad.x > 0)
                readInp.x = 1;
            else if (Input.GetKey(RIGHT_KEYCODE) || ps4DPad.x < 0)
                readInp.x = -1;

            // Read y input
            if (Input.GetKey(DOWN_KEYCODE) || ps4DPad.y < 0)
                readInp.y = -1;
            else if (Input.GetKey(UP_KEYCODE) || ps4DPad.y > 0)
                readInp.y = 1;

            // If the input was nothing, then it is okay to reset the timer
            if (readInp.x == 0 && readInp.y == 0)
                _menuActiveTime = 0;

            // For holding input, wait until the delay is over
            if (_menuActiveTime <= Time.time)
            {
                // Since timer is up, set the return vector to the input that was read.
                rtnVect = readInp;

                // Only increase the timer if there was some input
                if (rtnVect.x != 0 || rtnVect.y != 0)
                    _menuActiveTime = Time.time + MENU_INPUT_DELAY;
            }

            _inpHolder.MenuAxis = rtnVect;
        }

        return _inpHolder.MenuAxis;
    }

    /// <summary>
    /// Returns true the frame the user hits menu select input.
    /// </summary>
    /// <returns></returns>
    public static bool GetMenuSelectDown()
    {
        if (_inpHolder.HasMenuSelPress == NullBool.NULL)
        {
            if (Input.GetKeyDown(KeyCode.Return) || PS4ControllerInput.GetXButtonDown())
                _inpHolder.HasMenuSelPress = NullBool.TRUE;
            else
                _inpHolder.HasMenuSelPress = NullBool.FALSE;
        }

        return _inpHolder.HasMenuSelPress == NullBool.TRUE;
    }

    /// <summary>
    /// Returns true if the user has inputted some menu commands.
    /// </summary>
    /// <returns>bool</returns>
    public static bool HasMenuInput()
    {
        if (GetMenuAxis().magnitude != 0 || GetMenuSelectDown())
        {
            return true;
        }

        return false;
    }
}
