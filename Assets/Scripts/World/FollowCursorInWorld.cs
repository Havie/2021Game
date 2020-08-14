using UnityEngine;

public class FollowCursorInWorld : MonoBehaviour
{
    // Update is called once per frame
    private void Update()
    {
        // Set the position of the object on cursor move
        if (InputController.GetHasCursorMoved())
        {
            LayerMask layerMask = LayerMask.GetMask("Ground");
            Vector3 newPos = InputController.GetCursorRayWorldPosition(layerMask);
            if (newPos != Vector3.negativeInfinity)
                this.transform.position = newPos;
        }
    }
}
