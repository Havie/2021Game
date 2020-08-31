using System.Collections;
using UnityEngine;
using UnityEngine.AI;

//Cant store an action?
//private System.Action<int> _onComplete;

[RequireComponent(typeof(NavMeshAgent))]
public class MovementController : MonoBehaviour
{
    // Reference to the nav mesh agent on our character
    private NavMeshAgent _agent;

    private bool _isMoving;
    private bool _agentThinking;

    // Called 0th
    private void Awake()
    {
        _agent = this.GetComponent<NavMeshAgent>();
    }



                                           
    /// <summary>
    /// Tell the Agent to start movement towards a position.
    /// Start a Coroutine that takes in a callback function to fire when 
    /// the agent has reached the destination
    /// This will allow us perform different actions based on different skills
    /// </summary>
    /// <param name="_pos_"></param>
    /// <param name="OnComplete"></param>        
    public void DoMovement(Vector3 _pos_, System.Action OnComplete) //Action<Type> OnComplete
    {
        if (_pos_ != Vector3.negativeInfinity)
        {
            _agent.SetDestination(_pos_); //Moves the agent
            _agentThinking = true; // used by the coroutine to establish a delay
            StartCoroutine(MoveRoutine(OnComplete));
        }
    }
    //Helper Coroutine to detect when the agent has reached its destination and fire a callback
    private IEnumerator MoveRoutine(System.Action OnComplete)
    {
        //This sucker takes awhile to get going, so we have to perform this check to figure out when it is ready
        while (_agentThinking )
        {
            if (_agent.hasPath)
            {
                _isMoving = true;
                _agentThinking = false;
            }
            yield return new WaitForEndOfFrame();
        }
        
        //Once the agent no longer has a path, we know its reached its destination
        while(_isMoving)
        {
            if (!_agent.hasPath)
            {
                _isMoving = false;
                //Fire our callback function if any
                OnComplete?.Invoke();
            }
            yield return new WaitForEndOfFrame();
        }
        yield return null;
    }


    /// OBSOLETE
    public void DoMovement(Vector3 _pos_)
    {
        if (_pos_ != Vector3.negativeInfinity)
        {
            // Debug.Log("DoMovement");
            // Move our agent
            _agent.SetDestination(_pos_);
            _agentThinking = true;
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