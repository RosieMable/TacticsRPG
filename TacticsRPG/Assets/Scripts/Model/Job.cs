using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "RPG/Job")]
public partial class Job : ScriptableObject
{
    public string jobName;
    public string description;
    public Sprite icon;
    
    [Header("Stat Growth Rates (0-100%)")]
    public int hpGrowth = 50;
    public int mpGrowth = 50;
    public int atkGrowth = 50;
    public int defGrowth = 50;
    public int intGrowth = 50;
    public int resGrowth = 50;
    public int spdGrowth = 50;
    public int movGrowth = 10;
    public int jmpGrowth = 10;

    [Header("Base Stats")]
    public int baseHP = 20;
    public int baseMP = 10;
    public int baseATK = 5;
    public int baseDEF = 5;
    public int baseINT = 5;
    public int baseRES = 5;
    public int baseSPD = 5;
    public int baseMOV = 3;
    public int baseJMP = 1;

    [Header("Available Abilities")]
    public List<Ability> abilities = new List<Ability>();

    public void ApplyToUnit(GameObject unit)
    {
        Stats stats = unit.GetComponent<Stats>();
        if (stats == null) return;

        // Set base stats for this job
        stats.SetBaseValue(StatType.HP, baseHP);
        stats.SetBaseValue(StatType.MP, baseMP);
        stats.SetBaseValue(StatType.ATK, baseATK);
        stats.SetBaseValue(StatType.DEF, baseDEF);
        stats.SetBaseValue(StatType.INT, baseINT);
        stats.SetBaseValue(StatType.RES, baseRES);
        stats.SetBaseValue(StatType.SPD, baseSPD);
        stats.SetBaseValue(StatType.MOV, baseMOV);
        stats.SetBaseValue(StatType.JMP, baseJMP);
    }

    public bool CanGrowStat(StatType statType)
    {
        switch (statType)
        {
            case StatType.HP: return Random.Range(0, 100) < hpGrowth;
            case StatType.MP: return Random.Range(0, 100) < mpGrowth;
            case StatType.ATK: return Random.Range(0, 100) < atkGrowth;
            case StatType.DEF: return Random.Range(0, 100) < defGrowth;
            case StatType.INT: return Random.Range(0, 100) < intGrowth;
            case StatType.RES: return Random.Range(0, 100) < resGrowth;
            case StatType.SPD: return Random.Range(0, 100) < spdGrowth;
            case StatType.MOV: return Random.Range(0, 100) < movGrowth;
            case StatType.JMP: return Random.Range(0, 100) < jmpGrowth;
            default: return false;
        }
    }
}