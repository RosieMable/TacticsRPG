using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    [SerializeField] List<Stat> _stats = new List<Stat>();
    Dictionary<StatType, Stat> _statDict = new Dictionary<StatType, Stat>();

    void Awake()
    {
        // Initialize the dictionary for faster lookups
        for (int i = 0; i < _stats.Count; ++i)
        {
            _statDict[_stats[i].type] = _stats[i];
        }
    }

    public Stat this[StatType type]
    {
        get
        {
            if (_statDict.ContainsKey(type))
                return _statDict[type];
            return null;
        }
    }

    public bool Contains(StatType type)
    {
        return _statDict.ContainsKey(type);
    }

    public void AddStat(StatType type, int baseValue)
    {
        var stat = new Stat(type, baseValue);
        _stats.Add(stat);
        _statDict[type] = stat;
    }

    // Helper methods for common operations
    public int GetStatValue(StatType type)
    {
        Stat stat = this[type];
        return stat != null ? stat.value : 0;
    }

    public void SetBaseValue(StatType type, int value)
    {
        Stat stat = this[type];
        if (stat != null)
            stat.SetBaseValue(value);
    }

    // Experience and leveling system
    public void AddExperience(int amount)
    {
        Stat exp = this[StatType.EXP];
        Stat level = this[StatType.Level];
        
        if (exp != null && level != null)
        {
            int currentExp = exp.value;
            int currentLevel = level.value;
            
            exp.SetBaseValue(currentExp + amount);
            
            // Check for level up (simple formula: 100 * level EXP needed)
            int expNeededForNextLevel = 100 * (currentLevel + 1);
            if (exp.value >= expNeededForNextLevel)
            {
                LevelUp();
            }
        }
    }

    void LevelUp()
    {
        Stat level = this[StatType.Level];
        if (level != null)
        {
            level.SetBaseValue(level.value + 1);
            
            // Increase stats on level up (basic implementation)
            IncreaseRandomStats();
        }
    }

    void IncreaseRandomStats()
    {
        // Simple stat growth - in a real game you'd have job-based growth rates
        StatType[] growthStats = { StatType.HP, StatType.MP, StatType.ATK, StatType.DEF, StatType.INT, StatType.RES, StatType.SPD };
        
        foreach (StatType statType in growthStats)
        {
            Stat stat = this[statType];
            if (stat != null && UnityEngine.Random.Range(0f, 1f) < 0.5f) // 50% chance to grow
            {
                int growth = UnityEngine.Random.Range(1, 4); // 1-3 points growth
                stat.SetBaseValue(stat.baseValue + growth);
            }
        }
    }
}