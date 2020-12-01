using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum TileType { Empty, Dirt, Wood, Stone, Snow };

public class Tile 
{
    TileType type = TileType.Empty;

    Action<Tile> cbTileChanged;

    public TileType Type
    {
        get
        {
            return type;
        }
        set
        {
            TileType oldType = type;
            type = value;

            if (cbTileChanged != null && oldType != type)
            {
                cbTileChanged(this);
            }
        }
    }

    Inventory inventory;
    public Furnitures furniture { get; protected set; }

    public World world { get; protected set; }
    public int X { get; protected set; }
    public int Y { get; protected set; }

    public Tile( World world, int x, int y )
    {
        this.world = world;
        this.X = x;
        this.Y = y;
    }

    public void RegisterTileTypeChangedCallback (Action<Tile> callback)
    {
        cbTileChanged += callback;
    }

    public void UnRegisterTileTypeChangedCallback(Action<Tile> callback)
    {
        cbTileChanged -= callback;
    }

    public bool PlaceFurniture(Furnitures furnInstance)
    {
        if (furnInstance == null)
        {
            furniture = null;
            return true;
        }
        if(furniture != null)
        {
            Debug.LogError("Try to assign a Furniture to a tile that already has one");
            return false;
        }
        furniture = furnInstance;
        return true; 
    }

}
