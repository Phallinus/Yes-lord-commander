using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public class WorldController : MonoBehaviour
{

    static WorldController _instance;
    public static WorldController Instance { get { return _instance; } }

    public Sprite dirtSprite;
    public Sprite stoneSprite;
    public Sprite snowSprite;
    public Sprite woodSprite;
    public Sprite emptySprite;

    Dictionary<Tile, GameObject> tileGameObjectMap;
    Dictionary<Furnitures, GameObject> FurnitureGameObjectMap;

    Dictionary<string, Sprite> FurnitureSprites;

    public World World { get; protected set; }

    void OnEnable()
    {

        LoadSprites();

        if(_instance != null)
        {
            Debug.LogError("There should never be 2 world controllers.");
        }
        //Creation of the world
        World = new World();

        World.RegisterFurnitureCreated(OnFurnitureCreated);

        //Instantiate dictionary that tracks which GameObject is rendering which tile data
        tileGameObjectMap = new Dictionary<Tile, GameObject>();
        FurnitureGameObjectMap = new Dictionary<Furnitures, GameObject>();

        //Creation of a GameObject for each tiles so they show visually
        for (int x = 0; x < World.Width; x++)
        {
            for (int y = 0; y < World.Height; y++)
            {
                Tile tile_data = World.GetTileAt(x, y);
                GameObject tile_go = new GameObject();

                //Add tile/GO pair to the dictionnary
                tileGameObjectMap.Add(tile_data, tile_go);

                tile_go.name = "Tile_" + x + "_" + y;
                tile_go.transform.position = new Vector3(tile_data.X, tile_data.Y, 0);
                tile_go.transform.SetParent(this.transform, true);

                tile_go.AddComponent<SpriteRenderer>().sprite = emptySprite;

                //Register the callback so the gameobject is updated when the tile type's change
                
            }
        }
        World.RegisterTileChanged(OnTileChanged);

        Camera.main.transform.position = new Vector3 (World.Width/2, World.Height/2 , Camera.main.transform.position.z);
        //World.RandomizeTiles();
        _instance = this;
    }


    void LoadSprites()
    {
        FurnitureSprites = new Dictionary<string, Sprite>();
        Sprite[] sprites = Resources.LoadAll<Sprite>("Images/Furnitures");

        foreach (Sprite s in sprites)
        {
            FurnitureSprites[s.name] = s;
        }
    }

    void Update()
    {

    }

    void DestroyAllTileGameObjects()
    {
        //Destroying all visuals gameobjects but not the data, may be used when changing floors/level
        while (tileGameObjectMap.Count > 0)
        {
            Tile tile_data = tileGameObjectMap.Keys.First();
            GameObject tile_go = tileGameObjectMap[tile_data];

            tileGameObjectMap.Remove(tile_data);
            tile_data.UnRegisterTileTypeChangedCallback(OnTileChanged);
            Destroy(tile_go);
        }
    }

    void OnTileChanged(Tile tile_data)
    {
        if(tileGameObjectMap.ContainsKey(tile_data) == false)
        {
            Debug.LogError("tileGameObjectMap doesn't contain the tile_data -- did you forget to add the tile to the dictionnary? Or maybe forget to unregister a callback ?");
            return;
        }

        GameObject tile_go = tileGameObjectMap[tile_data];

        if (tile_go == null)
        {
            Debug.LogError("tileGameObjectMap's returned GameObject is null -- did you forget to add the tile to the dictionnary? Or maybe forget to unregister a callback ?");
            return;
        }

        if (tile_data.Type == TileType.Dirt)
        {
            tile_go.GetComponent<SpriteRenderer>().sprite = dirtSprite;
        }
        else if (tile_data.Type == TileType.Stone)
        {
            tile_go.GetComponent<SpriteRenderer>().sprite = stoneSprite;
        }
        else if (tile_data.Type == TileType.Snow)
        {
            tile_go.GetComponent<SpriteRenderer>().sprite = snowSprite;
        }
        else if (tile_data.Type == TileType.Wood)
        {
            tile_go.GetComponent<SpriteRenderer>().sprite = woodSprite;
        }
        else if (tile_data.Type == TileType.Empty )
        {
            tile_go.GetComponent<SpriteRenderer>().sprite = null;
        }
        else
        {
            Debug.LogError("unrecognized tile type");
        }
    }

    public Tile GetTileAtWorldCoord(Vector3 coord)
    {
        int x = Mathf.FloorToInt(coord.x);
        int y = Mathf.FloorToInt(coord.y);

        return WorldController.Instance.World.GetTileAt(x, y);
    }

    public void OnFurnitureCreated (Furnitures furn)
    {
        GameObject furn_go = new GameObject();

        FurnitureGameObjectMap.Add(furn, furn_go);

        furn_go.name = furn.FurnitureType + "_" + furn.tile.X + "_" + furn.tile.Y;
        furn_go.transform.position = new Vector3(furn.tile.X, furn.tile.Y, -1);
        furn_go.transform.SetParent(this.transform, true);

        furn_go.AddComponent<SpriteRenderer>().sprite = GetSpriteForFurniture(furn);

        //Register the callback so the gameobject is updated when the tile type's change
        furn.RegisterOnChangedCallback(OnFurnitureChanged);
    }
    void OnFurnitureChanged(Furnitures furn)
    {

        if (FurnitureGameObjectMap.ContainsKey(furn) == false)
        {
            Debug.LogError("OnFurnitureChanged -- trying to change visuals for furniture not in our map");
            return;
        }
        GameObject furn_go = FurnitureGameObjectMap[furn];
        furn_go.GetComponent<SpriteRenderer>().sprite = GetSpriteForFurniture(furn);

    }

    Sprite GetSpriteForFurniture(Furnitures furn)
    {
        if (furn.linksToNeighbour == false)
        {
            return FurnitureSprites[furn.FurnitureType];
        }

        string spriteName = furn.FurnitureType + "_";

        int x = furn.tile.X;
        int y = furn.tile.Y;

        //Check for neighbours

        Tile t;

        t = World.GetTileAt(x, y + 1);
        if (t != null && t.furniture != null && t.furniture.FurnitureType == furn.FurnitureType)
        {
            spriteName += "N";
        }
        t = World.GetTileAt(x +1, y);
        if (t != null && t.furniture != null && t.furniture.FurnitureType == furn.FurnitureType)
        {
            spriteName += "E";
        }
        t = World.GetTileAt(x, y - 1);
        if (t != null && t.furniture != null && t.furniture.FurnitureType == furn.FurnitureType)
        {
            spriteName += "S";
        }
        t = World.GetTileAt(x - 1, y);
        if (t != null && t.furniture != null && t.furniture.FurnitureType == furn.FurnitureType)
        {
            spriteName += "W";
        }

        if(FurnitureSprites.ContainsKey(spriteName) == false)
        {
            Debug.LogError("GetSpriteForFurniture -- No sprites with name: " + spriteName);
            return null;
        }

        return FurnitureSprites[spriteName];
    }

    
}
