using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Ground", menuName = "Ground")]
public class Ground : ScriptableObject
{
    public string Name;
    public GameObject GroundPrefab;
    public int MovementCost;
    public int ObsucreLevel;
    public int CoverLevel;

}
