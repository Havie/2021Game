using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRadius : MonoBehaviour
{

    public bool _isOn=true; // not used
    private bool _coroutineOn; // not used

    public float _radius; //kinda used 
    public Vector3 _direction; //notused
    public float _maxDis; //kinda used
    public LayerMask _layermask; //notused

    private float _currentHitDistance; //not used


    public bool _ImHuman;

    private void Start()
    {
        _radius = this.transform.localScale.x/2;
        _maxDis = _radius;
    }


    void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.R))
            DetectEnemies();



        //CircleCast();
        //if(!_coroutineOn)
        // StartCoroutine(RadiusSweep());
    }

    public void DetectEnemies()
    {

        foreach (GameObject g in BattleManager.Instance.GetForceList(!_ImHuman))
        {
            if (Vector3.Distance(this.transform.position, g.transform.position) < _maxDis)
            {
                Debug.Log(" In range: " + g);
                try
                { g.GetComponent<Playable>().GetSpriteRenderer().material = SelectionManager.Instance._enemy; }
                catch
                { Debug.LogWarning("Missing SpriteRender for " + gameObject.name); }
            }
            else
            {

                Debug.Log("Not in range: " + g + "  dis=" + Vector3.Distance(this.transform.position, g.transform.position));
                try
                    { g.GetComponent<Playable>().GetSpriteRenderer().material = SelectionManager.Instance._normal; }
                catch
                    { Debug.LogWarning("Missing SpriteRender for " + gameObject.name); }

            }
        }

    }




    /*
    * None of these below work proper cant figure out how to detect anything in a radius
    */



    //This is so not efficient to call every Frame
    IEnumerator RadiusSweep()
    {
        Debug.Log("STARTED?");
        _coroutineOn = true;
        float count = 0;
        while(count<361)
        {
            Debug.Log("Count= " + count);
            count += 0.5f +Time.deltaTime;
            Vector3 endpoint = new Vector3(Mathf.Cos(count), 0, Mathf.Sin(count));
            Debug.DrawLine(this.transform.position, this.transform.position + endpoint * _radius, Color.red, 0.5f);
        }

        _coroutineOn = false;
        yield return null;
    }

    //This somehow only picks up rock colliders, not other characters???
    private void SphereCast()
    {
        _direction = Vector3.zero;
        if (_isOn)
        {
            RaycastHit hit;
            if (Physics.SphereCast(this.transform.position, 2, _direction, out hit, _maxDis, _layermask, QueryTriggerInteraction.UseGlobal))
            {
                Debug.Log(hit.transform.name);
                _currentHitDistance = hit.distance;
            }
            else
                _currentHitDistance = _maxDis;
        }
    }
    private void CircleCast()
    {
        Debug.Log("circleCast");
        if (_isOn)
        {
            RaycastHit2D[] hit = Physics2D.CircleCastAll(this.transform.position, _radius, _direction, _maxDis, _layermask);
            foreach(RaycastHit2D h in hit)
            {
               Debug.Log(h.transform.gameObject.name);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
       // Gizmos.color = Color.red;
       // Debug.DrawLine(this.transform.position, this.transform.position + _direction * _currentHitDistance);
        //Gizmos.DrawWireSphere(this.transform.position + _direction * _currentHitDistance, _sphereRadius);

    }
}
