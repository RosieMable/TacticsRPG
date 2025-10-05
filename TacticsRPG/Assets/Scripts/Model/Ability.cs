using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "RPG/Ability")]
public class Ability : ScriptableObject
{
    public string abilityName;
    public string description;
    public Sprite icon;
    public int mpCost;
    public AbilityCategory category;

    [Header("Components")]
    public AbilityRange range;
    public AbilityAreaOfEffect areaOfEffect;
    public List<AbilityEffect> effects = new List<AbilityEffect>();

    public List<Tile> GetTilesInRange(Board board, Point pos)
    {
        return range.GetTilesInRange(board, pos);
    }

    public List<Tile> GetTilesInArea(Board board, Point pos)
    {
        return areaOfEffect.GetTilesInArea(board, pos);
    }
}

public enum AbilityCategory
{
    Attack,
    Magic,
    Support,
    Item
}