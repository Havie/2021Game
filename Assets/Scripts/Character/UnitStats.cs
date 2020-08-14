using System.Collections.Generic;
using UnityEngine;

//Used by Generals and Troops
public class UnitStats
{

    #region statsMAX	
    public const int _MORALEMAX = 100;
    public const int _ATTACKMAX = 120;
    public const int _DEFENSEMAX = 120;
    public const int _WILLMAX = 100;
    public const int _MOVESPEEDMAX = 10;
    public const int _APMAX = 10;
    #endregion

    #region stats	
    private int _morale;
    private int _attack;
    private int _defense;
    private int _will;
    private int _moveSpeed;
    private int _ap;
    #endregion

    public UnitStats(int m, int a, int d, int w, int ms, int ap)
    {
        _morale = m;
        _attack = a;
        _defense = d;
        _will = w;
        _moveSpeed = ms;
        _ap = ap;
    }


    public int GetMorale() => _morale;
    public int GetAttack() => _attack;
    public int GetDefense() => _defense;
    public int GetWill() => _will;
    public int GetMoveSpeed() => _moveSpeed;
    public int GetAP() => _ap;

    public void setMorale(int amnt)
    {
        _morale += amnt;
        if ((_morale > UnitStats._MORALEMAX))

            _morale = UnitStats._MORALEMAX;
        else if ((_morale) < 0)
            _morale = 0;
    }
    public void setAttack(int amnt)
    {
        _attack += amnt;
        if ((_attack > UnitStats._ATTACKMAX))

            _attack = UnitStats._ATTACKMAX;
        else if ((_attack) < 0)
            _attack = 0;
    }
    public void setDefense(int amnt)
    {
        _defense += amnt;
        if ((_defense > UnitStats._DEFENSEMAX))

            _defense = UnitStats._DEFENSEMAX;
        else if ((_defense) < 0)
            _defense = 0;
    }
    public void setWill(int amnt)
    {
        _will += amnt;
        if ((_will > UnitStats._WILLMAX))

            _will = UnitStats._WILLMAX;
        else if ((_will) < 0)
            _will = 0;
    }
    public void setMoveSpeed(int amnt)
    {
        _moveSpeed += amnt;
        if ((_moveSpeed > UnitStats._MOVESPEEDMAX))

            _moveSpeed = UnitStats._MOVESPEEDMAX;
        else if ((_moveSpeed) < 0)
            _moveSpeed = 0;
    }
    public void setAP(int amnt)
    {
        _ap += amnt;
        if ((_ap > UnitStats._APMAX))

            _ap = UnitStats._APMAX;
        else if ((_ap) < 0)
            _ap = 0;
    }


}
