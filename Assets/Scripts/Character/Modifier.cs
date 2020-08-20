using UnityEngine;


public class Modifier : MonoBehaviour
{
    public enum eStat { MORALE, ATTACK, DEFENSE, WILL, MOVESPEED, AP }
    public enum eType { FLAT, PERCENT }

    //Public for now (Stats uses)
    public eStat _stat;
    public eType _type;
    public int _amnt;

    public void Init(eStat stat, eType type, int amnt)
    {
        _stat = stat;
        _type = type;
        _amnt = amnt;
    }

}
