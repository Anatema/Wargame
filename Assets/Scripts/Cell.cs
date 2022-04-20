using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    public HexCoordinates coordinates;    
    public Text label;

    public List<Cell> Neighbors;
    public Color OldColor;
    public void Awake()
    {
        OldColor = GetComponent<MeshRenderer>().material.color;
    }
    public void Higlight()
    {
        GetComponent<MeshRenderer>().material.color = Color.red;
        StartCoroutine(GetColorBack());
    }
    public IEnumerator GetColorBack()
    {
        yield return new WaitForSeconds(0.2f);
        GetComponent<MeshRenderer>().material.color = OldColor;
    }

}
