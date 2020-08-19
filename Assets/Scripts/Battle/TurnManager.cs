using System.Collections.Generic;
using UnityEngine;
using static cEventSystem;


// I do not think its possible to write this in an abstract way that takes in Factions and Characters
// Because the determine order function will be different? Could just Override that?

// How to handle this turn manager between loading campaign vs loading battle??
// Maybe we need to make a singleton of Campaign and cache its data while battle plays
// then load the cache and results of battle (or campaign singleton comes with)

//Dont extend monobehavior so no start/update
//Maybe put this in a BattleManager which is responsible for returning info to campaign?
public class TurnManager
{
    private bool _isBattle; //determines if we change the order each turn of who goes first.
    private List<GameObject> _allPossible = new List<GameObject>();
    private List<GameObject> _inOrder = new List<GameObject>();
    private int _turnNo;
    private int _index;

    public void Subscribe(bool cond)
    {
        if (cond)
            if (cEventSystem.Instance)
                cEventSystem.Instance.ACT += Next;
       else //Is this Legal? lol seems to work 
            if (cEventSystem.Instance)
                cEventSystem.Instance.ABR -= Next;
    }
    //Constructor
    public TurnManager(bool isBattle)
    {
        _isBattle = isBattle;
    }
    /**
     *Takes in game objects to cycle through (Characters or Factions) 
     */
    public void AddToList(GameObject go)
    {
        // TODO Based on battlemode should verify they are char vs faction 
        if (!_allPossible.Contains(go))
            _allPossible.Add(go);
    }
    /**
     * Called from Next() which is subscribed to the event system 
     * Will determine the order and inform the UI 
     */
    public void BeginNewTurn()
    {
        Debug.Log("Begin new Turn!");
        if (_turnNo == 0)
            UICharacterTurnManager.Instance.StartBattle();
        //Debug.Log("Begin Turn: "+_turnNo);
        _index = 0;
        ++_turnNo;
        _inOrder = DetermineOrder();
        Next();
    }
    /**
     * Character: Order based on Morale
     * Factions : Default Order passed in
     * Updates UI
     */
    private List<GameObject> DetermineOrder()
    {
        if (!_isBattle)
            return _allPossible;

        //Sort List based on character Morale. 
        List<GameObject> newOrder = new List<GameObject>(); //okay to create new or better to cache and clear?
        int iterations = _allPossible.Count;
        int highestMorale = -1;
        GameObject lastGo = null;
        while (iterations != 0)
        {
            foreach (GameObject go in _allPossible)
            {
                Debug.Log(go);
                if (!newOrder.Contains(go))
                {
                    cGeneral general = go.GetComponent<cGeneral>();
                    if (general)
                    {
                        if (general.GetMorale() > highestMorale)
                        {
                            lastGo = go;
                            highestMorale = general.GetMorale();
                        }
                    }
                    else
                        Debug.LogError("passed in Character doesnt have a General");
                }
            }
            highestMorale = -1; //reset
            newOrder.Add(lastGo);
            --iterations;

        }
        //Tell The UI Turn Manager The order 
        UICharacterTurnManager.Instance.SetUpTurn(newOrder);
        return newOrder;
    }

    /**
     * Advances to the next turn , letting the playable object Know its their turn 
     * If last item, starts next round
     * Called from the Event System
     */
    public void Next()
    {
        if (_index == _inOrder.Count) // -1 ?
            BeginNewTurn();
        else
        {
            //Both Factions and Characters implement Playable
            Playable p = _inOrder[_index++].GetComponent<Playable>();
            if (p)
                p.YourTurn(this); //Tells the selection Manager
        }
    }

}
