using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class MovementController : MonoBehaviour
{
    // Reference to the nav mesh agent on our character
    private NavMeshAgent _agent;

    // Called 0th
    private void Awake()
    {
        _agent = this.GetComponent<NavMeshAgent>();
    }

    /// <summary>
    /// Starts moving this character towards the given point.
    /// </summary>
    /// <param name="_pos_">End position of the path.</param>
    public void DoMovement(Vector3 _pos_)
    {
        if (_pos_ != Vector3.negativeInfinity)
        {
            // Debug.Log("DoMovement");
            // Move our agent
            _agent.SetDestination(_pos_);
        }
    }

    /// <summary>
    /// Creates a potential path for the character to the given end point.
    /// </summary>
    /// <param name="_end_">End position of path</param>
    public NavMeshPath GetPotentialPath(Vector3 _end_)
    {
        NavMeshPath path = new NavMeshPath();
        _agent.CalculatePath(_end_, path);
        return path;
    }
}