using UnityEngine;

public class PS4ControllerInput
{
    // Front and back input buffers.
    private static PS4InputHolder _frontHolder = new PS4InputHolder();
    private static PS4InputHolder _lastHolder = new PS4InputHolder();


    /// <summary>
    /// Exchanges the holders and then reads current input.
    /// </summary>
    public static void ReadInput()
    {
        ExchangeHolders();

        ReadDPadInput();
        ReadLeftJoystickInput();
        ReadShapeInput();
    }

    /// <summary>
    /// Helper function for ReadInput. Reads input for the dpad.
    /// </summary>
    private static void ReadDPadInput()
    {
        float x = Input.GetAxis("PS4Controller_DPad_X");
        float y = Input.GetAxis("PS4Controller_DPad_Y");
        _frontHolder.DPad = new Vector2(x, y);
    }

    /// <summary>
    /// Helper function for ReadInput. Reads input for the left joystick.
    /// </summary>
    private static void ReadLeftJoystickInput()
    {
        float x = Input.GetAxisRaw("PS4Controller_LeftJoystick_X");
        float y = Input.GetAxisRaw("PS4Controller_LeftJoystick_Y");
        _frontHolder.LeftJoystick = new Vector2(x, y);
        Debug.Log("( " + x + ", " + y + ")");
    }

    /// <summary>
    /// Helper function for ReadInput. Reads input for the shape buttons.
    /// </summary>
    private static void ReadShapeInput()
    {
        _frontHolder.SquareButt = Input.GetAxis("PS4Controller_SquareButt") > 0;
        _frontHolder.XButt = Input.GetAxis("PS4Controller_XButt") > 0;
        _frontHolder.CircleButt = Input.GetAxis("PS4Controller_CircleButt") > 0;
        _frontHolder.TriangleButt = Input.GetAxis("PS4Controller_TriangleButt") > 0;
    }

    /// <summary>
    /// Gives the last holder the front holder's current info and then resets the front holder.
    /// </summary>
    private static void ExchangeHolders()
    {
        _lastHolder.CopyHolder(_frontHolder);
        _frontHolder.RefreshHolder();
    }



    /// <summary>
    /// Gets DPad input where
    /// x is left (-1) and right (1) and 
    /// y is down (-1) and up (1)
    /// </summary>
    /// <returns></returns>
    public static Vector2 GetDPad() { return _frontHolder.DPad; }
    /// <summary>
    /// Gets Left Joystick input where
    /// x is left (-1) and right (1) and 
    /// y is down (-1) and up (1)
    /// </summary>
    /// <returns></returns>
    public static Vector2 GetLeftJoystick() { return _frontHolder.LeftJoystick; }


    #region ShapeButtons

    /// <summary>
    /// Returns true while the Square button is being held down.
    /// </summary>
    /// <returns>bool</returns>
    public static bool GetSquareButton() { return _frontHolder.SquareButt; }
    /// <summary>
    /// Returns true the frame the Square button is pressed.
    /// </summary>
    /// <returns>bool</returns>
    public static bool GetSquareButtonDown()
    {
        // When the current frame is down, but the last one wasn't.
        return _frontHolder.SquareButt && !_lastHolder.SquareButt;
    }
    /// <summary>
    /// Returns true the frame the Square button is released.
    /// </summary>
    /// <returns>bool</returns>
    public static bool GetSquareButtonUp()
    {
        // When the current frame is not down, but the last one was.
        return !_frontHolder.SquareButt && _lastHolder.SquareButt;
    }

    /// <summary>
    /// Returns true while the X button is being held down.
    /// </summary>
    /// <returns>bool</returns>
    public static bool GetXButton() { return _frontHolder.XButt; }
    /// <summary>
    /// Returns true the frame the X button is pressed.
    /// </summary>
    /// <returns>bool</returns>
    public static bool GetXButtonDown()
    {
        // When the current frame is down, but the last one wasn't.
        return _frontHolder.XButt && !_lastHolder.XButt;
    }
    /// <summary>
    /// Returns true the frame the X button is released.
    /// </summary>
    /// <returns>bool</returns>
    public static bool GetXButtonUp()
    {
        // When the current frame is not down, but the last one was.
        return !_frontHolder.XButt && _lastHolder.XButt;
    }

    /// <summary>
    /// Returns true while the Circle button is being held down.
    /// </summary>
    /// <returns>bool</returns>
    public static bool GetCircleButton() { return _frontHolder.CircleButt; }
    /// <summary>
    /// Returns true the frame the Circle button is pressed.
    /// </summary>
    /// <returns>bool</returns>
    public static bool GetCircleButtonDown()
    {
        // When the current frame is down, but the last one wasn't.
        return _frontHolder.CircleButt && !_lastHolder.CircleButt;
    }
    /// <summary>
    /// Returns true the frame the Circle button is released.
    /// </summary>
    /// <returns>bool</returns>
    public static bool GetCircleButtonUp()
    {
        // When the current frame is not down, but the last one was.
        return !_frontHolder.CircleButt && _lastHolder.CircleButt;
    }

    /// <summary>
    /// Returns true while the Triangle button is being held down.
    /// </summary>
    /// <returns>bool</returns>
    public static bool GetTriangleButton() { return _frontHolder.TriangleButt; }
    /// <summary>
    /// Returns true the frame the Triangle button is pressed.
    /// </summary>
    /// <returns>bool</returns>
    public static bool GetTriangleButtonDown()
    {
        // When the current frame is down, but the last one wasn't.
        return _frontHolder.TriangleButt && !_lastHolder.TriangleButt;
    }
    /// <summary>
    /// Returns true the frame the Triangle button is released.
    /// </summary>
    /// <returns>bool</returns>
    public static bool GetTriangleButtonUp()
    {
        // When the current frame is not down, but the last one was.
        return !_frontHolder.TriangleButt && _lastHolder.TriangleButt;
    }

    #endregion ShapeButtons
}
