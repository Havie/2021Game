using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CreateMovementLine : MonoBehaviour
{
    // The transform of the starting position (the currently selected character)
    [SerializeField]
    private Transform _startTrans = null;
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

    // Update is called once per frame
    private void Update()
    {
        CreatePathPreview();     
    }

    /// <summary>
    /// Creates a new path preview.
    /// </summary>
    private void CreatePathPreview()
    {
        // Destroy all the current dotted lines
        foreach (DottedLine dLine in _dotLines)
        {
            dLine.DestroyDots();
        }
        _dotLines.Clear();

        // Get the start and end positions
        Vector3 startPos = _startTrans.transform.position;
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
            _dotLines.Add(new DottedLine(_dotPref, _size, _delta, lineStart, lineEnd));

            float dist = sub.magnitude;
            float amDots = dist / _delta;
            leftover = (1 - (amDots - Mathf.FloorToInt(amDots))) * _delta;
            if (leftover > 1 || leftover < 0)
                leftover = 0;

            curPos = corner;
        }
    }
}
