using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    // Singleton
    public static BattleManager Instance { get; private set; }

    #region Variables
    // Reference to the TurnManager
    private TurnManager _turnManager;

    // Change these to a smaller obj type once we know use
    private List<GameObject> _darkelves;
    private List<GameObject> _orcs;
    #endregion

    #region ArtificalTestVars
    public GameObject[] _chars;
    #endregion

    // Called 0th
    private void Awake()
    {
        // Get Singleton
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
        {
            Debug.LogError("There should never be two BattleManagers in the scene. Original is " 
                + Instance.name + ". A second one was found on " + this.name);
            Destroy(this.gameObject);
        }

        // Make this object persist
        DontDestroyOnLoad(this.gameObject);
    }
    
    // Called 1st
    private void Start()
    {
        // Create lists
        _darkelves = new List<GameObject>();
        _orcs = new List<GameObject>();

        // Iterate over the gameobjects to sort into orcs and elves
        foreach (GameObject go in _chars)
        {
            // Make sure its not null
            if (go)
            {
                iniCharacter initChar = go.GetComponent<iniCharacter>();
                // Initialize the character is they exist
                if (initChar)
                    initChar.Init();
                else
                    Debug.LogError("Could not find iniCharacter attached to " + go.name);

                // Get teh faction
                Faction f = go.GetComponent<Faction>();
                if (f)
                {
                    if (f.IsHuman())
                        _darkelves.Add(go);
                    else
                        _orcs.Add(go);
                }
            }
            else
                Debug.LogError("Null GameObject in BattleManager");
        }

        _turnManager = new TurnManager(true);

        //TMP will have async load start the battle or something in UI 
        StartBattle();
    }

    // Called when the component is destroyed.
    private void OnDestroy()
    {
        _turnManager.Subscribe(false);
    }

    // Called every frame
    private void Update()
    {
        // TEMP for testing
        if (Input.GetKeyDown(KeyCode.N))
            _turnManager.Next();
    }

    /// <summary>
    /// Starts the battle by adding the charactes to the turn manager.
    /// </summary>
    public void StartBattle()
    {
        // Add all characters to the turn manager.
        foreach (GameObject go in _chars)
            _turnManager.AddToList(go);

        // If there are characters, start the first turn.
        if (_chars.Length != 0)
            _turnManager.BeginNewTurn();

        _turnManager.Subscribe(true);
    }

    /// <summary>
    /// Returns the list of forces depending on the bool. Return Elves is true, Orcs if false.
    /// </summary>
    /// <param name="isHuman">Which force to return.</param>
    /// <returns>List<GameObject></returns>
    public List<GameObject> GetForceList(bool isHuman)
    {
        if (isHuman)
            return _darkelves;
        else
            return _orcs;
    }

    //Underdevelopment subject to change
    public bool ManageAttack(Playable attacker, Playable defender)
    {

        if (!attacker || !defender)
            return false;
        Skill basic = attacker.GetComponent<SkillManager>().GetBasicAttack();

        //Check if Enemy (or ally depending on skill?)
        Faction current = attacker.GetComponent<Faction>();
        Faction target = defender.GetComponent<Faction>();

        if (current.IsHuman() == target.IsHuman())
            return ErrorManager.Instance.DisplayError("[TODO] Can't target an Ally (for now)");

        //Check if enough AP (1?)
        if (attacker.GetCurrentAP() < basic.GetAPCost())
            return ErrorManager.Instance.DisplayError("Not enough AP to attack : " +attacker.name);


        List<GameObject> targets = new List<GameObject>();
        targets.Add(defender.transform.gameObject);
        //Check if in Range 
        if (!attacker.EnemiesInRange().Contains(defender))
        {
            //If not in range move to location then attack?
            MovementController mc = attacker.GetComponent<MovementController>(); //cant be null, required 
            mc.DoMovement(CursorController.Instance.transform.position, basic.Perform(attacker.transform.gameObject, targets));
            // TODO Will need to subtract AP Costs of getting there
            
        }
        else
        {
            StartCoroutine(basic.Perform(attacker.transform.gameObject, targets));
        }
        //Subtract AP now?
        attacker.SubtractAP(basic.GetAPCost());

        return true;
    }




    //Underdevelopment subject to change
    public bool ManageSkill(List<Playable> attackers, Playable target, Skill skill)
    {
        Debug.Log("Manage1");
        if (attackers == null || target == null || skill == null)
            return ErrorManager.Instance.DisplayError("ManageSkill: somethings null");
        if (attackers.Count == 0)
            return ErrorManager.Instance.DisplayError("ManageSkill: Missing Attacker");
        if (attackers[0].GetCurrentAP() >= skill.GetAPCost())
        {

            List<GameObject> targets = FindTargets(attackers[0], target.transform.position, skill);

            foreach (Playable attacker in attackers)
                StartCoroutine(skill.Perform(attacker.transform.gameObject, targets));

            //Subtract AP now?
            attackers[0].SubtractAP(skill.GetAPCost());

            return true;
        }
        else
            return false;
    }

    //Underdevelopment subject to change
    public bool ManageSkill(List<Playable> attackers, Vector3 location, Skill skill)
    {
        Debug.Log("Manage2");
        if (attackers == null || location == null || skill == null)
            return ErrorManager.Instance.DisplayError("ManageSkill: somethings null");
        if (attackers.Count == 0)
            return ErrorManager.Instance.DisplayError("ManageSkill: Missing Attacker");

        if (attackers[0].GetCurrentAP() >= skill.GetAPCost())
        {

            List<GameObject> targets = FindTargets(attackers[0], location, skill);
            if (targets.Count < 1)
                return ErrorManager.Instance.DisplayError("ManageSkill: Can't find Target (TMP)");




            foreach (Playable attacker in attackers)
            StartCoroutine(skill.Perform(attacker.transform.gameObject, targets));

            return true;
        }
        else
            return false;
    }


    //Underdevelopment subject to change
    private List<GameObject> FindTargets(Playable user, Vector3 location, Skill skill)
    {
        //Nothing can be null here - safe

        List<GameObject> targets = new List<GameObject>();


        //Find targets based on location and skill radius 
        int radius = skill.GetRadius();
        Collider[] hitColliders = Physics.OverlapSphere(location, radius);
        int i = 0;
        foreach (var hitCollider in hitColliders)
        {
            //Somehow it isnt being added sometimes and giving an odd error ToDo
            Debug.Log("Collider found  " + ++i + "  "+ hitCollider.transform.gameObject);
            Playable p = hitCollider.transform.GetComponent<Playable>();
            if ( p && !targets.Contains(hitCollider.transform.gameObject))
            {
                //Check Enemy or Ally Requirements  depending on skill
                Faction current = user.GetComponent<Faction>();
                Faction target = p.GetComponent<Faction>();

                bool sameFaction = current.IsHuman() == target.IsHuman();
                //Validate if skill effects enemies or allies 
                if (skill.GetIsFriendly() == sameFaction)
                {
                    Debug.Log("Found and added: " + hitCollider.transform.gameObject);
                    targets.Add(hitCollider.transform.gameObject);

                }
                else
                    Debug.Log("Failed check for IS Friendly (" + skill.GetIsFriendly() + " , " + sameFaction +")");



            }
        }



        return targets;
    }

}
