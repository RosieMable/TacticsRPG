using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffect : MonoBehaviour
{
    public StatusType type;
    public int duration = -1; // -1 means permanent
    public bool removeOnBattleEnd = true;
    
    protected Stats _stats;
    List<StatModifier> _modifiers = new List<StatModifier>();

    protected virtual void Awake()
    {
        _stats = GetComponent<Stats>();
    }

    protected virtual void Start()
    {
        ApplyEffect();
    }

    protected virtual void OnDestroy()
    {
        RemoveEffect();
    }

    protected virtual void ApplyEffect()
    {
        // Override in derived classes
    }

    protected virtual void RemoveEffect()
    {
        // Remove all stat modifiers
        foreach (var modifier in _modifiers)
        {
            // Find which stat has this modifier and remove it
            foreach (StatType statType in System.Enum.GetValues(typeof(StatType)))
            {
                if (_stats.Contains(statType))
                {
                    _stats[statType].RemoveModifier(modifier);
                }
            }
        }
        _modifiers.Clear();
    }

    protected void AddStatModifier(StatType statType, int value, StatModType modType = StatModType.Flat)
    {
        if (_stats.Contains(statType))
        {
            var modifier = new StatModifier(value, modType, this);
            _stats[statType].AddModifier(modifier);
            _modifiers.Add(modifier);
        }
    }

    public virtual void OnTurnStart()
    {
        if (duration > 0)
        {
            duration--;
            if (duration <= 0)
            {
                Destroy(this);
            }
        }
    }

    public virtual void OnTurnEnd()
    {
        // Override for effects that trigger at turn end
    }
}

public enum StatusType
{
    Poison,
    Haste,
    Slow,
    Stop,
    Protect,
    Shell,
    Regen,
    KO
}