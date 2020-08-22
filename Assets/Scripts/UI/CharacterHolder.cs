using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHolder
{
    public Sprite _portrait;
    public Transform _location;
    public bool _isFriendly;
    public string _name;

    /// <summary>
    /// Used by UICharacterTurnManager to store data 
    /// </summary>
    /// <param name="sprite portrait"></param>
    /// <param name="transform loc"></param>
    /// <param name="friendly"></param>
    /// <param name="name"></param>
    public CharacterHolder(Sprite s, Transform t, bool friendly, string name)
    {
        _portrait = s;
        _location = t;
        _isFriendly = friendly;
        _name = name;
    }

}
