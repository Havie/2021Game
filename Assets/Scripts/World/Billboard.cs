using UnityEngine;

public class Billboard : MonoBehaviour
{
    // Called when the behaviour is enabled.
    // Subscribe to events.
    protected void OnEnable()
    {
        // Face the camera when it is rotated
        cEventSystem.OnCameraRotate += FaceCamera;
    }
    // Called when the behaviour is disabled.
    // Unsubscribe from events.
    protected void OnDisable()
    {
        cEventSystem.OnCameraRotate -= FaceCamera;
    }
    // Called when the gameobject is destroyed.
    // Unsubscribe from ALL events.
    protected void OnDestroy()
    {
        cEventSystem.OnCameraRotate -= FaceCamera;
    }

    // Called 0th
    protected void Start()
    {
        // Start off facing the camera
        FaceCamera();
    }

    /// <summary>
    /// Points this gameobject to be facing the camera and calls the update sprite function.
    /// </summary>
    private void FaceCamera()
    {
        Vector3 camEuler = Camera.main.transform.rotation.eulerAngles;
        this.transform.rotation = Quaternion.Euler(camEuler.x, camEuler.y, camEuler.z);
        UpdateSprite();
    }
    
    /// <summary>
    /// Meant to be overriden in child classes.
    /// </summary>
    protected virtual void UpdateSprite() { }
}
