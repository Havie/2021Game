using System;
using System.Collections.Generic;
using UnityEngine;

public class TroopContainer : MonoBehaviour
{

    public enum eTroopType { SWORD, BOW, POLEAXE };
    private eTroopType _etype;
    [SerializeField] private TroopType _type;


    [SerializeField] private int _hpMAX=10000;
    [SerializeField] private int _hpSword;
    [SerializeField] private int _hpBow;
    [SerializeField] private int _hpPoleaxe;

    public List<Modifier> _modifiers = new List<Modifier>();
    public bool test;

    internal void Init()
    {
        Debug.Log("Todo Init");
        SetType(eTroopType.SWORD);
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
        //TMP - TURN OFF
        IncrementTroops(1000);
    }

    #region SimpleGetters
    public Army GetArmy() => this.GetComponent<Army>(); // shouldn't be used often

    //These will have to be expanded to handle modifiers (TallyModifier) See UnitStats and General boosts
    public int GetMorale() { return _type != null ? (_type.GetMorale()) : 0; }
    public int GetAttack() { return _type != null ? (_type.GetAttack()) : 0; }
    public int GetDefense() { return _type != null ? (_type.GetDefense()) : 0; }
    public int GetWill() { return _type != null ? (_type.GetWill()) : 0; }
    public int GetMoveSpeed() { return _type != null ? (_type.GetMoveSpeed()) : 0; }
    public int GetAP() { return _type != null ? (_type.GetAP()) : 0; }

    public int GetHP() => GetProperHP();
    public int GetHPMAX() => _hpMAX;
    public TroopType GetTroopType() => _type;

    public Skill[] GetSkills() { return _type != null ? (_type.GetSkills()) : null; }
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

        if (AllTroops.Instance == null)
            Debug.LogWarning("ALL TROOPs IS NULL, no idea why");

      
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
                    if (_hpSword <= 0)
                    {
                        _hpSword = 0;
                        Die();
                    }
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

    private void Die()
    {
        //TODO : IF in Battle Cond

        //Play Anim (will remove from BM list thru event system)
        cAnimator animator = this.GetComponentInChildren<cAnimator>();
        animator.PlayAnim(cAnimator.AnimationID.DEATH);
    }


    public void SetModifier(Modifier mod)
    {
        //Might have to pass in values to instantiate new, OR a prefab modifier?

        //set a child of this object and add to list 
    }
    public void RemoveModifier(Modifier mod)
    {
        if (_modifiers.Contains(mod))

        {
            //Not sure if you can pass in the exact modifier
            // or if you remove a modifier of a certain type

            //More than likely the modifier itself has an update loop (or subbed to event sys) and when it expires
            // it calls this method to be removed from the list 
        }
    }
    /**
    * Helper function to calculate the modifiers on top of initial stats
    */
    private int TallyModifier(int flat, float bonus, Modifier.eStat stat)
    {
        //shouldn't be a problem if empty yea?
        foreach (Modifier m in _modifiers)

        {
            if (m._stat == stat)
            {
                if (m._type == Modifier.eType.FLAT)
                    flat += m._amnt;
                else if (m._type == Modifier.eType.PERCENT)
                    bonus += m._amnt;
            }
        }

        return (int)bonus * flat;
    }

    private int RNG() => UnityEngine.Random.Range(1, 90);


}
