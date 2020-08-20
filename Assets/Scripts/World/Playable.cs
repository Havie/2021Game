using UnityEngine;

public class Playable : MonoBehaviour
{
    // SpriteRenderer for this Playable
    [SerializeField]
    private SpriteRenderer _sprRend = null;

    public bool _isActive;
    public bool _isSelected;
    private TurnManager _turnManager;
    private bool _isCharacter;
    private GameObject _battleMenu;

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
            _sprRend = this.GetComponentInChildren<SpriteRenderer>();
    }
    public bool IsActive() => _isActive;
    public bool IsSelected() => _isSelected;

    public void SetActive(bool cond) { _isActive = cond; }
    public void SetSelected(bool cond) { _isSelected = cond; }

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


    /// <summary>
    /// Returns the SpriteRenderer of this playable.
    /// </summary>
    /// <returns>SpriteRenderer</returns>
    public SpriteRenderer GetSpriteRenderer()
    {
        return _sprRend;
    }

}
