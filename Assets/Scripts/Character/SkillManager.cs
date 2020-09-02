using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    [SerializeField] TroopContainer _troops;
    [SerializeField] Officer _officer;

    private void Awake()
    {
        if (_troops == null)
            _troops = this.GetComponent<TroopContainer>();
        if (_officer == null)
            _officer = this.GetComponent<Officer>();

        if (!_troops || !_officer)
            Debug.LogError("Missing Troops or officer for " + this.gameObject.name);
    }

    /// <summary>
    /// Gets all the available skills associated with this game object
    /// </summary>
    /// <returns>List of Skills</returns>
    public List<Skill> GetSkills()
    {
        List<Skill> skillList = new List<Skill>();

        if(_troops)
        {
            foreach(Skill s in _troops.GetSkills())
            {
               skillList.Add(s);
            }
        }

        if(_officer)
        {
            //ToDo
        }

        return skillList;
    }

    public Skill GetBasicAttack()
    {
      return AllSkills.Instance._basicAttack;

    }
}
