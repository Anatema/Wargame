using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battle : MonoBehaviour
{
    [SerializeField]
    public List<Unit> Units;
    public PlayerController PlayerController;

    public int CurrentPlayer = 0;
    public int NumberOfPlayers = 2;
    public void AddUnit(UnitData unitData, Cell cell, int unitSide)
    {
        Unit unit = CreateUnit(unitData);
        unit.Instantiate(cell, unitData);
        unit.PlayerIndex = Convert.ToByte(unitSide);

        if (Units == null)
        {
            Units = new List<Unit>();
        }
        unit.transform.SetParent(transform);
        Units.Add(unit);
    }
    private Unit CreateUnit(UnitData data)
    {
        GameObject obj = Instantiate(data.Model);
        Unit unit = obj.AddComponent<Unit>();

        //Unit unit = PrefabUtility.InstantiatePrefab(_unitData) as Unit;
        //PrefabUtility.UnpackPrefabInstance(unit.gameObject, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
        //FindObjectOfType<Battle>().AddUnit(unit);

        //Undo.RecordObject(target, target.name);
        //unit.Instantiate(target, _unitData);
        //EditorUtility.SetDirty(target);
        //unit.transform.parent = transform;
        return unit;
    }
    public void RemoveUnit(Unit unit)
    {
        unit.Cell.RemoveGroundUnit();
        if (Application.isPlaying)
        {
            Destroy(unit.gameObject);
        }
        else
        {
            DestroyImmediate(unit.gameObject);
        }
        Units.Remove(unit);
    }
    public void RemovaAllUnits()
    {
        foreach(Unit unit in Units)
        {
            unit.Cell.RemoveGroundUnit();
            if (Application.isPlaying)
            {
                Destroy(unit.gameObject);
            }
            else
            {
                DestroyImmediate(unit.gameObject);
            }
        }
        Units = new List<Unit>();
    }

    public void Start()
    { 
        SetPlayerTurn(0);
    }
    private void SetPlayerTurn(int i)
    {
        CurrentPlayer = i;
        foreach (Unit unit in Units)
        {
            if(unit.PlayerIndex == CurrentPlayer)
            {
                unit.RecoverUnit();
            }
        }
        PlayerController.SetAsSelecting();

    }
    public void NextTurn()
    {
        CurrentPlayer++;
        if(CurrentPlayer >= NumberOfPlayers)
        {
            CurrentPlayer = 0;
        }
        SetPlayerTurn(CurrentPlayer);
    }

    public void ClearAllUnits()
    {

    }
}
