using System.Collections.Generic;
using UnityEngine;


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


    public TurnManager(bool isBattle)
    {
        _isBattle = isBattle;
    }
    public void AddToList(GameObject go)
    {
        if (!_allPossible.Contains(go))
            _allPossible.Add(go);
    }
    public void BeginTurn()
    {
        if (_turnNo == 0)
            UICharacterTurnManager.Instance.StartBattle();
        //Debug.Log("Begin Turn: "+_turnNo);
        _index = 0;
        ++_turnNo;
        _inOrder = DetermineOrder();
        Next();
    }
    private List<GameObject> DetermineOrder()
    {
        if (!_isBattle)
            return _allPossible;

        //Sort List based on character Morale. 
        List<GameObject> newOrder = new List<GameObject>(); //okay to create new or better to cache and clear?
        int iterations = _allPossible.Count;
        int highestMorale = 0;
        GameObject lastGo = null;
        while (iterations != 0)
        {
            foreach (GameObject go in _allPossible)
            {
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
            highestMorale = 0; //reset
            newOrder.Add(lastGo);
            --iterations;

        }
        //Tell The UI Turn Manager The order 
        UICharacterTurnManager.Instance.SetUpTurn(newOrder);
        return newOrder;
    }
    public void Next()
    {
        if (_index == _inOrder.Count) // -1 ?
            BeginTurn();
        else
        {
            //Both Factions and Characters implement Playable
            Playable p = _inOrder[_index++].GetComponent<Playable>();
            if (p)
                p.YourTurn(this); // will call Next() when its done 
           // Debug.Log("TURN:" + p.gameObject.name);
        }
    }

}
