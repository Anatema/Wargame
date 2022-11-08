using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battle : MonoBehaviour
{
    public HexGrid Grid;
    [SerializeField]
    public List<Unit> Units;
    public PlayerController PlayerController;

    public int CurrentPlayer = 0;
    public int NumberOfPlayers = 2;
    public void AddUnit(Unit unit)
    {
        if (Units == null)
        {
            Units = new List<Unit>();
        }
        unit.transform.SetParent(transform);
        Units.Add(unit);
    }
    public void RemoveUnit(Unit unit)
    {
        Units.Remove(unit);
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
}
