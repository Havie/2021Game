using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NullBool { NULL, FALSE, TRUE };

public class InputHolder
{
    // If select has been pressed
    public NullBool HasSelPress { set; get; }
    // Axis for how to move the cursor
    public Vector2 CursorMoveAxis { set; get; }
    // If the mouse has moved
    public NullBool HasMouseMoved { set; get; }
    // Axis for how to rotate the camera
    public Vector2 CameraRotAxis { set; get; }
    // Axis for how to navigate the menu
    public Vector2Int MenuAxis { set; get; }

    /// <summary>
    /// Constructs an InputHolder
    /// </summary>
    public InputHolder()
    {
        Reset();
    }

    /// <summary>
    /// Resets the values of the input.
    /// </summary>
    public void Reset()
    {
        HasSelPress = NullBool.NULL;
        CursorMoveAxis = new Vector2(int.MinValue, int.MinValue);
        HasMouseMoved = NullBool.NULL;
        CameraRotAxis = new Vector2(int.MinValue, int.MinValue);
        MenuAxis = new Vector2Int(int.MinValue, int.MinValue);
    }

}
