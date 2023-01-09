using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System;

public class UnitEditor : EditorWindow
{
    private UnitData _unitData;
    private Actions _action;
    private int _unitSide;
    private enum Actions { AddObjects, RemoveObjects, Nothing }

    private bool _activeUnit = false;

    
    [MenuItem("Window/Unit Editor")]
    public static void ShowWindow()
    {
        var window = GetWindow(typeof(UnitEditor));
        window.titleContent.text = "Unit Editor";

    }

    public void OnEnable()
    {
        _unitData = Resources.Load<UnitData>("Units/Warior");
    }

    private void OnFocus()
    {
        // Remove delegate listener if it has previously
        // been assigned.
        SceneView.duringSceneGui -= this.OnSceneGUI;
        // Add (or re-add) the delegate.
        SceneView.duringSceneGui += this.OnSceneGUI;
    }

    void OnDestroy()
    {
        // When the window is destroyed, remove the delegate
        // so that it will no longer do any drawing.
        SceneView.duringSceneGui -= this.OnSceneGUI;
    }
   
    private void OnGUI()
    {
        GUILayout.Label("Unit placer", EditorStyles.boldLabel);
        //_activeUnit = EditorGUILayout.Toggle("Place unit", _activeUnit);
        _unitData = (UnitData)EditorGUILayout.ObjectField("Unit", _unitData, typeof(UnitData), true);
        //_cellLayer = EditorGUILayout.MaskField("Ground layer", _cellLayer, displayedOptions);
        _action = (Actions)EditorGUILayout.EnumPopup(_action);
        _unitSide = EditorGUILayout.IntField("Player index:", _unitSide);

        GUI.enabled = !_activeUnit;
        if (GUILayout.Button("Start editing"))
        {
            _activeUnit = true;
        }
        GUI.enabled = true;

        GUI.enabled = _activeUnit;
        if (GUILayout.Button("End editing"))
        {
            _activeUnit = false;
        }
        GUI.enabled = true;
    }
    private void OnSceneGUI(SceneView sceneView)
    {
        SetUnit();
    }
    private void SetUnit()
    {
        if (_unitData == null)
        {
            return;
        }
        if (!_activeUnit)
        {
            return;
        }
        //Move the circle when moving the mouse
        //A ray from the mouse position
        Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);

        RaycastHit hit;
        Vector3 pos = Vector3.zero;
        Cell target = null;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, 64))
        {
            if (hit.collider.GetComponent<Cell>())
            {
                pos = hit.collider.transform.position;
                target = hit.collider.GetComponent<Cell>();
                //Need to tell Unity that we have moved the circle or the circle may be displayed at the old position
                SceneView.RepaintAll();

            }
        }



        //Display the circle
        Handles.color = Color.white;
        Handles.DrawWireDisc(pos, Vector3.up, 5);


        //Add or remove objects with left mouse click

        //First make sure we cant select another gameobject in the scene when we click
        HandleUtility.AddDefaultControl(0);


        //Have we clicked with the left mouse button?
        if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
        {
            //Should we add or remove objects?
            if (_action == Actions.AddObjects)
            {
                AddUnit(target);
            }
            else if (_action == Actions.RemoveObjects)
            {
                if (target)
                {
                    RemoveUnit(target);
                }
            }
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

    private void AddUnit(Cell target)
    {
        CheckGroLevelEditor().AddUnit(_unitData, target, _unitSide);
    }
    private void RemoveUnit(Cell targetCell)
    {
        CheckGroLevelEditor().RemoveUnit(targetCell);

    }
}
