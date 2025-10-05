using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Base class for ability area of effect
public abstract class AbilityAreaOfEffect : ScriptableObject
{
    public abstract List<Tile> GetTilesInArea(Board board, Point pos);
}

[CreateAssetMenu(menuName = "RPG/Ability AOE/Self")]
public class SelfAbilityAreaOfEffect : AbilityAreaOfEffect
{
    public override List<Tile> GetTilesInArea(Board board, Point pos)
    {
        List<Tile> retValue = new List<Tile>();
        Tile tile = board.GetTile(pos);
        if (tile != null)
            retValue.Add(tile);
        return retValue;
    }
}

[CreateAssetMenu(menuName = "RPG/Ability AOE/Specify")]
public class SpecifyAbilityAreaOfEffect : AbilityAreaOfEffect
{
    public List<Point> pattern = new List<Point>();

    public override List<Tile> GetTilesInArea(Board board, Point pos)
    {
        List<Tile> retValue = new List<Tile>();
        
        for (int i = 0; i < pattern.Count; ++i)
        {
            Point p = pos + pattern[i];
            Tile tile = board.GetTile(p);
            if (tile != null)
                retValue.Add(tile);
        }
        
        return retValue;
    }
}

[CreateAssetMenu(menuName = "RPG/Ability AOE/Full")]
public class FullAbilityAreaOfEffect : AbilityAreaOfEffect
{
    public override List<Tile> GetTilesInArea(Board board, Point pos)
    {
        return new List<Tile>(board.tiles.Values);
    }
}