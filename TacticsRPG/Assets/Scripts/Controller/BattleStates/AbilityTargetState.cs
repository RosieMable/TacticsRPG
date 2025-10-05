using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityTargetState : BattleState
{
    List<Tile> tiles;
    Ability selectedAbility;

    public override void Enter()
    {
        base.Enter();
        
        // For now, use a default attack ability
        // In a complete implementation, this would be passed from the previous state
        selectedAbility = CreateDefaultAttackAbility();
        
        if (selectedAbility != null)
        {
            tiles = selectedAbility.GetTilesInRange(board, pos);
            board.SelectTiles(tiles);
        }
    }

    public override void Exit()
    {
        base.Exit();
        if (tiles != null)
        {
            board.DeSelectTiles(tiles);
            tiles = null;
        }
    }

    protected override void OnMove(object sender, InfoEventArgs<Point> e)
    {
        SelectTile(e.info + pos);
    }

    protected override void OnFire(object sender, InfoEventArgs<int> e)
    {
        if (e.info == 0) // Confirm
        {
            if (tiles != null && tiles.Contains(owner.currentTile))
            {
                // Execute ability
                ExecuteAbility();
                owner.ChangeState<SelectUnitState>();
            }
        }
        else if (e.info == 1) // Cancel
        {
            owner.ChangeState<CategorySelectionState>();
        }
    }

    void ExecuteAbility()
    {
        if (selectedAbility == null) return;

        // Get targets in area of effect
        List<Tile> targetTiles = selectedAbility.GetTilesInArea(board, pos);
        
        foreach (Tile tile in targetTiles)
        {
            if (tile.content != null)
            {
                Unit targetUnit = tile.content.GetComponent<Unit>();
                if (targetUnit != null)
                {
                    // Apply ability effects
                    foreach (AbilityEffect effect in selectedAbility.effects)
                    {
                        effect.Apply(targetUnit.gameObject);
                    }
                }
            }
        }
        
        // Mark that unit has acted
        turn.hasUnitActed = true;
    }

    Ability CreateDefaultAttackAbility()
    {
        // Create a simple attack ability
        // In a real implementation, this would come from unit's available abilities
        Ability attack = ScriptableObject.CreateInstance<Ability>();
        attack.abilityName = "Attack";
        attack.mpCost = 0;
        attack.category = AbilityCategory.Attack;
        
        // Set range (adjacent tiles)
        attack.range = ScriptableObject.CreateInstance<SpecifyAbilityRange>();
        ((SpecifyAbilityRange)attack.range).horizontal = 1;
        ((SpecifyAbilityRange)attack.range).vertical = 1;
        
        // Set area of effect (single target)
        attack.areaOfEffect = ScriptableObject.CreateInstance<SelfAbilityAreaOfEffect>();
        
        // Set damage effect
        DamageAbilityEffect damageEffect = ScriptableObject.CreateInstance<DamageAbilityEffect>();
        damageEffect.amount = 10;
        attack.effects.Add(damageEffect);
        
        return attack;
    }
}