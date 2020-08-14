using UnityEngine;
using UnityEngine.AI;

public class FakePathing : MonoBehaviour
{
    // Reference to the nav mesh agent on our character
    private NavMeshAgent _agent;

    // Called 0th
    private void Awake()
    {
        try
        {
            _agent = this.GetComponent<NavMeshAgent>();
        }
        catch
        {
            Debug.LogError("Could not set references in MovmentController");
        }
    }

    /// <summary>
    /// Creates a path from the start to the end and returns it.
    /// </summary>
    /// <param name="_start_">Start position of path.</param>
    /// <param name="_end_">End position of path.</param>
    /// <returns>NavMeshPath</returns>
    public NavMeshPath GetPathFromTo(Vector3 _start_, Vector3 _end_)
    {
        this.transform.position = _start_;
        // Move our agent
        _agent.SetDestination(_end_);
        return _agent.path;
    }
}
