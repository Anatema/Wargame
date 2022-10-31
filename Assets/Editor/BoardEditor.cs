using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

public class BoardEditor : EditorWindow
{
    private int _gridRadius = 0;
    private Cell _cellPrefab;
    private List<Cell> _cells;
    private HexGrid _hexGrid;

    private Unit _unitPrefab;
    private LayerMask _cellLayer;
    private string[] displayedOptions;
    private Actions _action;
    private enum Actions { AddObjects, RemoveObjects, Nothing }

    private bool _activeChange = false;
    private bool _activeUnit = false;
    private GroundType _activeGroundType;

    private enum EditorActions { Nothing, EditMap, AddUnits}
    private EditorActions _editorRegime;

    private bool _pointerOnCell = false;
    [MenuItem("Window/Map Editor")]
    public static void ShowWindow()
    {
        var window = GetWindow(typeof(BoardEditor));
        window.titleContent.text = "Map Editor";

    }

    public void OnEnable()
    {
        displayedOptions = new string[7];
        for(int i = 0; i < 7; i++)
        {
            displayedOptions[i] = LayerMask.LayerToName(i);
        }
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
        
        _editorRegime = (EditorActions)EditorGUILayout.EnumPopup(_editorRegime);


        if (_editorRegime == EditorActions.EditMap)
        {
            MapCreatorGui();
            MapEditorGui();
            if (GUI.changed)
            {
                _activeUnit = false;
            }
        }
        else if (_editorRegime == EditorActions.AddUnits)
        {
            UnitPlacerGui(); 
            if (GUI.changed)
            {
                _activeChange = false;
            }
        }
        else
        {
            if (GUI.changed)
            {
                _activeUnit = false;
                _activeChange = false;
            }
        }
    }
    private void MapCreatorGui()
    {
        GUILayout.Label("Map creator", EditorStyles.boldLabel);
        _gridRadius = Mathf.Clamp(EditorGUILayout.IntField(new GUIContent("Grid radius"), _gridRadius), 0, 25);
        _cellPrefab = (Cell)EditorGUILayout.ObjectField("Cell prefab", _cellPrefab, typeof(Cell), true);

        if (GUILayout.Button("Create map"))
        {
            CreateGrid();
        }
        if (GUILayout.Button("Clear map"))
        {
            ClearGrid();
        }
    }
    private void MapEditorGui()
    {
        GUILayout.Label("Map editor", EditorStyles.boldLabel);
        //_activeChange = EditorGUILayout.Toggle("Edit map", _activeChange);
        _activeGroundType = (GroundType)EditorGUILayout.EnumPopup(_activeGroundType);

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

    private void UnitPlacerGui()
    {
        GUILayout.Label("Unit placer", EditorStyles.boldLabel);
        //_activeUnit = EditorGUILayout.Toggle("Place unit", _activeUnit);
        _unitPrefab = (Unit)EditorGUILayout.ObjectField("Unit prefab", _unitPrefab, typeof(Unit), true);
        _cellLayer = EditorGUILayout.MaskField("Ground layer", _cellLayer, displayedOptions);
        _action = (Actions)EditorGUILayout.EnumPopup(_action);

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
        SetGround();
        SetUnit();
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
        CellChanger target = null;
        if (Physics.Raycast(ray, out hit))
        {
            //Where did we hit the ground?
            pos = hit.collider.transform.position;
            if (hit.collider.GetComponent<CellChanger>())
            {
                _pointerOnCell = true;
                target = hit.collider.GetComponent<CellChanger>();

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
                Undo.RecordObject(target, target.name);
                target.OnLandChanged((int)_activeGroundType);
                
            }
        }
    }
    private void SetUnit()
    {
        if (_unitPrefab == null)
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

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, _cellLayer))
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

                //MarkSceneAsDirty();
            }
            else if (_action == Actions.RemoveObjects)
            {
                if (target)
                {
                    RemoveUnit(target);
                }

                //MarkSceneAsDirty();
            }
        }
    }

    private void CreateGrid()
    {
        ClearGrid();
        GameObject grid = new GameObject("Board");
        _hexGrid = grid.AddComponent<HexGrid>();
        FindObjectOfType<Battle>().Grid = _hexGrid;
        _cells = new List<Cell>();

        for (int z = -_gridRadius; z <= _gridRadius; z++)
        {
            int r1 = Mathf.Max(-_gridRadius, -z - _gridRadius);
            int r2 = Mathf.Min(_gridRadius, -z + _gridRadius);

            for (int x = r1; x <= r2; x++)
            {
                CreateCell(x, z);
            }
        }
        _hexGrid.SetGrid(_cells);
        SetCellsNeighbours();
    }
    private void ClearGrid()
    {
        ClearAllUnits();
        HexGrid grid = (HexGrid)FindObjectOfType(typeof(HexGrid));
        if (grid)
        {
            DestroyImmediate(grid.gameObject);
        }

        FindObjectOfType<Battle>().Grid = null;
    }
    private void CreateCell(int x, int z)
    {
        Vector3 position = new Vector3();
        position.x = (x * HexMetrics.INNER_RADIUS * 2f + z * HexMetrics.INNER_RADIUS);
        position.z = z * HexMetrics.OUTER_RADIUS * 2f - HexMetrics.OUTER_RADIUS / 2 * z;

        Cell cell = PrefabUtility.InstantiatePrefab(_cellPrefab) as Cell;
        PrefabUtility.UnpackPrefabInstance(cell.gameObject, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
        cell.coordinates = new HexCoordinates(x, z);
        cell.Grid = _hexGrid;
        cell.transform.SetParent(_hexGrid.transform, false);
        cell.transform.localPosition = position;
        _cells.Add(cell);
    }
    private void SetCellsNeighbours()
    {
        foreach (Cell cell in _cells)
        {
            cell.Neighbors = new List<Cell>();
            cell.Neighbors = (from c in _cells
                              where (c.coordinates.X == cell.coordinates.X && Mathf.Abs(c.coordinates.Y - cell.coordinates.Y) == 1) ||
                              (c.coordinates.Y == cell.coordinates.Y && Mathf.Abs(c.coordinates.X - cell.coordinates.X) == 1) ||
                              (c.coordinates.Z == cell.coordinates.Z && Mathf.Abs(c.coordinates.X - cell.coordinates.X) == 1)
                              select c).ToList();
        }
    }


    private void AddUnit(Cell target)
    {
        if (!target)
        {
            return;
        }
        if (target.GroundUnit)
        {
            return;
        }
        Unit unit = PrefabUtility.InstantiatePrefab(_unitPrefab) as Unit;
        PrefabUtility.UnpackPrefabInstance(unit.gameObject, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
        FindObjectOfType<Battle>().AddUnit(unit);

        Undo.RecordObject(target, target.name);
        unit.Instantiate(target);
        EditorUtility.SetDirty(target);

        //unit.transform.parent = transform;
    }
    private void RemoveUnit(Cell targetCell)
    {
        if (!targetCell )
        {
            return;
        }
        if (!targetCell.GroundUnit)
        {
            return;
        }
        Unit unit = targetCell.GroundUnit;
        FindObjectOfType<Battle>().RemoveUnit(unit);
        targetCell.RemoveGroundUnit();
        DestroyImmediate(unit.gameObject);
    }
    private void ClearAllUnits()
    {
        if(!FindObjectOfType<Battle>())
        {
            return;
        }
        Battle battle = FindObjectOfType<Battle>();
        foreach(Unit unit in battle.Units)
        {
            if (unit)
            {
                DestroyImmediate(unit);
            }
        }
    }
}
