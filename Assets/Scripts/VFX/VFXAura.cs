using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class VFXAura : MonoBehaviour
{
    // This script exists due to 2D billboard rotations messing this effect up
    Material _mat;
    //Not sure how to set these statically
    public enum eColorChoice { WHITE, RED, PURPLE};
    public eColorChoice _color;

    private void Awake()
    {
        if (_mat == null)
            _mat = this.GetComponent<SpriteRenderer>().material;

        switch(_color)
        {
            case eColorChoice.WHITE:
                {
                    SetColor(ColorManager.Instance._white);
                    break;
                }
            case eColorChoice.RED:
                {
                    SetColor(ColorManager.Instance._red);
                    break;
                }
            case eColorChoice.PURPLE:
                {
                    SetColor(ColorManager.Instance._purple);
                    break;
                }
        }
    }
    void LateUpdate()
    {
        // if (_characterToFollow)
        //   this.transform.position = new Vector3(_characterToFollow.position.x, this.transform.position.y, _characterToFollow.position.z);

        this.transform.rotation = Quaternion.Euler(90, 0, 0);

        //TODO
        //If enemies are in range (hit collider?) Activate their glow 

       

    }
    public void SetColor(Color c)
    {
        _mat.SetColor("Color_18F9B8B4", c);
    }
    public void SetSize(Vector3 size)
    {
        this.transform.localScale = size;
    }
}
