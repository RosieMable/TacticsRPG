using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    //Four steps are one tile
    public const float stepHeight = .25f;

    //Track its own position and height
    public Point pos;
    public int height;

    //Convenience property called Center
    //Lets place other objects in the center of the top of the tile
    public Vector3 center { get { return new Vector3(pos.x, height * stepHeight, pos.y); } }

    //Tile can hold something (char, traps, trees, etc)
    public GameObject content;

    //Variables for the pathfinding
    //prev - stores the tile which was traversed to reach it
    [HideInInspector] public Tile prev;
    //distance - store number of tiles which have been crossed to reach this point
    [HideInInspector] public int distance;

    //When the height is modified, then the tiles are visually updated
    void Match()
    {
        transform.localPosition = new Vector3(pos.x, height * stepHeight / 2f, pos.y);
        transform.localScale = new Vector3(1, height * stepHeight, 1);
    }

    //The board creator will create boards by randomly growing and/or shrinking tiles
    //Methods to shrink and grow

    public void Grow()
    {
        height++;
        Match();
    }

    public void Shrink()
    {
        height--;
        Match();
    }

    //Method called Load, overloaded to accept various parameter types
    //Makes it easier to persist the Tile data as a Vector3

    public void Load (Point p, int h)
    {
        pos = p;
        height = h;
        Match();
    }

    public void Load (Vector3 v)
    {
        Load(new Point((int)v.x, (int)v.z), (int)v.y);
    }
}
