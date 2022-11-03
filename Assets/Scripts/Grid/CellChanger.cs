using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellChanger : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> _landPrefabs;
    public void OnLandChanged(int index)
    {
        if(transform.childCount > 0)
        {
            foreach(Transform child in transform)
            {
                DestroyImmediate(child.gameObject);
            }
        }
        if(index == 0)
        {
            GetComponent<Cell>().movementCost = 4;
        }
        else if (index-1 < _landPrefabs.Count)
        {
            Instantiate(_landPrefabs[index-1], transform);
            GetComponent<Cell>().movementCost = index * 2;

        }
    }

}
public enum GroundType { None, Road, Field, Forest, Swamp, Mountan}
