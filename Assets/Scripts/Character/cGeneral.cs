using Animancer.Examples.Events;
using System.Collections.Generic;
using UnityEngine;


public class cGeneral : MonoBehaviour
{
    private Faction _faction;
    private Army _army; //Can be null
  //  private Skill[] _skills;
    private TroopDetails _troops;
    public Sprite _portrait;
    public string _name;

    #region StatBoosts
    private float _moraleBoost = 1f;
    private float _attackBoost = 1f;
    private float _defenseBoost = 1f;
    private float _willBoost = 1f;
    private float _moveSpeedBoost = 1f;
    private float _apBoost = 1f;
    #endregion

    #region animations

    [SerializeField] Sprite[] _idle;
    [SerializeField] Sprite[] _walk;
    [SerializeField] Sprite[] _attack;
    [SerializeField] Sprite[] _death;
    [SerializeField] Sprite[] _burst;

    #endregion

    #region Getters
    public Army GetArmy() => _army;
    public Sprite GetPortrait() => _portrait;
    public Faction GetFaction() => _faction;
    public string GetName() => _name;
    //For damage calculations
    public int GetMorale() { return _troops !=null ? ((int)_moraleBoost * _troops.GetMorale()) : 0; }
    public int GetAttack() { return _troops != null ? ((int)_attackBoost * _troops.GetAttack()) : 0; }
    public int GetDefense() { return _troops != null ? ((int)_defenseBoost * _troops.GetDefense()) : 0; } 
    public int GetWill() { return _troops != null ? ((int)_willBoost * _troops.GetWill()) : 0; }
    public int GetMoveSpeed() { return _troops != null ? ((int)_moveSpeedBoost * _troops.GetMoveSpeed()) : 0; }
    public int GetAP() { return _troops != null ? ((int)_apBoost * _troops.GetAP()) : 0; }

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

    void Awake()
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
        if (_portrait == null)
            _portrait = Resources.Load<Sprite>("UI/Battle/CharacterPortraits/TmpChar_BattleSelect");
    }
    private void Start()
    {
        _troops = this.GetComponent<TroopDetails>();
    }

}
