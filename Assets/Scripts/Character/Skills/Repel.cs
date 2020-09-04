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
    public override IEnumerator Perform(GameObject self, List<GameObject> targets)
    {
        Debug.Log("Perform Repel");

        //Save Camera initial position
        Vector3 _cameraStart = Camera.main.transform.position;

        Debug.Log("1");

        if (targets != null)
        {
            if (targets.Count > 0)
            {
                //Play Camera and wait till its done 
                CoroutineManager.Instance.StartThread(CameraMovement(1, targets[0].transform.position));
                while (!_cameraDone) // on base Skill script
                {
                    yield return new WaitForEndOfFrame();
                }
            }
            Debug.Log("2");

            //Play Animations and wait till they are ready for damage
            cAnimator sAnimator = self.GetComponentInChildren<cAnimator>();
            if (sAnimator)
            {
                yield return new WaitForSeconds
                    (sAnimator.PlayAnim(cAnimator.AnimationID.BASICATTACK) * 0.75f);
                //Apply Damage and any effects
                foreach (GameObject g in targets)
                {
                    TroopContainer tc = g.GetComponent<TroopContainer>();
                    if (tc)
                        tc.IncrementTroops(-10);

                    //PushBack();
                }
                sAnimator.ReturnToIdle();
            }


            Debug.Log("3");


            //Play Closing Camera animation 
            CoroutineManager.Instance.StartThread(CameraMovement(1, _cameraStart));
        }


        //Let someone know we're done
        SelectionManager.Instance.EnableMove(false);
    }
}
