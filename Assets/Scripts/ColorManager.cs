using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorManager : MonoBehaviour
{
    private static ColorManager _instance;

    public Color _purpleAura;
    public Color _redAura;
    public Color _whiteAura;

    public Color _menuValid;
    public Color _menuInvalid;
    public Color _buttonHighlighed;
    public Color _buttonNormal;
    public Color _textValid;
    public Color _textInvalid;

    public static ColorManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = GameObject.FindObjectOfType<ColorManager>();
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else if (_instance != this)
            Destroy(this);
    }

}
