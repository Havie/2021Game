using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TroopDetails))]
[RequireComponent(typeof(Faction))]
[RequireComponent(typeof(Officer))]
[RequireComponent(typeof(Playable))]
public class iniCharacter : MonoBehaviour
{
    public void Init()
    {
        TroopDetails troopDet = this.GetComponent<TroopDetails>();
        Faction fact = this.GetComponent<Faction>();
        Officer officer = this.GetComponent<Officer>();
        Playable play = this.GetComponent<Playable>();

        troopDet.Init();
        fact.Init();
        officer.Init();
        play.Init();
    }
}
