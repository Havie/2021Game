using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName ="Troop Stats" , menuName="Troop Data/TroopDetails")]
public class TroopType : ScriptableObject
{

    #region stats	
    [SerializeField] private int _morale;
    [SerializeField] private int _attack;
    [SerializeField] private int _defense;
    [SerializeField] private int _will;
    [SerializeField] private int _moveSpeed;
    [SerializeField] private int _ap;
    #endregion

    [SerializeField] private Skill[] _skills;


    public int GetMorale() => _morale;
    public int GetAttack() => _attack;
    public int GetDefense() => _defense;
    public int GetWill() => _will;
    public int GetMoveSpeed() => _moveSpeed;
    public int GetAP() => _ap;
    public Skill[] GetSkills() =>_skills;
   


}
