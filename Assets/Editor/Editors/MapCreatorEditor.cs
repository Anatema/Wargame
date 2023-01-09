using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MapCreatorEditor : EditorWindow
{
    private int _gridRadius;
    private Cell _cellPrefab;
    [MenuItem("Window/Map Creator")]
    public static void OpenWindow()
    {
        var window = GetWindow(typeof(MapCreatorEditor));
        window.titleContent.text = "Map Creator";

    }

    private void OnEnable()
    {
        //set cell prefab so there is no need to do it manualy
        _cellPrefab = Resources.Load<Cell>("Prefabs/Hex");
    }
    private void OnGUI()
    {
        GUILayout.Label("Map creator", EditorStyles.boldLabel);
        _gridRadius = Mathf.Clamp(EditorGUILayout.IntField(new GUIContent("Grid radius"), _gridRadius), 0, 25);
        _cellPrefab = (Cell)EditorGUILayout.ObjectField("Cell prefab", _cellPrefab, typeof(Cell), true);

        if (GUILayout.Button("Create map"))
        {
            CreateGrid();
        }
        if (GUILayout.Button("Delete map"))
        {
            DeleteGrid();
        }
    }
    private LevelManager CheckGroLevelEditor()
    {
        LevelManager level = FindObjectOfType<LevelManager>();
        if (!level)
        {
            GameObject obj = new GameObject("Level");
            level = obj.AddComponent<LevelManager>();
        }
        return level;
    }
    private void CreateGrid()
    {
        LevelManager level = CheckGroLevelEditor();
        level.CreateGrid(_cellPrefab, _gridRadius);
    }
    private void DeleteGrid()
    {
        LevelManager level = CheckGroLevelEditor();
        level.DeleteGrid();
    }
}
