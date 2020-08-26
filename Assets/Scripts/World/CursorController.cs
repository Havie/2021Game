﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class CursorController : MonoBehaviour
{
    // Instance of the game object
    private static CursorController _instance;
    public static CursorController Instance
    {
        get
        {
            if (_instance == null)
                _instance = GameObject.FindObjectOfType<CursorController>();
            return _instance;
        }

    }

    // Reference to the sprite renderer
    [SerializeField] SpriteRenderer _sprRenderer;

    // If the cursor is enabled
    private bool _cursorOn;
    // If the camera is currently following the cursor
    private bool _camFollow;

    // How fast the cursor should move
    private const float MOVE_SPEED = 0.2f;

    // Called 0th
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance!=this)
        {
            Debug.LogError("There should never be more than 1 CursorController. Found another on " + this.name);
            Destroy(this.gameObject);
        }

        _sprRenderer = this.GetComponent<SpriteRenderer>();
    }
    // Called when this gameObject is destroyed.
    private void OnDestroy()
    {
        if (_instance == this)
        {
            _instance = null;
        }
    }
    // Called 1st
    private void Start()
    {
        _cursorOn = false;
        _camFollow = false;
    }

    // Update is called once per frame
    private void Update()
    {
        if (_cursorOn)
        {
            // See if the user is using a mouse
            if (InputController.GetHasMouseMoved())
            {
                // If they are using a mouse, put the cursor at the mouse's location
                // Also, don't move the camera with the cursor.
                if (_camFollow)
                {
                    CameraController.Instance.StopFollowingCharacter();
                    _camFollow = false;
                }

                int groundMask = LayerMask.GetMask("Ground");
                Vector3 mouseWorldPos = InputController.GetCursorRayWorldPosition(groundMask);
                if (mouseWorldPos.x != float.NegativeInfinity && mouseWorldPos.z != float.NegativeInfinity)
                {
                    //mouseWorldPos.y = 0;
                    this.transform.position = mouseWorldPos;
                }
            }
            // If they aren't using a mouse, use the cursor move axis
            else
            {
                Vector2 cursorMove = InputController.GetCursorMoveAxis();
                if (cursorMove != Vector2.zero)
                {
                    // Also make the camera follow the cursor, if it isn't already
                    if (!_camFollow)
                    {
                        CameraController.Instance.RecenterOnCursor();
                        _camFollow = true;
                    }

                    Vector3 dir = Camera.main.transform.forward.normalized;
                    Vector3 perp = Camera.main.transform.right.normalized;
                    Vector3 incrPos = cursorMove.x * perp + cursorMove.y * dir;
                    incrPos.y = 0;
                    this.transform.position += incrPos.normalized * MOVE_SPEED;
                }
            }
        }
    }

    /// <summary>
    /// Turns the cursors on or off.
    /// </summary>
    /// <param name="_onOff_">Whether the cursor should be on or off</param>
    public void ToggleCursor(bool _onOff_)
    {
        if(_sprRenderer)
            _sprRenderer.enabled = _onOff_;
        _cursorOn = _onOff_;

    }
}
