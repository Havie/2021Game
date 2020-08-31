using UnityEngine;

public class InputManager : MonoBehaviour
{
    // Called 0th
    private void Awake()
    {
        InputController.Initialize();
    }

    // Called every frame after update
    private void LateUpdate()
    {
        InputController.RefreshHolder();
    }
}
