using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Base class for ability range calculations
public abstract class AbilityRange : ScriptableObject
{
    public abstract List<Tile> GetTilesInRange(Board board, Point pos);
}

// Range based on movement distance
[CreateAssetMenu(menuName = "RPG/Ability Range/Self")]
public class SelfAbilityRange : AbilityRange
{
    public override List<Tile> GetTilesInRange(Board board, Point pos)
    {
        List<Tile> retValue = new List<Tile>();
        Tile tile = board.GetTile(pos);
        if (tile != null)
            retValue.Add(tile);
        return retValue;
    }
}

[CreateAssetMenu(menuName = "RPG/Ability Range/Specify")]
public class SpecifyAbilityRange : AbilityRange
{
    public int horizontal = 1;
    public int vertical = 1;

    public override List<Tile> GetTilesInRange(Board board, Point pos)
    {
        List<Tile> retValue = new List<Tile>();
        
        for (int x = pos.x - horizontal; x <= pos.x + horizontal; ++x)
        {
            for (int z = pos.y - vertical; z <= pos.y + vertical; ++z)
            {
                Point p = new Point(x, z);
                Tile tile = board.GetTile(p);
                if (tile != null)
                    retValue.Add(tile);
            }
        }
        
        return retValue;
    }
}

[CreateAssetMenu(menuName = "RPG/Ability Range/Global")]
public class GlobalAbilityRange : AbilityRange
{
    public override List<Tile> GetTilesInRange(Board board, Point pos)
    {
        return new List<Tile>(board.tiles.Values);
    }
}