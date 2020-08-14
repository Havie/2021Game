using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHolder
{
    public Sprite _portrait;
    public Transform _location;
    public bool _isFriendly;
    public string _name;

    public CharacterHolder(Sprite s, Transform t, bool friendly, string name)
    {
        _portrait = s;
        _location = t;
        _isFriendly = friendly;
        _name = name;
    }

}
