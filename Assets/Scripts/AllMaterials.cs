using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "All Materials", menuName = "Materials/All Materials (singleton)")]
public class AllMaterials : SingletonScriptableObject<AllMaterials>
{
    //Outlines 
    public Material _outlineNormal = null;
    public Material _outlineSelected = null;
    public Material _outlineAllied = null;
    public Material _outlineEnemy = null;
}
