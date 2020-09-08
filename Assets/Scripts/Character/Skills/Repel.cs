using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/* Implements Skill which is a ScriptableObject
 * For Info on available methods please see the base class : Skill 
 * All Skills operate as a Coroutine and must be run by the event system since
 * ScriptableObjects can not start their own Coroutines.
 */


[CreateAssetMenu(fileName = "Skills", menuName = "Skills/Repel")]
public class Repel : Skill
{
    
}
