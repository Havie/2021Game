using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "AnimSet", menuName = "AnimSet/New AnimSet")]
public class AnimSet : ScriptableObject
{
    [SerializeField] string _name;
    [SerializeField] Animancer.ClipState.Transition[] _animSet;

    public Animancer.ClipState.Transition[] GetAnimSet() => _animSet;
}
