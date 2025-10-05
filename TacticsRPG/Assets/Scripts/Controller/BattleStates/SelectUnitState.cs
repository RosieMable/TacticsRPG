using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Unit selection based on turn order system
public class SelectUnitState : BattleState
{
    public override void Enter()
    {
        base.Enter();
        StartCoroutine("ChangeCurrentUnit");
    }
    
    IEnumerator ChangeCurrentUnit()
    {
        // Get next unit from turn order controller
        Unit nextUnit = owner.turnOrderController.GetNextUnit();
        
        if (nextUnit != null)
        {
            turn.Change(nextUnit);
            owner.currentUnit = nextUnit;
            
            // Move camera to current unit
            if (cameraRig != null && nextUnit.tile != null)
            {
                pos = nextUnit.tile.pos;
                cameraRig.follow = nextUnit.transform;
            }
            
            // Update stat panel
            if (statPanelController != null)
            {
                statPanelController.DisplayUnit(nextUnit);
            }
            
            yield return null;
            
            // Check if unit is controlled by AI or player
            if (nextUnit.allegiance == Allegiance.Enemy)
            {
                // TODO: Transition to AI state when AI system is implemented
                owner.ChangeState<CommandSelectionState>();
            }
            else
            {
                owner.ChangeState<CommandSelectionState>();
            }
        }
        else
        {
            // No more units, end battle
            owner.ChangeState<EndBattleState>();
        }
    }
}