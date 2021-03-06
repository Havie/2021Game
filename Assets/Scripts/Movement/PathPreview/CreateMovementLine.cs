﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CreateMovementLine : MonoBehaviour
{

    private static CreateMovementLine _instance;
    public static CreateMovementLine Instance
    {
        get
        {
            if (_instance == null)
                _instance = GameObject.FindObjectOfType<CreateMovementLine>();
            return _instance;
        }
    }

    // The transform of the end positions (the cursor)
    [SerializeField]
    private Transform _endTrans = null;

    // Prefab of the dot to spawn
    [SerializeField]
    private GameObject _dotPref = null;
    // How large the dots should be
    [SerializeField]
    private float _size = 1f;
    // The spacing of the dots
    [SerializeField]
    private float _delta = 0.5f;

    // The current character movement controller
    [SerializeField]
    private MovementController _curCharaMove = null;

    // List of dotted lines
    private List<DottedLine> _dotLines = new List<DottedLine>();
    // If the preview is on
    private bool _previewEnabled;

    private bool _isAttackPrediction;

    // Called 0th
    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        if (_instance != this)
            Destroy(this);
    }


    // Called 1st
    private void Start()
    {
        _previewEnabled = false;
    }

    // Update is called once per frame
    private void Update() //Maybe change to late update?
    {
        if (_previewEnabled && !_isAttackPrediction)
        {
            cEventSystem.CallOnHasMovementPrediction(CreatePathPreview());
        }
        else if(_previewEnabled && _isAttackPrediction)
        {
            CreatePathPreview();
            cEventSystem.CallOnHasMovementPrediction(
                SelectionManager.Instance.GetCurrentSkill().GetAPCost()
                );

        }
    }

    /// <summary>
    /// Enables the line preview.
    /// </summary>
    /// <param name="_charToMove_">MovementController of the character that will be moving.</param>
    public void EnablePathPreview(MovementController _charToMove_, bool isAttack)
    {
        _curCharaMove = _charToMove_;
        _previewEnabled = true;
        cEventSystem.CallOnShowAPCostPrediction();
        _isAttackPrediction = isAttack;
    }

    /// <summary>
    /// Disables the line preview.
    /// </summary>
    public void DisablePathPreview()
    {
        ClearDottedLines();
        _curCharaMove = null;
        _previewEnabled = false;
        cEventSystem.CallOnHideAPCostPrediction();
    }

    /// <summary>
    /// Creates a new path preview.
    /// </summary>
    private int CreatePathPreview()
    {
        // Clear the last lines
        ClearDottedLines();

        // Get the start and end positions
        Vector3 startPos = _curCharaMove.transform.position;
        Vector3 endPos = _endTrans.transform.position;

        
        // Fake path with these positions to create a path
        NavMeshPath path = _curCharaMove.GetPotentialPath(endPos);

        
        // Iterate over the corners and create a dotted line between subsequent corners
        Vector3 curPos = startPos;
        float leftover = 0;
        foreach (Vector3 corner in path.corners)
        {
            Vector3 sub = (corner - curPos);
            Vector3 dir = sub.normalized;

            // Create the dotted lines
            Vector3 lineStart = curPos + dir * leftover;
            lineStart.y = 0;
            Vector3 lineEnd = corner;
            lineEnd.y = 0;
            DottedLine dl = new DottedLine(_dotPref, _size, _delta, lineStart, lineEnd);
            _dotLines.Add(dl);

            float dist = sub.magnitude;
            float amDots = dist / _delta;
            leftover = (1 - (amDots - Mathf.FloorToInt(amDots))) * _delta;
            if (leftover > 1 || leftover < 0)
                leftover = 0;

            curPos = corner;
        }

        //Random calculation of movement costs 
        int dotCount = 0;
        foreach (DottedLine d in _dotLines)
            dotCount += d.GetNumDots();
        return (dotCount / 3 ) +1;
    }

    /// <summary>
    /// Gets rid of all the dots and clears the list of lines.
    /// </summary>
    private void ClearDottedLines()
    {
        // Destroy all the current dotted lines
        foreach (DottedLine dLine in _dotLines)
        {
            dLine.DestroyDots();
        }
        _dotLines.Clear();
    }
}
