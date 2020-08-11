using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldObj : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        FaceCamera();
    }

    private void FaceCamera()
    {
        Vector3 camEuler = Camera.main.transform.rotation.eulerAngles;
        this.transform.rotation = Quaternion.Euler(camEuler.x, camEuler.y, camEuler.z);
        UpdateSprite();
    }

    protected virtual void UpdateSprite() { }
}
