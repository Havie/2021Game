using UnityEngine;

public class FollowMouseInWorld : MonoBehaviour
{
    // Update is called once per frame
    private void Update()
    {
        // Set the position of the object on mouse move
        if (InputController.GetHasCursorMoved())
        {
            Vector3 newPos = InputController.GetCursorRayWorldPosition();
            if (newPos != Vector3.negativeInfinity)
                this.transform.position = newPos;
        }
    }
}
