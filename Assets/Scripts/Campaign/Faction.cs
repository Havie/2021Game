using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Faction : MonoBehaviour
{

    [SerializeField]private bool _isHuman;
    //Ideally we'd need a faction int ID as well but we only have 2 factions;


    // Start is called before the first frame update
    public void Init()
    {
        
    }

    public bool IsHuman() => _isHuman;
}
