using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "All Troops", menuName = "Troop Data/All Troops")]
public class AllTroops : SingletonScriptableObject<AllTroops>
{
    public  List<TroopType> _troopsDE;
    public  List<TroopType> _troopsORC;

    
}
