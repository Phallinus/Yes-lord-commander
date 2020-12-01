using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// Furnitures are things like walls, doors, furniture

public class Furnitures
{

    public Tile tile { get; protected set; }

    public string FurnitureType { get ; protected set; }

    // Value of 2 means you move twice as slowly;
    // If movementCost = 0, this tile is impassible (wall...)
    float movementCost;

    int width;
    int height;

    public bool linksToNeighbour { get; protected set; }

    Action<Furnitures> cbOnChanged;

    protected Furnitures()
    {

    }

    static public Furnitures CreatePrototype(string FurnitureType, float movementCost =1f, int width=1, int height=1, bool linksToNeighbour = false)
    {
        Furnitures furn = new Furnitures();
        furn.FurnitureType = FurnitureType;
        furn.movementCost = movementCost;
        furn.width = width;
        furn.height = height;
        furn.linksToNeighbour = linksToNeighbour;

        return furn;
    }

    static public Furnitures PlaceInstance(Furnitures proto, Tile tile)
    {
        Debug.Log(tile.X + "_" + tile.Y);
        if (proto.isValidPosition(tile) == false)
        {
            Debug.LogError("PlaceInstance -- Position validity function returned False");
            return null;
        }

        Furnitures furn = new Furnitures();
        furn.FurnitureType = proto.FurnitureType;
        furn.movementCost = proto.movementCost;
        furn.width = proto.width;
        furn.height = proto.height;
        furn.linksToNeighbour = proto.linksToNeighbour;

        furn.tile = tile;

        if (tile.PlaceFurniture(furn) == false)
        {
            return null;
        }
        
        if(furn.linksToNeighbour)
        {
            Tile t;
            int x = tile.X;
            int y = tile.Y;

            t = tile.world.GetTileAt(x, y + 1);
            if (t != null && t.furniture != null && t.furniture.FurnitureType == furn.FurnitureType)
            {
                t.furniture.cbOnChanged(t.furniture);
            }
            t = tile.world.GetTileAt(x + 1, y);
            if (t != null && t.furniture != null && t.furniture.FurnitureType == furn.FurnitureType)
            {
                t.furniture.cbOnChanged(t.furniture);
            }
            t = tile.world.GetTileAt(x, y - 1);
            if (t != null && t.furniture != null && t.furniture.FurnitureType == furn.FurnitureType)
            {
                t.furniture.cbOnChanged(t.furniture);
            }
            t = tile.world.GetTileAt(x - 1, y);
            if (t != null && t.furniture != null && t.furniture.FurnitureType == furn.FurnitureType)
            {
                t.furniture.cbOnChanged(t.furniture);
            }
        }

        return furn;
    }

    public void RegisterOnChangedCallback(Action<Furnitures> callbackFunc)
    {
        cbOnChanged += callbackFunc;
    }
    public void UnRegisterOnChangedCallback(Action<Furnitures> callbackFunc)
    {
        cbOnChanged -= callbackFunc;
    }

    public bool  isValidPosition (Tile t)
    {
        if (t.Type == TileType.Empty)
        {
            return false;
        }

        if (t.furniture != null)
        {
            return false;
        }
        return true;
    }

    public bool isValidPosition_Door(Tile t)
    {
        if (isValidPosition(t) == false)
        {
            return false;
        }
        return true;
    }
}
