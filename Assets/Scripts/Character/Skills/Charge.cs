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
        Vector3 _cameraStart = CameraController.Instance.transform.rotation.eulerAngles;
        //Play Camera and wait till its done 
        CoroutineManager.Instance.StartThread(
            CameraController.Instance.RevolveCoroutine(new Vector3(15, 90), false)
            );
        Time.timeScale = 1;

        //Do other logic 
        self.GetComponentInChildren<EightDir>().LookAt(targets[0].transform);
            yield return new WaitForSeconds(2.4f);

        //Play Closing Camera animation 
        CoroutineManager.Instance.StartThread(
          CameraController.Instance.RevolveCoroutine(_cameraStart, false)
          );


        //Let someone know we're done
        SelectionManager.Instance.EnableMove(false);
        Time.timeScale = 1f;
    }
}
