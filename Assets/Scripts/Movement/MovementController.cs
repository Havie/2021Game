using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MovementController : MonoBehaviour
{
    // Reference to the camera we are using
    private Camera _cam;
    // Reference to the nav mesh agent on our character
    private NavMeshAgent _agent;

    private bool _isMoving;
    private bool _agentThinking;

    // Called 0th
    private void Awake()
    {
        // Set references
        _cam = Camera.main;

        try
        {
            _agent = this.GetComponent<NavMeshAgent>();
        }
        catch
        {
            Debug.LogError("Could not set references in MovmentController");
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (_agentThinking && _agent.hasPath)
        {
            _isMoving = true;
            _agentThinking = false;
        }
        else if(_isMoving && !_agent.hasPath)
        {
           // Debug.Log("Agent stopped");
            _isMoving = false;
            //TODO going to need more specific if as in if its this playables current turn etc
            SelectionManager.Instance.EnableMove(false);
            SelectionManager.Instance.ShowBattleMenu();
           
        }
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