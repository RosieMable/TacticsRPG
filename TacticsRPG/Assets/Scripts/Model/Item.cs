using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "RPG/Item")]
public class Item : ScriptableObject
{
    public string itemName;
    public string description;
    public Sprite icon;
    public ItemType itemType;
    public int value; // Buy/sell price
    
    [Header("Item Effects")]
    public List<ItemFeatureData> features = new List<ItemFeatureData>();
}

[System.Serializable]
public class ItemFeatureData
{
    public StatType targetStat;
    public int amount;
    public FeatureType featureType;
}

public enum FeatureType
{
    StatModifier,
    // Add more feature types as needed
}

public enum ItemType
{
    Consumable,
    Weapon,
    Armor,
    Accessory
}