using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class CursorController : MonoBehaviour
{
    // Instance of the game object
    public static CursorController Instance { get; private set; }

    // Reference to the sprite renderer
    private SpriteRenderer _sprRenderer;

    // If the cursor is enabled
    private bool _cursorOn;

    // How fast the cursor should move
    private const float MOVE_SPEED = 0.2f;

    // Called 0th
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        _sprRenderer = this.GetComponent<SpriteRenderer>();
    }
    // Called when this gameObject is destroyed.
    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
    // Called 1st
    private void Start()
    {
        _cursorOn = true;
    }

    // Update is called once per frame
    private void Update()
    {
        if (_cursorOn)
        {
            Vector2 cursorMove = InputController.GetCursorMoveAxis();
            if (cursorMove != Vector2.zero)
            {
                Vector3 incrPos = new Vector3(cursorMove.x * MOVE_SPEED, 0, cursorMove.y * MOVE_SPEED);
                this.transform.position += incrPos;
            }
        }
    }

    /// <summary>
    /// Turns the cursors on or off.
    /// </summary>
    /// <param name="_onOff_">Whether the cursor should be on or off</param>
    public void ToggleCursosr(bool _onOff_)
    {
        _sprRenderer.enabled = _onOff_;
        _cursorOn = _onOff_;
    }

    /// <summary>
    /// Returns the position of the cursor in the world.
    /// </summary>
    /// <returns>Vector3</returns>
    public Vector3 GetCursorPosition() { return this.transform.position; }
}
