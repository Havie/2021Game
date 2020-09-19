
//using UnityEngine;  // To print things w debug.log

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

    //When battles over
    public delegate void BattleEnd(bool playerWon);
    public static event BattleEnd OnBattleEnd;
    public static void CallOnBattleEnd(bool playerWon) { OnBattleEnd?.Invoke(playerWon); }

    // When a character dies. Takes in the character that died as a parameter.
    public delegate void CharacterDeath(UnityEngine.GameObject officer);
    public static event CharacterDeath OnCharacterDeath;
    public static void CallOnCharacterDeath(UnityEngine.GameObject corpse) { OnCharacterDeath?.Invoke(corpse); }

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
    
    // Shows the AP text
    public delegate void ShowAPCostPrediction();
    public static event ShowAPCostPrediction OnShowAPCostPrediction;
    public static void CallOnShowAPCostPrediction() { OnShowAPCostPrediction?.Invoke(); }


    // Hides the AP text
    public delegate void HideAPCostPrediction();
    public static event HideAPCostPrediction OnHideAPCostPrediction;
    public static void CallOnHideAPCostPrediction() { OnHideAPCostPrediction?.Invoke(); }

    //Stores last known AP cost
    public static int _lastMovementCost;
    //Updates the UI for AP costs
    public delegate void MovementCostPrediction(int num);
    public static event MovementCostPrediction OnHasMovementPrediction;
    public static void CallOnHasMovementPrediction(int amnt)
    {
        _lastMovementCost = amnt;
        OnHasMovementPrediction?.Invoke(amnt);
    }

  
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
