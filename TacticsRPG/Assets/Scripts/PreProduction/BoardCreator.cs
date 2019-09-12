using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class BoardCreator : MonoBehaviour
{
    [SerializeField] GameObject tileViewPrefab;
    [SerializeField] GameObject tileSelectionIndicatorPrefab;

    //Lazy loading for the selection indicator
    //If it isnt present, then it gets instantiated

    Transform marker
    {
        get
        {
            if (_marker == null)
            {
                GameObject instance = Instantiate(tileSelectionIndicatorPrefab) as GameObject;
                _marker = instance.transform;
            }
            return _marker;
        }
    }

    Transform _marker;

    //Dictionary to map froma point struct to a tile instance
    Dictionary<Point, Tile> tiles = new Dictionary<Point, Tile>();

    //Board dimensions
    //Height -> Steps, how many steps does it take to the unit to go through that tile?
    [SerializeField] int width = 10;
    [SerializeField] int depth = 10;
    [SerializeField] int height = 8;

    //For individual modifications to the game board
    [SerializeField] Point pos;

    //To load previously saved boards
    [SerializeField] LevelData levelData;

    //Methods that are going to be called by the editor script for 
    //the board creation

    public void GrowArea()
    {
        Rect r = RandomRect();
        GrowRect(r);
    }

    public void ShrinkArea()
    {
        Rect r = RandomRect();
        ShrinkRect(r);
    }

    //RandomRect creates a Rect struct somewhere within the specificed board dimensions
    Rect RandomRect()
    {
        int x = UnityEngine.Random.Range(0, width);
        int y = UnityEngine.Random.Range(0, depth);
        int w = UnityEngine.Random.Range(1, width - x + 1);
        int h = UnityEngine.Random.Range(0, depth - y + 1);
        return new Rect(x, y, w, h);
    }

    //Grow and Shrink Rect loop through the range of positions specified by the randomly generated rect area
    //Growing or shrinking a single specified tile
    void GrowRect (Rect rect)
    {
        for (int y = (int)rect.yMin; y < (int)rect.yMax; ++y)
        {
            for (int x = (int)rect.xMin; x < (int)rect.xMax; ++x)
            {
                Point p = new Point(x, y);
                GrowSingle(p);
            }
        }
    }

    void ShrinkRect(Rect rect)
    {
        for (int y = (int) rect.yMin; y<(int)rect.yMax; ++y)
        {
            for (int x = (int) rect.xMin; x<(int)rect.xMax; ++x)
            {
                Point p = new Point(x, y);
                ShrinkSingle(p);
            }
        }
    }

    //To grow a single tile, there is the need of a reference from the dictionary of tiles
    //If the tile doesnt exist, it instantiates one from the prefab

    Tile CreateTile()
    {
        GameObject instance = Instantiate(tileViewPrefab) as GameObject;
        instance.transform.parent = transform;
        return instance.GetComponent<Tile>();
    }

    Tile GetOrCreate(Point p)
    {
        if (tiles.ContainsKey(p))
            return tiles[p];

        Tile t = CreateTile();
        t.Load(p, 0);
        tiles.Add(p, t);

        return t;
    }

    void GrowSingle(Point p)
    {
        Tile t = GetOrCreate(p);
        if (t.height < height)
            t.Grow();
    }

     //Shrink a tile
     //Check if it exists or not, if it doesnt don't create a new one
     //If it shrinks to a height less than zero, then destroy tile

    void ShrinkSingle(Point p)
    {
        if (!tiles.ContainsKey(p))
            return;

        Tile t = tiles[p];
        t.Shrink();

        if (t.height <= 0)
        {
            tiles.Remove(p);
            DestroyImmediate(t.gameObject);
        }
    }

    //To hand modify a single time at time
    //Shrink or Grow based on point pos
    public void Grow()
    {
        GrowSingle(pos);
    }

    public void Shrink()
    {
        ShrinkSingle(pos);
    }

    //To update where the marker is, to see which tile is being modified
    public void UpdateMarker()
    {
        Tile t = tiles.ContainsKey(pos) ? tiles[pos] : null;
        marker.localPosition = t != null ? t.center : new Vector3(pos.x, 0, pos.y);
    }

    //To clear the whole board and the dictionary references
    public void Clear()
    {
        for (int i = transform.childCount - 1; i >= 0; --i)
            DestroyImmediate(transform.GetChild(i).gameObject);
        tiles.Clear();
    }

    //To save the level
    //Stores the tile's position and height data in a list of Vector3

    public void Save()
    {
        string filePath = Application.dataPath + "/Resources/Levels";
        if (!Directory.Exists(filePath))
            CreateSaveDirectory();

        LevelData board = ScriptableObject.CreateInstance<LevelData>();
        board.tiles = new List<Vector3>(tiles.Count);
        foreach (Tile t in tiles.Values)
        {
            board.tiles.Add(new Vector3(t.pos.x, t.height, t.pos.y));
        }

        string fileName = string.Format("Assets/Resources/Levels/{1}.asset",
            filePath, name);
        AssetDatabase.CreateAsset(board, fileName);
    }
    
    void CreateSaveDirectory()
    {
        string filePath = Application.dataPath + "/Resources";
        if (!Directory.Exists(filePath))
            AssetDatabase.CreateFolder("Assets", "Resources");
        filePath += "/Levels";
        if (!Directory.Exists(filePath))
            AssetDatabase.CreateFolder("Assets/Resources", "Levels");
        AssetDatabase.Refresh();
    }

    //To load a previously saved board (needs to be linked up in the inspector)
    public void Load()
    {
        Clear();
        if (levelData == null)
            return;
        foreach (Vector3 v in levelData.tiles)
        {
            Tile t = CreateTile();
            t.Load(v);
            tiles.Add(t.pos, t);
        }
    }
}
