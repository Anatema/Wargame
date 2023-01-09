using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class MapEditor : EditorWindow
{
    private Ground[] _groundVariants;

    private string[] _displayedOptions;
    private int _activeGroundType = 0;

    private bool _activeChange = false;
    private bool _pointerOnCell = false;
    [MenuItem("Window/Map Editor")]
    public static void OpenWindow()
    {
        var window = GetWindow(typeof(MapEditor));
        window.titleContent.text = "Map Editor";

    }

    private void OnEnable()
    {
        _groundVariants = Resources.LoadAll<Ground>("GroundTypes");
        _displayedOptions = (from groundType in _groundVariants
                             select groundType.Name).ToArray();
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
        GUILayout.Label("Map editor", EditorStyles.boldLabel);

        _activeGroundType = EditorGUILayout.Popup("Ground Type", _activeGroundType, _displayedOptions);

        GUI.enabled = !_activeChange;
        if (GUILayout.Button("Start editing"))
        {
            _activeChange = true;
        }
        GUI.enabled = true;

        GUI.enabled = _activeChange;
        if (GUILayout.Button("End editing"))
        {
            _activeChange = false;
        }
        GUI.enabled = true;
    }
    private void OnSceneGUI(SceneView sceneView)
    {
        SetGround();
    }
    private void SetGround()
    {
        if (!_activeChange)
        {
            return;
        }
        _pointerOnCell = false;
        //Move the circle when moving the mouse
        //A ray from the mouse position
        Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);

        RaycastHit hit;
        Vector3 pos = Vector3.zero;
        Cell target = null;

        if (Physics.Raycast(ray, out hit))
        {
            //Where did we hit the ground?
            pos = hit.collider.transform.position;
            if (hit.collider.GetComponent<Cell>())
            {
                _pointerOnCell = true;
                target = hit.collider.GetComponent<Cell>();

            }

            //Need to tell Unity that we have moved the circle or the circle may be displayed at the old position
            SceneView.RepaintAll();
        }



        //Display the circle
        if (_pointerOnCell)
        {
            Handles.color = Color.white;
            Handles.DrawWireDisc(pos, Vector3.up, 5);
        }

        //Add or remove objects with left mouse click

        //First make sure we cant select another gameobject in the scene when we click
        HandleUtility.AddDefaultControl(0);

        //Have we clicked with the left mouse button?
        if ((Event.current.type == EventType.MouseDrag || Event.current.type == EventType.MouseDown) && Event.current.button == 0)
        {
            if (target)
            {
               // Undo.RecordObject(target, target.name);
                CheckGroLevelEditor().ChangeHexGround(_groundVariants[_activeGroundType], target);
                //Undo.RecordObject(target, target.name);
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
}
