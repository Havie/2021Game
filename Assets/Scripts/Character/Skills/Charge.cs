using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/* Implements Skill which is a ScriptableObject
 * For Info on available methods please see the base class : Skill 
 * All Skills operate as a Coroutine and must be run by the event system since
 * ScriptableObjects can not start their own Coroutines.
 */


[CreateAssetMenu(fileName = "Skills", menuName = "Skills/Charge")]
public class Charge : Skill
{
    public override IEnumerator Perform(GameObject self, List<GameObject> targets)
    {
        Debug.Log("Perform Charge");

        //Save Camera initial position
        Vector3 _cameraStart = Camera.main.transform.position;
        //Play Camera and wait till its done 
        cEventSystem.Instance.StartCoroutine(CameraMovement(1, targets[0].transform.position));
        while(!_cameraDone) // on base Skill script
        {
            yield return new WaitForEndOfFrame();
        }

        //Play Animations and wait till they are ready for damage
        cAnimator sAnimator= self.GetComponentInChildren<cAnimator>();
        if(sAnimator)
        {
            yield return new WaitForSeconds
                (sAnimator.PlayAnim(cAnimator.AnimationID.BASICATTACK)*0.75f);
            //Apply Damage and any effects
            foreach (GameObject g in targets)
            {
                TroopContainer tc = g.GetComponent<TroopContainer>();
                if (tc)
                    tc.IncrementTroops(-10);

                PushBack();
            }
            sAnimator.ReturnToIdle();
        }


        //Play Closing Camera animation 
        cEventSystem.Instance.StartCoroutine(CameraMovement(1, _cameraStart));


        //Let someone know we're done
        SelectionManager.Instance.EnableMove(false);
    }
}
