using System;
using System.Collections.Generic;
using UnityEngine;


public class Officer : MonoBehaviour
{
    private Faction _faction;
    private TroopContainer _troopContainer;
    private Army _army; //Can be null
                        //  private Skill[] _skills;
    public string _name;

    #region StatBoosts
    private float _moraleBoost = 1f;
    private float _attackBoost = 1f;
    private float _defenseBoost = 1f;
    private float _willBoost = 1f;
    private float _moveSpeedBoost = 1f;
    private float _apBoost = 1f;
    #endregion



    #region Getters
    public Army GetArmy() => _army;
    public ArtSet GetArtSet() => this.GetComponentInChildren<ArtSet>();  // shouldn't happen often
    public Faction GetFaction() => _faction;
    public string GetName() => _name;

    //raw float
    public float GetMoraleBoost() => _moraleBoost;
    public float GetAttackBoost() => _attackBoost;
    public float GetDefenseBoost() => _defenseBoost;
    public float GetWillBoost() => _willBoost;
    public float GetMoveSpeedBoost() => _moveSpeedBoost;
    public float GetAPBoost() => _apBoost;


    //For UI tells percentage
    public int GetMoraleBonus() => ((int)_moraleBoost * 100) - 100;
    public int GetAttackBonus() => ((int)_attackBoost * 100) - 100;
    public int GetDefenseBonus() => ((int)_defenseBoost * 100) - 100;
    public int GetWillBonus() => ((int)_willBoost * 100) - 100;
    public int GetMoveSpeedBonus() => ((int)_moveSpeedBoost * 100) - 100;
    public int GetAPBonus() => ((int)_apBoost * 100) - 100;


    #endregion

    #region Setters
    public void AssignArmy(Army a) { _army = a; }

    public void SetMoraleBonus(float amnt) { _moraleBoost = amnt; }
    public void SetAttackBonus(float amnt) { _attackBoost = amnt; }
    public void SetDefenseBonus(float amnt) { _defenseBoost = amnt; }
    public void SetWillBonus(float amnt) { _willBoost = amnt; }
    public void SetMoveSpeedBonus(float amnt) { _moveSpeedBoost = amnt; }
    public void SetAPBonus(float amnt) { _apBoost = amnt; }

    #endregion

    public void Init()
    {
        //Try to get army component from parent if we have one?

        //Tell the animator about us 
        cAnimator animator = this.GetComponent<cAnimator>();
        if (animator)
        {

        }
        if (_name.Equals(""))
            _name = gameObject.name;
        if (_faction == null)
            _faction = this.GetComponent<Faction>();
        if (_troopContainer == null)
            _troopContainer = this.GetComponent<TroopContainer>();

    }

    //Should be unused now 
    internal int GetMorale()
    {
       return  _troopContainer.GetMorale();
    }

    internal Sprite GetPortrait()
    {
        Debug.LogWarning("Need to clean this up from calling ref, get some other way");
        //Might wana try to rework arset to be a scriptablebject 
       return  GetArtSet()._portrait;
    }
}
