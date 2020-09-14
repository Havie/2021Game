using UnityEngine;

public class PS4InputHolder
{
    // Constant Defaults
    private readonly static Vector2 DEFAULT_VECTOR2 = Vector2.zero;

    // Vectors for d-pad and joysticks
    public Vector2 DPad { set; get; }
    public Vector2 LeftJoystick { set; get; }

    // Shapes
    public bool XButt { set; get; }
    public bool TriangleButt { set; get; }
    public bool SquareButt { set; get; }
    public bool CircleButt { set; get; }

    /// <summary>
    /// Constructs a PS4InputHolder.
    /// </summary>
    public PS4InputHolder()
    {
        RefreshHolder();
    }

    /// <summary>
    /// Resets the variables to their defaults.
    /// </summary>
    public void RefreshHolder()
    {
        DPad = DEFAULT_VECTOR2;
        LeftJoystick = DEFAULT_VECTOR2;

        XButt = false;
        TriangleButt = false;
        SquareButt = false;
        CircleButt = false;
    }

    /// <summary>
    /// Copys the values stored in the given holder to this holder.
    /// </summary>
    /// <param name="_holdToCopy_">Holder whose values will be copied to this holder.</param>
    public void CopyHolder(PS4InputHolder _holdToCopy_)
    {
        DPad = _holdToCopy_.DPad;
        LeftJoystick = _holdToCopy_.LeftJoystick;

        XButt = _holdToCopy_.XButt;
        TriangleButt = _holdToCopy_.TriangleButt;
        SquareButt = _holdToCopy_.SquareButt;
        CircleButt = _holdToCopy_.CircleButt;
    }
}
