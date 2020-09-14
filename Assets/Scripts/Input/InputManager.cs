using UnityEngine;

public class InputManager : MonoBehaviour
{
    // Called 0th
    private void Awake()
    {
        InputController.Initialize();
    }

    // Called every frame
    private void Update()
    {
        // SelectPressDown
        if (cEventSystem.IsOnSelectPressDownUsed())
            if (InputController.GetSelectPressDown())
                cEventSystem.CallOnSelectPressDown();

        // CameraRotAxis
        if (cEventSystem.IsOnHasCameraRotInputUsed())
            if (InputController.HasCameraRotateInput())
                cEventSystem.CallOnHasCameraRotInput();

        // HasMenuInput
        if (cEventSystem.IsOnHasMenuInputUsed())
            if (InputController.HasMenuInput())
                cEventSystem.CallOnHasMenuInput();
    }

    // Called every frame after update
    private void LateUpdate()
    {
        InputController.RefreshHolder();
        PS4ControllerInput.ReadInput();
    }
}
