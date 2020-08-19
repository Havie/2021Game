using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TroopContainer))]
[RequireComponent(typeof(Faction))]
[RequireComponent(typeof(Officer))]
[RequireComponent(typeof(Playable))]
public class iniCharacter : MonoBehaviour
{
    public void Init()
    {
        TroopContainer troopDet = this.GetComponent<TroopContainer>();
        Faction fact = this.GetComponent<Faction>();
        Officer officer = this.GetComponent<Officer>();
        Playable play = this.GetComponent<Playable>();

        troopDet.Init();
        fact.Init();
        officer.Init();
        play.Init();
    }
}
