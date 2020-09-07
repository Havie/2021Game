﻿using System.Collections;
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

    // Default Vectors
    public static Vector2 DEFAULT_VECTOR2 = new Vector2(float.MinValue, int.MinValue);
    public static Vector2Int DEFAULT_VECTOR2INT = new Vector2Int(int.MinValue, int.MinValue);

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
        CursorMoveAxis = DEFAULT_VECTOR2;
        HasMouseMoved = NullBool.NULL;
        CameraRotAxis = DEFAULT_VECTOR2;
        MenuAxis = DEFAULT_VECTOR2INT;
    }

}
