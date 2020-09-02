using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cEventSystem
{
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

    // When the camera moves
    public delegate void CameraMove();
    public static event CameraMove OnCameraMove;
    public static void CallOnCameraMove() { OnCameraMove?.Invoke(); }

    // When the camera rotates
    public delegate void CameraRotate();
    public static event CameraRotate OnCameraRotate;
    public static void CallOnCameraRotate() { OnCameraRotate?.Invoke(); }



}
