using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovmentController : MonoBehaviour
{
    private const float MOVE_PRECISION = 1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CreatePath(this.transform.position, this.transform.position + new Vector3(10, 0, 0));
    }

    private void CreatePath(Vector3 fromPoint, Vector3 toPoint)
    {
        Vector3 dir = (fromPoint - toPoint).normalized;
        Ray ray = new Ray(fromPoint, dir);
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(ray, out hit, 1, -1))
        {
            Debug.Log("Hit");
        }
        Debug.Log(hit);
        Debug.DrawLine(ray.origin, toPoint, Color.blue);
    }
}
