using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public Battle Battle;

    [SerializeField]
    private PlayerState _playerState;
    private Waiting _waiting;
    private Selecting _selecting;
    private Acting _acting;

    public Waiting Waiting => _waiting;
    public Acting Acting => _acting;
    public Selecting Selecting => _selecting;

    [SerializeField]
    private Text _currentState;

    public DataPanel DataPanel;
    public Unit SelectedUnit;

    [SerializeField]
    public LineRenderer LinePrefab;

    public void Awake()
    {
        _waiting = new Waiting(this);
        _selecting = new Selecting(this);
        _acting = new Acting(this);

        _playerState = Waiting;
        _currentState.text = Waiting.StateName;
    }

    public void Update()
    {
        _playerState.Update();
    }

    public void SetAsSelecting()
    {
        SelectState(Selecting);
    }
    public void SelectState(PlayerState state)
    {
        _playerState.EndState();
        _playerState = state;
        _currentState.text = state.StateName;
        _playerState.EnterState();
    }
    
}
