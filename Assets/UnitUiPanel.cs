using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitUiPanel : MonoBehaviour
{
    [SerializeField]
    private Text _name;
    [SerializeField]
    private Text _health;
    [SerializeField]
    private Text _size;

    [SerializeField]
    private Transform _abilitiesContainer;
    [SerializeField]
    private AbilityButton _abiltyButtonPrefab;
    public void RefreshUnitUI(Unit unit)
    {
        _name.text = unit.name;
        _health.text = $"{unit.CurrentHealth.ToString()} / {unit.MaxHealth.ToString()}";
        _size.text = $"{unit.CurrentUnitSize.ToString()} / {unit.MaxUnitSize.ToString()}";
        SetAbilites(unit);
    }
    public void ShowPanel()
    {

    }
    public void HidePanel()
    {

    }

    public void SetAbilites(Unit unit)
    {
        foreach(Transform child in _abilitiesContainer)
        {
            Destroy(child.gameObject);
        }

        foreach(Ability ability in unit.Abilities)
        {
            Instantiate(_abiltyButtonPrefab, _abilitiesContainer).Text.text = ability.name;
        }
    }

}
