using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnCharacter : MonoBehaviour
{
    // Event for when the characer has finished their turn.
    public delegate void TurnFinish();
    public static TurnFinish OnTurnFinish;

    /// <summary>
    /// Starts the character taking their turn.
    /// </summary>
    public void TakeTurn()
    {
        StartCoroutine(TurnLoop());
    }

    /// <summary>
    /// Waits for the character to finish doing their actions and 
    /// then calls the event for the character finishing their turn.
    /// </summary>
    /// <returns>IEnumerator</returns>
    private IEnumerator TurnLoop()
    {
        // Wait until the character is done with their actions.
        yield return DoActions();

        // Call the event for their turn being finished
        OnTurnFinish?.Invoke();

        yield return null;
    }

    /// <summary>
    /// To be overriden by child classes. Does the actios for the character.
    /// </summary>
    /// <returns>IEnumerator</returns>
    protected virtual IEnumerator DoActions() { yield return null; }
}
