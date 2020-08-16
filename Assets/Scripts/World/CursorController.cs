using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    // Instance of the game object
    public static CursorController Instance { get; private set; }

    // How fast the cursor should move
    private const float MOVE_SPEED = 0.2f;

    // Called 0th
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    // Called when this gameObject is destroyed.
    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    // Update is called once per frame
    private void Update()
    {
        Vector2 cursorMove = InputController.GetCursorMoveAxis();
        if (cursorMove != Vector2.zero)
        {
            Vector3 incrPos = new Vector3(cursorMove.x * MOVE_SPEED, 0, cursorMove.y * MOVE_SPEED);
            this.transform.position += incrPos;
        }
    }

    /// <summary>
    /// Returns the position of the cursor in the world.
    /// </summary>
    /// <returns>Vector3</returns>
    public Vector3 GetCursorPosition() { return this.transform.position; }
}
