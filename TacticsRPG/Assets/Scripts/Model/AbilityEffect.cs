using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Base class for ability effects
public abstract class AbilityEffect : ScriptableObject
{
    public abstract void Apply(GameObject target);
}

[CreateAssetMenu(menuName = "RPG/Ability Effect/Damage")]
public class DamageAbilityEffect : AbilityEffect
{
    public int amount = 10;
    public StatType attackStat = StatType.ATK;
    public StatType defenseStat = StatType.DEF;

    public override void Apply(GameObject target)
    {
        Stats targetStats = target.GetComponent<Stats>();
        if (targetStats == null) return;

        int damage = amount;
        
        // Simple damage calculation
        // In a real game, you'd have the attacker's stats too
        int defense = targetStats.GetStatValue(defenseStat);
        damage = Mathf.Max(1, damage - defense);

        // Apply damage to HP
        Stat hp = targetStats[StatType.HP];
        if (hp != null)
        {
            int newHP = Mathf.Max(0, hp.value - damage);
            hp.SetBaseValue(newHP);
            
            // Check for KO
            if (newHP <= 0)
            {
                // Add KO status effect here
                Debug.Log($"{target.name} has been knocked out!");
            }
        }
    }
}

[CreateAssetMenu(menuName = "RPG/Ability Effect/Heal")]
public class HealAbilityEffect : AbilityEffect
{
    public int amount = 10;

    public override void Apply(GameObject target)
    {
        Stats targetStats = target.GetComponent<Stats>();
        if (targetStats == null) return;

        Stat hp = targetStats[StatType.HP];
        if (hp != null)
        {
            int newHP = Mathf.Min(hp.maxValue, hp.value + amount);
            hp.SetBaseValue(newHP);
        }
    }
}

[CreateAssetMenu(menuName = "RPG/Ability Effect/Restore MP")]
public class RestoreMPAbilityEffect : AbilityEffect
{
    public int amount = 5;

    public override void Apply(GameObject target)
    {
        Stats targetStats = target.GetComponent<Stats>();
        if (targetStats == null) return;

        Stat mp = targetStats[StatType.MP];
        if (mp != null)
        {
            int newMP = Mathf.Min(mp.maxValue, mp.value + amount);
            mp.SetBaseValue(newMP);
        }
    }
}