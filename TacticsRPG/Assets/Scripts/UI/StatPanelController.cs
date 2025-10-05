using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatPanelController : MonoBehaviour
{
    [Header("UI References")]
    public Text unitNameText;
    public Text levelText;
    public Text hpText;
    public Text mpText;
    public Text expText;
    public Image avatarImage;
    public GameObject panel;

    Unit currentUnit;
    Stats currentStats;

    void Start()
    {
        if (panel != null)
            panel.SetActive(false);
    }

    public void DisplayUnit(Unit unit)
    {
        currentUnit = unit;
        currentStats = unit != null ? unit.GetComponent<Stats>() : null;
        
        if (panel != null)
            panel.SetActive(unit != null);

        if (unit != null && currentStats != null)
        {
            UpdateDisplay();
            
            // Subscribe to stat changes
            if (currentStats.Contains(StatType.HP))
                currentStats[StatType.HP].changed += OnStatChanged;
            if (currentStats.Contains(StatType.MP))
                currentStats[StatType.MP].changed += OnStatChanged;
            if (currentStats.Contains(StatType.EXP))
                currentStats[StatType.EXP].changed += OnStatChanged;
        }
    }

    public void Hide()
    {
        if (currentStats != null)
        {
            // Unsubscribe from stat changes
            if (currentStats.Contains(StatType.HP))
                currentStats[StatType.HP].changed -= OnStatChanged;
            if (currentStats.Contains(StatType.MP))
                currentStats[StatType.MP].changed -= OnStatChanged;
            if (currentStats.Contains(StatType.EXP))
                currentStats[StatType.EXP].changed -= OnStatChanged;
        }

        currentUnit = null;
        currentStats = null;
        
        if (panel != null)
            panel.SetActive(false);
    }

    void UpdateDisplay()
    {
        if (currentUnit == null || currentStats == null) return;

        // Update unit name
        if (unitNameText != null)
            unitNameText.text = currentUnit.name;

        // Update level
        if (levelText != null && currentStats.Contains(StatType.Level))
            levelText.text = "LV " + currentStats.GetStatValue(StatType.Level);

        // Update HP
        if (hpText != null && currentStats.Contains(StatType.HP))
        {
            Stat hp = currentStats[StatType.HP];
            hpText.text = $"HP: {hp.value}/{hp.maxValue}";
        }

        // Update MP
        if (mpText != null && currentStats.Contains(StatType.MP))
        {
            Stat mp = currentStats[StatType.MP];
            mpText.text = $"MP: {mp.value}/{mp.maxValue}";
        }

        // Update EXP
        if (expText != null && currentStats.Contains(StatType.EXP))
        {
            int exp = currentStats.GetStatValue(StatType.EXP);
            int level = currentStats.GetStatValue(StatType.Level);
            int expNeeded = 100 * (level + 1); // Simple formula
            expText.text = $"EXP: {exp}/{expNeeded}";
        }

        // Update avatar (if you have sprites for different units)
        if (avatarImage != null)
        {
            // Set avatar sprite based on unit type or leave default
        }
    }

    void OnStatChanged(object sender, StatChangedEventArgs e)
    {
        UpdateDisplay();
    }
}