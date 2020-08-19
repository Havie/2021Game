using System.Collections.Generic;
using UnityEngine;

public class TroopContainer : MonoBehaviour
{

    public enum eTroopType { SWORD, BOW, POLEAXE };
    public eTroopType _type;

    private TroopStats _troopStats;
    // private TroopType _type;
    //private Skill[] _skills;
    private int _hpMAX;
    private int _hp;

    public bool test;

    #region animations 
    // Might only use these for *juice* death anims on field
    //[SerializeField] Sprite[] _idle;
    //[SerializeField] Sprite[] _walk;
    //[SerializeField] Sprite[] _attack;
    //[SerializeField] Sprite[] _death;
    //[SerializeField] Sprite[] _burst;
    #endregion

    public Army GetArmy() => this.GetComponent<Army>(); // shouldn't be used often
                                                        // public Skills _GetSkills() _skills;

    public int GetMorale() { return _troopStats != null ? (_troopStats.GetMorale()) : 0; }
    public int GetAttack() { return _troopStats != null ? (_troopStats.GetAttack()) : 0; }
    public int GetDefense() { return _troopStats != null ? (_troopStats.GetDefense()) : 0; }
    public int GetWill() { return _troopStats != null ? (_troopStats.GetWill()) : 0; }
    public int GetMoveSpeed() { return _troopStats != null ? (_troopStats.GetMoveSpeed()) : 0; }
    public int GetAP() { return _troopStats != null ? (_troopStats.GetAP()) : 0; }
    public int GetHP() => _hp;
    public int GetHPMAX() => _hpMAX;
    public eTroopType GetType() => _type;

    public void AssignArmy(Army a) { Debug.Log("TODO"); }
    public int SetType(eTroopType t)
    {
        _type = t;
        UpdateStats();

        if (_troopStats!=null)
        {
            //_hpMAX = _unitStats.GetMaxHp(); //based on level
        }

        //Have to handle swapping men
        int tmp = _hp;
        _hp = 0;
        return tmp;
    }
    private void UpdateStats()
    {
        Transform t = this.transform.GetChild((int)_type);
        if (t)
            _troopStats = t.GetComponent<TroopStats>();
        else
            _troopStats = null;
    }
    public void IncrementTroops(int amnt)
    {
        _hp += amnt;
        if (_hp < 0)
            _hp = 0;
        else if (_hp > _hpMAX)
            _hp = _hpMAX;
    }

    public void Init(bool darkelf)
    {
        GameObject sword;
        GameObject bow;
        GameObject poleaxe;

        //Get from Prefabs
        if (darkelf)
        {
            sword = Resources.Load<GameObject>("Prefabs/SwordDE");
            bow = Resources.Load<GameObject>("Prefabs/BowDE");
            poleaxe = Resources.Load<GameObject>("Prefabs/PoleDE");

        }
        else //orc
        {
            sword = Resources.Load<GameObject>("Prefabs/SwordORC");
            bow = Resources.Load<GameObject>("Prefabs/BowORC");
            poleaxe = Resources.Load<GameObject>("Prefabs/PoleORC");
        }

        //Spawn Prefabs as Children
        if (sword && bow && poleaxe)
        {
            sword = Instantiate(sword);
            bow = Instantiate(bow);
            poleaxe = Instantiate(poleaxe);

            sword.transform.parent = this.transform;
            bow.transform.parent = this.transform;
            poleaxe.transform.parent = this.transform;
        }
        else
            Debug.LogError(" Troop Prefabs missing for " + this.gameObject.name);



    }
    private int RNG()
    {
        return Random.Range(1, 90);
    }


}
