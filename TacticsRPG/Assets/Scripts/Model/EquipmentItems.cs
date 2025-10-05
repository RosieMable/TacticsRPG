using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "RPG/Weapon")]
public class Weapon : Item
{
    void Awake()
    {
        itemType = ItemType.Weapon;
    }
}

[CreateAssetMenu(menuName = "RPG/Armor")]
public class Armor : Item
{
    void Awake()
    {
        itemType = ItemType.Armor;
    }
}

[CreateAssetMenu(menuName = "RPG/Accessory")]
public class Accessory : Item
{
    void Awake()
    {
        itemType = ItemType.Accessory;
    }
}