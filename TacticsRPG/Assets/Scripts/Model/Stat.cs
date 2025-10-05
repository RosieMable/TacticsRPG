using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Stat 
{
    public event EventHandler<StatChangedEventArgs> changed;

    [SerializeField] StatType _type;
    [SerializeField] int _baseValue;
    [SerializeField] int _minValue = int.MinValue;
    [SerializeField] int _maxValue = int.MaxValue;

    List<StatModifier> _modifiers = new List<StatModifier>();

    public StatType type { get { return _type; } }
    public int baseValue { get { return _baseValue; } }
    public int minValue { get { return _minValue; } }
    public int maxValue { get { return _maxValue; } }

    public int value
    {
        get
        {
            int finalValue = _baseValue;
            
            // Apply modifiers
            for (int i = 0; i < _modifiers.Count; ++i)
            {
                finalValue += _modifiers[i].value;
            }

            // Clamp to min/max
            return Mathf.Clamp(finalValue, _minValue, _maxValue);
        }
    }

    public Stat(StatType type, int baseValue)
    {
        _type = type;
        _baseValue = baseValue;
    }

    public void AddModifier(StatModifier modifier)
    {
        _modifiers.Add(modifier);
        NotifyChanged();
    }

    public bool RemoveModifier(StatModifier modifier)
    {
        bool removed = _modifiers.Remove(modifier);
        if (removed)
            NotifyChanged();
        return removed;
    }

    public void SetBaseValue(int value)
    {
        int oldValue = this.value;
        _baseValue = value;
        
        if (oldValue != this.value)
            NotifyChanged();
    }

    void NotifyChanged()
    {
        if (changed != null)
            changed(this, new StatChangedEventArgs(this));
    }
}

[Serializable]
public class StatModifier
{
    public int value;
    public StatModType type;
    public object source; // What's causing this modifier (equipment, spell, etc.)

    public StatModifier(int value, StatModType type, object source = null)
    {
        this.value = value;
        this.type = type;
        this.source = source;
    }
}

public class StatChangedEventArgs : EventArgs
{
    public Stat stat;

    public StatChangedEventArgs(Stat stat)
    {
        this.stat = stat;
    }
}