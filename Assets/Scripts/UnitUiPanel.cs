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
    [SerializeField]
    private PlayerController _playerController;

    private List<AbilityButton> _abilityButtons = new List<AbilityButton>();

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
        _abilityButtons = new List<AbilityButton>();
        foreach (Transform child in _abilitiesContainer)
        {
            Destroy(child.gameObject);
        }

        int count = 0;
        foreach(Ability ability in unit.Abilities)
        {
            int index = count;
            AbilityButton button = Instantiate(_abiltyButtonPrefab, _abilitiesContainer);
            button.SetButton(ability.name, ability.Icon);
            button.Button.onClick.AddListener(() => AbiltyIndexChanged(index));
            _abilityButtons.Add(button);
            count++;
        }
    }

    public void ClearButtons()
    {
        _abilityButtons = new List<AbilityButton>();
        foreach (Transform child in _abilitiesContainer)
        {
            Destroy(child.gameObject);
        }
    }
    public void AbiltyIndexChanged(int index)
    {
        _playerController.ChangeSelectedAbility(index);
        foreach(AbilityButton button in _abilityButtons)
        {
            button.SetUnactive();
        }
        _abilityButtons[index].SetActive();
    }


}
