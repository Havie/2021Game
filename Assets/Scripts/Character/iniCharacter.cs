using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TroopDetails))]
[RequireComponent(typeof(Faction))]
[RequireComponent(typeof(cGeneral))]
[RequireComponent(typeof(Playable))]
public class iniCharacter : MonoBehaviour
{
    public void Init()
    {
        TroopDetails troopDet = this.GetComponent<TroopDetails>();
        Faction fact = this.GetComponent<Faction>();
        cGeneral general = this.GetComponent<cGeneral>();
        Playable play = this.GetComponent<Playable>();

        troopDet.Init();
        fact.Init();
        general.Init();
        play.Init();
    }
}
