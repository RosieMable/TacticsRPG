using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Feature that modifies stats
public class StatModifierFeature : Feature
{
    public StatType targetStat;
    public int amount;
    public StatModType modifierType;

    StatModifier modifier;

    public override void Apply(GameObject target)
    {
        Stats stats = target.GetComponent<Stats>();
        if (stats != null && stats.Contains(targetStat))
        {
            modifier = new StatModifier(amount, modifierType, this);
            stats[targetStat].AddModifier(modifier);
        }
    }

    public override void Remove(GameObject target)
    {
        if (modifier != null)
        {
            Stats stats = target.GetComponent<Stats>();
            if (stats != null && stats.Contains(targetStat))
            {
                stats[targetStat].RemoveModifier(modifier);
            }
            modifier = null;
        }
    }
}