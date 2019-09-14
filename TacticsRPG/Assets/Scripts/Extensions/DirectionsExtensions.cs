using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DirectionsExtensions 
{
    //can get a cardinal direction based off 
    //of the relationship between two tiles
    //i.e Directions d = t1.GetDirection(t2);
    public static Directions GetDirection(this Tile t1, Tile t2)
    {
        if (t1.pos.y < t2.pos.y)
            return Directions.North;
        if (t1.pos.x < t2.pos.x)
            return Directions.East;
        if (t1.pos.y > t2.pos.y)
            return Directions.South;
        return Directions.West;
    }
    // can convert from a Directions enum to a Vector3. 
    //This will come in handy for rotating characters on the board
    //i.e Directions d = Directions.North;
    // Vector3 r = d.ToEuler();
    public static Vector3 ToEuler(this Directions d)
    {
        return new Vector3(0, (int)d * 90, 0);
    }
}
