using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class World
{
    public Tile[,] tiles;

    Dictionary<string, Furnitures> FurniturePrototypes;

    public int Width { get; protected set; }

    public int Height { get; protected set; }

    Action<Furnitures> cbFurnitureCreated;
    Action<Tile> cbTileChanged;

    public World(int width = 50, int height = 50)
    {
        this.Width = width;
        this.Height = height;

        tiles = new Tile[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                tiles[x, y] = new Tile(this, x, y);
                tiles[x, y].RegisterTileTypeChangedCallback(OnTileChanged);
            }
        }

        Debug.Log("World created with " + (width * height) + " tiles.");

        CreateFurniturePrototypes();
    }

    void CreateFurniturePrototypes()
    {
        FurniturePrototypes = new Dictionary<string, Furnitures>();

        FurniturePrototypes.Add("Woodwall", Furnitures.CreatePrototype("Woodwall", 0, 1, 1, true));
        
    }

    public void RandomizeTiles()
    {
        Debug.Log("Tiles Randomized");
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                int randomtile = UnityEngine.Random.Range(0, 4);
                if (randomtile == 0)
                {
                    tiles[x, y].Type = TileType.Dirt;
                }
                if (randomtile == 1)
                {
                    tiles[x, y].Type = TileType.Snow;
                }
                if (randomtile == 2)
                {
                    tiles[x, y].Type = TileType.Stone;
                }
                if (randomtile == 3)
                {
                    tiles[x, y].Type = TileType.Wood;
                }
            }
        }
    }

    public Tile GetTileAt(int x, int y)
    {
        if (x >= Width || x < 0 || y >= Height || y < 0)
        {
            Debug.LogError("Tile (" + x + "," + y + ") is out of range");
            return null;
        }
        return tiles[x,y];
    }


    public void PlaceFurniture(string FurnitureType, Tile t)
    {
        //Debug.Log("PlaceFurniture");
        if(FurniturePrototypes.ContainsKey(FurnitureType) == false)
        {
            Debug.LogError("FurniturePrototypes doesn't contain a proto for key : " + FurnitureType);
            return;
        }

        Furnitures furn = Furnitures.PlaceInstance(FurniturePrototypes[FurnitureType], t);

        if (furn == null)
        {
            return;
        }

        if(cbFurnitureCreated != null)
        {
            cbFurnitureCreated(furn);
        }
    }

    
    public void RegisterFurnitureCreated(Action<Furnitures> callbackfunc)
    {
        cbFurnitureCreated += callbackfunc;
    }
    public void UnRegisterFurnitureCreated(Action<Furnitures> callbackfunc)
    {
        cbFurnitureCreated -= callbackfunc;
    }
    public void RegisterTileChanged(Action<Tile> callbackfunc)
    {
        cbTileChanged += callbackfunc;
    }
    public void UnRegisterTileChanged(Action<Tile> callbackfunc)
    {
        cbTileChanged -= callbackfunc;
    }

    void OnTileChanged(Tile t)
    {
        if (cbTileChanged == null)
        {
            return;
        }
        cbTileChanged(t);
    }
}
