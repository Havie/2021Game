﻿
public class cEventSystem
{
    #region GameEvents
    // When the battle round ends.
    public delegate void BattleRoundEnd();
    public static event BattleRoundEnd OnBattleRoundEnd;
    public static void CallOnBattleRoundEnd() { OnBattleRoundEnd?.Invoke(); }

    // When a character's turn ends. Tells the next character to go.
    public delegate void CharacterTurnEnd();
    public static event CharacterTurnEnd OnCharacterTurnEnd;
    public static void CallOnCharacterTurnEnd() { OnCharacterTurnEnd?.Invoke(); }

    // When a character dies. Takes in the character that died as a parameter.
    public delegate void CharacterDeath(Officer officer);
    public static event CharacterDeath OnCharacterDeath;
    public static void CallOnCharacterDeath(Officer _corpse_) { OnCharacterDeath?.Invoke(_corpse_); }

    //When a character finishes attacking
    public delegate void AttackFinished();
    public static event AttackFinished OnAttackFinished;
    public static void CallOnAttackFinished() { OnAttackFinished?.Invoke(); }

    #endregion

    #region InputEvents

    // When InputController.SelectPressDown is true
    public delegate void SelectPressDownDelegate();
    public static event SelectPressDownDelegate OnSelectPressDown;
    public static bool IsOnSelectPressDownUsed() { return OnSelectPressDown != null; }
    public static void CallOnSelectPressDown() { OnSelectPressDown?.Invoke(); }

    // When InputController.CameraRotAxis is true
    public delegate void HasCameraRotInputDelegate();
    public static event HasCameraRotInputDelegate OnHasCameraRotInput;
    public static bool IsOnHasCameraRotInputUsed() { return OnHasCameraRotInput != null; }
    public static void CallOnHasCameraRotInput() { OnHasCameraRotInput?.Invoke(); }

    // When InputController.HasMenuInput is true
    public delegate void HasMenuInputDelegate();
    public static event HasMenuInputDelegate OnHasMenuInput;
    public static bool IsOnHasMenuInputUsed() { return OnHasMenuInput != null; }
    public static void CallOnHasMenuInput() { OnHasMenuInput?.Invoke(); }

    #endregion



    #region CameraEvents

    // When the camera moves
    public delegate void CameraMove();
    public static event CameraMove OnCameraMove;
    public static void CallOnCameraMove() { OnCameraMove?.Invoke(); }

    // When the camera rotates
    public delegate void CameraRotate();
    public static event CameraRotate OnCameraRotate;
    public static void CallOnCameraRotate() { OnCameraRotate?.Invoke(); }

    // When the camera finishes revolving
    public delegate void CameraFinishRevolution();
    public static event CameraFinishRevolution OnCameraFinishRevolution;
    public static void CallOnCameraFinishRevolution() { OnCameraFinishRevolution?.Invoke(); }

    #endregion

}
