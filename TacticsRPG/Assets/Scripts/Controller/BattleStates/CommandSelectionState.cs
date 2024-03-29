using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Display menu at beggining of a turn
// Can choose to move, take an Action or Wait
// Wait ends the turn immediately
// Move and Wait have additional states or sub menus
// If move or take an action, then return to this state and have relevant option locked
public class CommandSelectionState : BaseAbilityMenuState
{

    protected override void LoadMenu()
    {
        if (menuOptions == null)
        {
            menuTitle = "Commands";
            menuOptions = new List<string>(3);
            menuOptions.Add("Move");
            menuOptions.Add("Action");
            menuOptions.Add("Wait");
        }

        abilityMenuPanelController.Show(menuTitle, menuOptions);
        abilityMenuPanelController.SetLocked(0, turn.hasUnitMoved);
        abilityMenuPanelController.SetLocked(1, turn.hasUnitActed);
    }

    protected override void Confirm()
    {
        switch (abilityMenuPanelController.selection)
        {
            case 0: //Move
                owner.ChangeState<MoveTargetState>();
                break;
            case 1: //Action
                owner.ChangeState<CategorySelectionState>();
                break;
            case 2: //Wait
                owner.ChangeState<SelectUnitState>();
                break;
        }
    }

    protected override void Cancel()
    {
        if (turn.hasUnitMoved && !turn.lockMove)
        {
            turn.UndoMove();
            abilityMenuPanelController.SetLocked(0, false);
            SelectTile(turn.actor.tile.pos);
        }
        else
        {
            owner.ChangeState<ExploreState>();
        }
    }

}