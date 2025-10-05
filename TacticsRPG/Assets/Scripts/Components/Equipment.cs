using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : MonoBehaviour
{
    [SerializeField] Weapon _weapon;
    [SerializeField] Armor _armor;
    [SerializeField] Accessory _accessory;

    Stats _stats;

    void Awake()
    {
        _stats = GetComponent<Stats>();
    }

    void Start()
    {
        // Apply initial equipment bonuses
        if (_weapon != null) EquipWeapon(_weapon);
        if (_armor != null) EquipArmor(_armor);
        if (_accessory != null) EquipAccessory(_accessory);
    }

    public Weapon weapon { get { return _weapon; } }
    public Armor armor { get { return _armor; } }
    public Accessory accessory { get { return _accessory; } }

    public void EquipWeapon(Weapon weapon)
    {
        UnequipWeapon();
        _weapon = weapon;
        AddItemFeatures(_weapon);
    }

    public void UnequipWeapon()
    {
        if (_weapon != null)
        {
            RemoveItemFeatures(_weapon);
            _weapon = null;
        }
    }

    public void EquipArmor(Armor armor)
    {
        UnequipArmor();
        _armor = armor;
        AddItemFeatures(_armor);
    }

    public void UnequipArmor()
    {
        if (_armor != null)
        {
            RemoveItemFeatures(_armor);
            _armor = null;
        }
    }

    public void EquipAccessory(Accessory accessory)
    {
        UnequipAccessory();
        _accessory = accessory;
        AddItemFeatures(_accessory);
    }

    public void UnequipAccessory()
    {
        if (_accessory != null)
        {
            RemoveItemFeatures(_accessory);
            _accessory = null;
        }
    }

    // Store applied modifiers so we can remove them later
    private Dictionary<Item, List<StatModifier>> _appliedModifiers = new Dictionary<Item, List<StatModifier>>();

    void AddItemFeatures(Item item)
    {
        if (item == null) return;

        Stats stats = GetComponent<Stats>();
        if (stats == null) return;

        if (!_appliedModifiers.ContainsKey(item))
            _appliedModifiers[item] = new List<StatModifier>();

        foreach (ItemFeatureData feature in item.features)
        {
            if (feature.featureType == FeatureType.StatModifier)
            {
                // Apply stat modifier
                Stat stat = stats[feature.targetStat];
                if (stat != null)
                {
                    StatModifier modifier = new StatModifier(feature.amount, StatModType.Flat, item);
                    stat.AddModifier(modifier);
                    _appliedModifiers[item].Add(modifier);
                }
            }
        }
    }

    void RemoveItemFeatures(Item item)
    {
        if (item == null) return;

        Stats stats = GetComponent<Stats>();
        if (stats == null) return;

        if (_appliedModifiers.ContainsKey(item))
        {
            foreach (StatModifier modifier in _appliedModifiers[item])
            {
                // Find the stat and remove the specific modifier
                foreach (StatType statType in System.Enum.GetValues(typeof(StatType)))
                {
                    Stat stat = stats[statType];
                    if (stat != null && stat.RemoveModifier(modifier))
                        break; // Modifier found and removed
                }
            }
            _appliedModifiers.Remove(item);
        }
    }
}