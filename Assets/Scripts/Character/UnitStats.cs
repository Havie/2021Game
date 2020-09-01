using System.Collections.Generic;
using UnityEngine;

//Used by Generals and Troops
public class UnitStats : MonoBehaviour
{

    #region cached
    public Officer _officer;
    public TroopContainer _troops;
    public List<Modifier> _modifiers = new List<Modifier>();
    #endregion

    #region statsMAX	
    public const int _MORALEMAX = 100;
    public const int _ATTACKMAX = 120;
    public const int _DEFENSEMAX = 120;
    public const int _WILLMAX = 100;
    public const int _MOVESPEEDMAX = 10;
    public const int _APMAX = 10;
    #endregion

    #region stats	
    //These are used on a per battle basis, do not affect the physical troop values
    private int _currWill;
    private int _currAP;
    private int _currAPMAX;
    #endregion


    private void Awake()
    {
        //Can be Null probably ok
        if (_officer == null)
            _officer = this.GetComponent<Officer>();
        if (_troops = null)
            _troops = this.GetComponent<TroopContainer>();
    }

    public void InitBattle()
    {
        //Find the Current val for each stat from the general / troops? 
        if (_officer == null || _troops == null)
            Debug.LogError(" Officer or troops are null,m will crash");
        else
        {
            _currWill =(int) _officer.GetWillBoost() * _troops.GetWill();
            _currAPMAX = (int)_officer.GetAPBoost() * _troops.GetAP();
            _currAP = _currAPMAX;
        }
    }
    public void StartTurn()
    {
        _currAP = _currAPMAX;
        _currWill += (int)GetMorale() / 10;
        //Display VFX 
    }


    public void IncrementWill(int amnt)
    {
        _currWill += amnt;
        if (_currWill > _WILLMAX)
            _currWill = _WILLMAX;
        else if (_currWill < 0) // should not happen 
        {
            _currWill = 0;
            Debug.LogWarning("Something is trying to set Will below zero, should not happen");
        }
    }
    public void IncrementAP(int amnt)
    {
        _currAP += amnt;
        if (_currAP > _currAPMAX)
            _currAP = _currAPMAX;
        else if (_currAP < 0) // should not happen 
        {
            _currAP = 0;
            Debug.LogWarning("Something is trying to set AP below zero, should not happen");
        }
    }

    public void IncrementTroops(int amnt)
    {
        //actually adjust the troops on _troops so this result will carry over to campaign
        if (_troops)
            _troops.IncrementTroops(amnt);
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

    public int GetMorale()
    {
        if (_officer == null || _troops == null)
            return 0;

        int tMorale = _troops.GetMorale();
        float oMorale = _officer.GetMoraleBoost();


        return Mathf.Clamp(TallyModifier(tMorale, oMorale, Modifier.eStat.MORALE), 0, _MORALEMAX);
    }

    public int GetAttack()
    {
        if (_officer == null || _troops == null)
            return 0;
        int tAttack = _troops.GetAttack();
        float oAttack = _officer.GetAttackBoost();

        return Mathf.Clamp(TallyModifier(tAttack, oAttack, Modifier.eStat.ATTACK), 0, _ATTACKMAX);
    }
    public int GetDefense()
    {
        if (_officer == null || _troops == null)
            return 0;
        int tDefense = _troops.GetDefense();
        float oDefense = _officer.GetDefenseBoost();

        return Mathf.Clamp(TallyModifier(tDefense, oDefense, Modifier.eStat.DEFENSE), 0, _DEFENSEMAX);
    }
    public int GetWill()
    {
        if (_officer == null || _troops == null)
            return 0;

        return Mathf.Clamp(TallyModifier(_currWill, 1, Modifier.eStat.DEFENSE), 0, _WILLMAX);
    }
    public int GetMoveSpeed()
    {
        if (_officer == null || _troops == null)
            return 0;

        int tMoveSpeed = _troops.GetMoveSpeed();
        float oMoveSpeed = _officer.GetMoveSpeedBoost();

        return Mathf.Clamp(TallyModifier(tMoveSpeed, oMoveSpeed, Modifier.eStat.MOVESPEED), 0, _MOVESPEEDMAX);
    }

    /**
	* Helper function to calculate the modifiers on top of initial stats
	*/
    private int TallyModifier(int flat, float bonus, Modifier.eStat stat)
    {
        //shouldn't be a problem if empty yea?
        foreach(Modifier m in _modifiers)

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







}
