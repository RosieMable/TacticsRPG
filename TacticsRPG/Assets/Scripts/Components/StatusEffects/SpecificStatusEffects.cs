using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Poison status effect - damages unit each turn
public class PoisonStatusEffect : StatusEffect
{
    public int damagePerTurn = 5;

    protected override void ApplyEffect()
    {
        type = StatusType.Poison;
        if (duration == -1) duration = 3; // Default 3 turns
    }

    public override void OnTurnEnd()
    {
        base.OnTurnEnd();
        
        // Apply poison damage
        if (_stats.Contains(StatType.HP))
        {
            Stat hp = _stats[StatType.HP];
            int newHP = Mathf.Max(0, hp.value - damagePerTurn);
            hp.SetBaseValue(newHP);
            
            Debug.Log($"{gameObject.name} takes {damagePerTurn} poison damage!");
            
            if (newHP <= 0)
            {
                // Add KO status
                gameObject.AddComponent<KOStatusEffect>();
            }
        }
    }
}

// Haste status effect - increases speed
public class HasteStatusEffect : StatusEffect
{
    public int speedBonus = 2;

    protected override void ApplyEffect()
    {
        type = StatusType.Haste;
        if (duration == -1) duration = 5; // Default 5 turns
        
        AddStatModifier(StatType.SPD, speedBonus);
    }
}

// Slow status effect - decreases speed
public class SlowStatusEffect : StatusEffect
{
    public int speedPenalty = -2;

    protected override void ApplyEffect()
    {
        type = StatusType.Slow;
        if (duration == -1) duration = 5; // Default 5 turns
        
        AddStatModifier(StatType.SPD, speedPenalty);
    }
}

// Stop status effect - prevents unit from acting
public class StopStatusEffect : StatusEffect
{
    protected override void ApplyEffect()
    {
        type = StatusType.Stop;
        if (duration == -1) duration = 3; // Default 3 turns
        
        // Set speed to 0 so unit can't act
        AddStatModifier(StatType.SPD, -999); // Large penalty to effectively stop
    }
}

// KO status effect - unit is knocked out
public class KOStatusEffect : StatusEffect
{
    protected override void ApplyEffect()
    {
        type = StatusType.KO;
        duration = -1; // Permanent until revived
        removeOnBattleEnd = false;
        
        // Set HP to 0 and disable the unit
        if (_stats.Contains(StatType.HP))
        {
            _stats[StatType.HP].SetBaseValue(0);
        }
        
        // Disable unit visually
        var renderers = GetComponentsInChildren<Renderer>();
        foreach (var renderer in renderers)
        {
            var color = renderer.material.color;
            color.a = 0.5f; // Make semi-transparent
            renderer.material.color = color;
        }
    }
}

// Regen status effect - restores HP each turn
public class RegenStatusEffect : StatusEffect
{
    public int healPerTurn = 3;

    protected override void ApplyEffect()
    {
        type = StatusType.Regen;
        if (duration == -1) duration = 5; // Default 5 turns
    }

    public override void OnTurnEnd()
    {
        base.OnTurnEnd();
        
        // Apply regen healing
        if (_stats.Contains(StatType.HP))
        {
            Stat hp = _stats[StatType.HP];
            int newHP = Mathf.Min(hp.maxValue, hp.value + healPerTurn);
            hp.SetBaseValue(newHP);
            
            Debug.Log($"{gameObject.name} recovers {healPerTurn} HP from regen!");
        }
    }
}