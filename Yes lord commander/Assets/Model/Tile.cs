﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Tile 
{
    public enum TileType { Empty, Dirt, Wood, Stone, Snow};

    TileType type = TileType.Empty;

    Action<Tile> cbTileTypeChanged;

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

            if (cbTileTypeChanged != null && oldType != type)
            {
                cbTileTypeChanged(this);
            }
        }
    }

    LooseObject looseObject;
    InstalledObject installedObject;

    World world;
    int x;

    public int X
    {
        get
        {
            return x;
        }
    }

    int y;

    public int Y
    {
        get
        {
            return y;
        }
    }

    public Tile( World world, int x, int y )
    {
        this.world = world;
        this.x = x;
        this.y = y;
    }

    public void RegisterTileTypeChangedCallback (Action<Tile> callback)
    {
        cbTileTypeChanged += callback;
    }

    public void UnRegisterTileTypeChangedCallback(Action<Tile> callback)
    {
        cbTileTypeChanged -= callback;
    }

}
