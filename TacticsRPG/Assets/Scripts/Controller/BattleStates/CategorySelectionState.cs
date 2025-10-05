using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CategorySelectionState : BaseAbilityMenuState
{
    protected override void LoadMenu()
    {
        if (menuOptions == null)
        {
            menuTitle = "Action";
            menuOptions = new List<string>();
            menuOptions.Add("Attack");
            menuOptions.Add("White Magic");
            menuOptions.Add("Black Magic");
        }

        abilityMenuPanelController.Show(menuTitle, menuOptions);
    }

    protected override void Confirm()
    {
        switch (abilityMenuPanelController.selection)
        {
            case 0: // Attack
                Attack();
                break;
            case 1: // White Magic
                ActionSelectionState.category = 0;
                owner.ChangeState<ActionSelectionState>();
                break;
            case 2: // Black Magic
                ActionSelectionState.category = 1;
                owner.ChangeState<ActionSelectionState>();
                break;
        }
    }

    void Attack()
    {
        owner.turn.hasUnitActed = true;
        if (owner.turn.hasUnitMoved)
            owner.turn.lockMove = true;
        owner.ChangeState<AbilityTargetState>();
    }

    protected override void Cancel()
    {
        owner.ChangeState<CommandSelectionState>();
    }
}
