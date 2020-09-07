using System.Collections.Generic;
using UnityEngine;

public class Playable : MonoBehaviour
{

    public enum eSpriteColor { ENEMY, ALLY, NEUTRAL };

    #region Variables
    [SerializeField] SpriteRenderer _sprRend;
    [SerializeField] AttackRadius _attackRadius;

    public bool _isActive;
    public bool _isSelected;
    private TurnManager _turnManager;
    private bool _isCharacter;
    private GameObject _battleMenu;
    

    private int _AP;
    #endregion

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
            catch // won't happen
            {
                Debug.LogError("Could not find AttackRadius for " + gameObject);
            }
        }
    }
    #endregion

    public bool IsActive() => _isActive; //Its your turn
    public bool IsSelected() => _isSelected; //Selected in UI somehow (currently same as Active)
    public SpriteRenderer GetSpriteRenderer() => _sprRend;

    public void SetActive(bool cond) { _isActive = cond; }
    public int GetCurrentAP() => _AP;
    public void SubtractAP(int amnt)
    {
        _AP -= amnt;
        if (amnt < 0 || amnt>UnitStats._APMAX)
            Debug.LogWarning("You've surpassed the AP limit , check what ur doing");
    }

    /// <summary>
    /// Sets the sprite outline, and if active tells the camera to look here now
    /// </summary>
    /// <param name="cond"></param>
    public void SetSelected(bool cond)
    {
        _isSelected = cond;

        if (cond)
            _sprRend.material = AllMaterials.Instance._outlineSelected;
        else
            _sprRend.material = AllMaterials.Instance._outlineNormal;


        //Tell the camera where to look
        if (cond)
            CameraController.Instance.MoveCameraToPos(this.transform.position);
    }

    /// <summary>
    /// Sets the Color of the Sprite based on an enum
    /// </summary>
    /// <param name="color"></param>
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
        //When you start your turn, refresh your AP:
        RefreshAP();

        _isActive = true;
        //Tell the UI its your turn ??? (Top UI will know from TurnManager)
        //Maybe we tell the side UI ? --Dont like this UI should tell UI

        //Tell the Selection Manager Youre Up
        SelectionManager.Instance.SetActiveCharacter(this);

        //subscribe to  event system that interacts with an end turn button
        //-- could base WHICH event u sub to based on faction vs char
        cEventSystem.OnCharacterTurnEnd += EndTurn;
    }
    public void EndTurn()
    {
        _isActive = false;
        //unsubscribe from event system 
        cEventSystem.OnCharacterTurnEnd -= EndTurn;
    }

    /// <summary>
    /// A UI representation of whos attackable for this character
    /// </summary>
    public void ShowCollidersInRange()
    {
        foreach (Playable p in EnemiesInRange())
            p.SetSpriteOutline(eSpriteColor.ENEMY);  
        foreach (Playable p in EnemiesNotInRange())
            p.SetSpriteOutline(eSpriteColor.NEUTRAL);
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


    private void RefreshAP()
    {
        //Will probably want to cache
        TroopContainer tc = this.GetComponent<TroopContainer>();
        Officer officer = this.GetComponent<Officer>();
        if (tc && officer)
            _AP = (int) (tc.GetAP() * officer.GetAPBoost());


        //Debug.Log("AP for " + this.gameObject.name + "  is:" + _AP);
    }


}
