using System.Collections.Generic;
using UnityEngine;

public class DottedLine
{
    // Prefab of the dot to spawn
    private GameObject _dotPref;
    // How large the dots should be
    private float _size;
    // The spacing of the dots
    private float _delta;

    // Positions of the dots
    private List<Vector3> _positions = new List<Vector3>();
    // List of the dots gameobjects
    private List<GameObject> _dotList = new List<GameObject>();

    /// <summary>
    /// Constructs a new DottedLine.
    /// </summary>
    /// <param name="_dot_">Prefab of the dot to spawn.</param>
    /// <param name="_s_">How large the dots should be.</param>
    /// <param name="_d_">The spacing of the dots.</param>
    /// <param name="_start_">Starting position of the line.</param>
    /// <param name="_end_">End position of the line.</param>
    public DottedLine(GameObject _dot_, float _s_, float _d_, Vector3 _start_, Vector3 _end_)
    {
        _dotPref = _dot_;
        _size = _s_;
        _delta = _d_;

        // Add points between start and end
        Vector3 point = _start_;
        Vector3 direction = (_end_ - _start_).normalized;

        if (direction != Vector3.zero)
        {
            int maxInf = 100;
            int infCheck = 0;
            while ((_end_ - _start_).magnitude > (point - _start_).magnitude)
            {
                _positions.Add(point);
                point += (direction * _delta);

                if (++infCheck > maxInf)
                {
                    Debug.Log(direction);
                    Debug.LogError("Infinite Dots");
                    return;
                }
            }
        }

        // Spawn the dots
        RenderDots();
    }

    /// <summary>
    /// Destroys the dots and clears the dotList and positions.
    /// </summary>
    public void DestroyDots()
    {
        for (int i = 0; i < _dotList.Count; ++i) {
            GameObject.Destroy(_dotList[i]);
        }
        _dotList.Clear();
        _positions.Clear();
    }

    /// <summary>
    /// Spawns a dot at each position in positions.
    /// </summary>
    private void RenderDots()
    {
        foreach (Vector3 pos in _positions)
        {
            GameObject newDot = GameObject.Instantiate(_dotPref, pos, _dotPref.transform.rotation);
            _dotList.Add(newDot);
        }
    }
}
