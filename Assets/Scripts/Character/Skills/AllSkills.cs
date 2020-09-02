using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "All Skills", menuName = "Skills/All Skills (singleton)")]
public class AllSkills : SingletonScriptableObject<AllSkills>
{
    public Skill _basicAttack;
    public Skill _charge;
    public Skill _repel;
}
