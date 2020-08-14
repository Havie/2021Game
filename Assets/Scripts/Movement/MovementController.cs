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
        // If the player inputted to select
        if (InputController.GetSelectPressDown())
        {
            // Get the mouse's raycast world position
            Vector3 hitPos = InputController.GetCursorRayWorldPosition();
            if (hitPos != Vector3.negativeInfinity)
            {
              //  DoMovement(hitPos);
            }
        }
    }
    public void DoMovement(Vector3 Pos)
    {
        if (Pos != Vector3.negativeInfinity)
        {
            // Move our agent
            _agent.SetDestination(Pos);
        }
    }
}