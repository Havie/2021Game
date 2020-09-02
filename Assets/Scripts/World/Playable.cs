using System.Collections.Generic;
using UnityEngine;

public class Playable : MonoBehaviour
{

    public enum eSpriteColor { ENEMY, ALLY, NEUTRAL};

    // SpriteRenderer for this Playable
    [SerializeField]
    private SpriteRenderer _sprRend;
    [SerializeField]
    private AttackRadius _attackRadius;

    public bool _isActive;
    public bool _isSelected;
    private TurnManager _turnManager;
    private bool _isCharacter;
    private GameObject _battleMenu;

    #region SetupMethods

    private void Awake()
    {
        //This could be on a faction OR a Character so we have to operate on GameObject?
        //Could try to getComponent character vs faction to set a bool that will tell this script how to operate.
    }
    public void Init()
    {
        //TMP
        _isCharacter = true;


        //Used by selection manager frequently, so cache this
        if (_sprRend == null)
        {
            try
            {
                _sprRend = this.GetComponentInChildren<ArtSet>().GetComponent<SpriteRenderer>();
            }
            catch
            {
                Debug.LogError("Could not find Sprite rendender or art set for " + gameObject);
            }
        }
        if (_attackRadius == null)
        {
            try
            {
                _attackRadius = this.GetComponentInChildren<AttackRadius>();
            }
            catch
            {
                Debug.LogError("Could not find AttackRadius for " + gameObject);
            }
        }
    }
    #endregion

    public bool IsActive() => _isActive;
    public bool IsSelected() => _isSelected;
    public SpriteRenderer GetSpriteRenderer() => _sprRend;

    public void SetActive(bool cond) { _isActive = cond; }
    public void SetSelected(bool cond) { _isSelected = cond; }


    public void SetSpriteOutline(eSpriteColor color)
    {
        if (color == eSpriteColor.ALLY)
            _sprRend.material = AllMaterials.Instance._outlineAllied;
        else if (color == eSpriteColor.ENEMY)
            _sprRend.material = AllMaterials.Instance._outlineEnemy;
        else if (color == eSpriteColor.NEUTRAL)
            _sprRend.material = AllMaterials.Instance._outlineNormal;
    }

    public void YourTurn(TurnManager t)
    {
        _isActive = true;
        //Tell the UI its your turn ??? (Top UI will know from TurnManager)
        //Maybe we tell the side UI ? 

        //Tell the Selection Manager Youre Up
        SelectionManager.Instance.SetActiveCharacter(this);

        //subscribe to  event system that interacts with an end turn button
        //-- could base WHICH event u sub to based on faction vs char
        cEventSystem.Instance.ACT += EndTurn;
    }
    public void EndTurn()
    {
        _isActive = false;
        //unsubscribe from event system 
        cEventSystem.Instance.ACT -= EndTurn;
    }

    public List<Playable> EnemiesInRange()
    {
        if (_attackRadius == null)
            return null;

        return _attackRadius.DetectEnemies()["INRANGE"];

    }
    public List<Playable> EnemiesNotInRange()
    {
        if (_attackRadius == null)
            return null;

        return _attackRadius.DetectEnemies()["NOTINRANGE"];

    }



}
