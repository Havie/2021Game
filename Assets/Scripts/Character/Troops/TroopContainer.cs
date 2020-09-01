using System;
using System.Collections.Generic;
using UnityEngine;

public class TroopContainer : MonoBehaviour
{

    public enum eTroopType { SWORD, BOW, POLEAXE };
    private eTroopType _etype;
    [SerializeField] private TroopType _type;

    //private Skill[] _skills;
    [SerializeField] private int _hpMAX=10000;
    [SerializeField] private int _hpSword;
    [SerializeField] private int _hpBow;
    [SerializeField] private int _hpPoleaxe;

    public bool test;

    internal void Init()
    {
        Debug.Log("Todo Init");
    }

    #region animations 
    // Might only use these for *juice* death anims on field
    //[SerializeField] Sprite[] _idle;
    //[SerializeField] Sprite[] _walk;
    //[SerializeField] Sprite[] _attack;
    //[SerializeField] Sprite[] _death;
    //[SerializeField] Sprite[] _burst;
    #endregion

    private void Start()
    {
        SetType(eTroopType.SWORD);
        //TMP - TURN OFF
        IncrementTroops(1000);
    }

    #region SimpleGetters
    public Army GetArmy() => this.GetComponent<Army>(); // shouldn't be used often
    // public Skills _GetSkills() _skills;
    public int GetMorale() { return _type != null ? (_type.GetMorale()) : 0; }
    public int GetAttack() { return _type != null ? (_type.GetAttack()) : 0; }
    public int GetDefense() { return _type != null ? (_type.GetDefense()) : 0; }
    public int GetWill() { return _type != null ? (_type.GetWill()) : 0; }
    public int GetMoveSpeed() { return _type != null ? (_type.GetMoveSpeed()) : 0; }
    public int GetAP() { return _type != null ? (_type.GetAP()) : 0; }
    public int GetHP() => GetProperHP();
    public int GetHPMAX() => _hpMAX;
    public TroopType GetTroopType() => _type;
    #endregion

    #region SimpleSetters
    public void SetMaxTroops(int amnt) => _hpMAX = amnt;
    public void AssignArmy(Army a) { Debug.Log("TODO AssignArmy"); }
    #endregion


    /// <summary>
    /// Sets the troop type based on the faction this character is apart of
    /// </summary>
    /// <param name="type"></param>
    public void SetType(eTroopType type)
    {
        _etype = type;
        Faction faction = this.GetComponent<Faction>();
        if (faction==null)
            return;

        if (faction.IsHuman())
            _type = AllTroops.Instance._troopsDE[(int)type];
        else
            _type = AllTroops.Instance._troopsORC[(int)type];

    }

    public void IncrementTroops(int amnt)
    {
        //Cant pass by reference on a primitive 
        SetProperHP(amnt);
    }

    private int GetProperHP()
    {
        switch(_etype)
        {
            case eTroopType.SWORD:
                {
                    return _hpSword;
                }
            case eTroopType.BOW:
                {
                    return _hpBow;
                }
            case eTroopType.POLEAXE:
                {
                    return _hpPoleaxe;
                }
        }

        return 0;
    }
    private void SetProperHP(int amnt)
    {
        switch (_etype)
        {
            case eTroopType.SWORD:
                {
                    _hpSword += amnt;
                    if (_hpSword < 0)
                        _hpSword = 0;
                    else if (_hpSword > _hpMAX)
                        _hpSword = _hpMAX;
                    break;
                }
            case eTroopType.BOW:
                {
                    _hpBow += amnt;
                    if (_hpBow < 0)
                        _hpBow = 0;
                    else if (_hpBow > _hpMAX)
                        _hpBow = _hpMAX;
                    break;
                }
            case eTroopType.POLEAXE:
                {
                    _hpPoleaxe += amnt;
                    if (_hpPoleaxe < 0)
                        _hpPoleaxe = 0;
                    else if (_hpPoleaxe > _hpMAX)
                        _hpPoleaxe = _hpMAX;
                    break;
                }
        }
    }

    private int RNG() => UnityEngine.Random.Range(1, 90);


}
