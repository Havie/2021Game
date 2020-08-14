using System.Collections.Generic;
using UnityEngine;

public class TroopDetails : MonoBehaviour
{
    private Army _army;
   // private TroopType _type;
    //private Skill[] _skills;
    private UnitStats _unitStats;
    private int _hpMAX;
    private int _hp;

    #region animations 
    // Might only use these for *juice* death anims on field

    [SerializeField] Sprite[] _idle;
    [SerializeField] Sprite[] _walk;
    [SerializeField] Sprite[] _attack;
    [SerializeField] Sprite[] _death;
    [SerializeField] Sprite[] _burst;

    #endregion

    public Army GetArmy() => _army;
   // public TroopType GetType() _type;
  // public Skills _GetSkills() _skills;
	public UnitStats GetUnitStats() => _unitStats;
    public void SetUnitStats(UnitStats unitStats) { _unitStats = unitStats; }

    public int GetMorale() { return _unitStats != null ? (_unitStats.GetMorale()) : 0; }
    public int GetAttack() { return _unitStats != null ? (_unitStats.GetAttack()) : 0; }
    public int GetDefense() { return _unitStats != null ? (_unitStats.GetDefense()) : 0; }
    public int GetWill() { return _unitStats != null ? (_unitStats.GetWill()) : 0; }
    public int GetMoveSpeed() { return _unitStats != null ? (_unitStats.GetMoveSpeed()) : 0; }
    public int GetAP() { return _unitStats != null ? (_unitStats.GetAP()) : 0; }
    public int GetHP() => _hp;
    public int GetHPMAX() => _hpMAX;

    public void AssignArmy(Army a) { _army = a; }
    //public void SetType(TroopType t) { _type = t};

    public void Start()
    {

        _unitStats = new UnitStats(RNG(), RNG(), RNG(), RNG(), RNG(), RNG());
    }
    private int RNG()
    {
        return Random.Range(1, 99);
    }


}
