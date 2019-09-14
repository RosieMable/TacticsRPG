using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Loads LevelData and create a game board level at run-time
public class Board : MonoBehaviour
{
    Point[] dirs = new Point[4]
    {
        new Point(0, 1),
        new Point(0, -1),
        new Point(1, 0),
        new Point(-1, 0)
    };

    Color selectedTileColor = new Color(0, 1, 1, 1);
    Color defaultTileColor = new Color(1, 1, 1, 1);


    [SerializeField] GameObject tilePrefab;
    public Dictionary<Point, Tile> tiles = new Dictionary<Point, Tile>();

    public void Load(LevelData data)
    {
        for (int i = 0; i < data.tiles.Count; i++)
        {
            GameObject instance = Instantiate(tilePrefab) as GameObject;
            Tile t = instance.GetComponent<Tile>();
            t.Load(data.tiles[i]);
            tiles.Add(t.pos, t);
        }
    }

    public void SelectTiles (List<Tile> tiles)
    {
        for (int i = tiles.Count - 1; i >= 0; --i)
        {
            tiles[i].GetComponent<Renderer>().material.SetColor("_Color", selectedTileColor);
        }
    }

    public void DeSelectTiles(List<Tile> tiles)
    {
        for (int i = tiles.Count - 1; i >= 0; --i)
            tiles[i].GetComponent<Renderer>().material.SetColor("_Color", defaultTileColor);
    }

    //Pathfinding logic
    //1 - decide which tile to start from. 
    //The tile is added to a queue of tiles for checking now.
    //This first tile has a distance of 0,
    //and no prev tile which indicates that it is the beginning of the path.


    //This method will return a list of Tiles, 
    //starting from a specific tile, which meet a certain criteria
    public List<Tile> Search (Tile start, Func<Tile, Tile, bool> addTile)
    {
        List<Tile> retValue = new List<Tile>();
        retValue.Add(start);

        ClearSearch();
        Queue<Tile> checkNext = new Queue<Tile>();
        Queue<Tile> checkNow = new Queue<Tile>();

        //Set correct value on start tile and add it to the list to be checked
        start.distance = 0;
        checkNow.Enqueue(start);

        //Loop that dequeues a tile and performs logic on it
        while(checkNow.Count > 0)
        {
            Tile t = checkNow.Dequeue();
            //Logic here

            //nested for loop which gets the tiles 
            //in each direction from the currently selected tile
            for (int i = 0; i < 4; i++)
            {
                Tile next = GetTile(t.pos + dirs[i]);

                //Verify if got a tile reference (there might be holes in the board)
                //If so, compare the distance which it has marked
                if (next == null || next.distance <= t.distance + 1)
                    continue;

                if(addTile(t, next))
                {
                    next.distance = t.distance + 1;
                    next.prev = t;
                    checkNext.Enqueue(next);
                    retValue.Add(next);
                }
            }

            //Queue Cleared? If so, swap queues references of points from the checknow points to the check next
            if (checkNow.Count == 0)
                SwapReference(ref checkNow, ref checkNext);
        }


        return retValue;
    }

    void SwapReference(ref Queue<Tile> a, ref Queue<Tile> b)
    {
        Queue<Tile> temp = a;
        a = b;
        b = temp;
    }

    public Tile GetTile(Point P)
    {
        return tiles.ContainsKey(P) ? tiles[P] : null;
    }

    void ClearSearch()
    {
        foreach (Tile t in tiles.Values)
        {
            t.prev = null;
            t.distance = int.MaxValue;
        }
    }
}
