using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MapGenerator))]
public class MapGeneratorWindow : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        MapGenerator generator = (MapGenerator)target;
        if(GUILayout.Button("Create grid"))
        {
            generator.GenerateGrid();
        }

        if (GUILayout.Button("Clear grid"))
        {
            generator.ClearGrid();
        }
    }
}
