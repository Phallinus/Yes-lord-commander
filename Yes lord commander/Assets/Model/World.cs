using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World
{
    Tile[,] tiles;
    int width;

    public int Width
    {
        get
        {
            return width;
        }
    }

    int height;
    public int Height
    {
        get
        {
            return height;
        }
    }

    public World(int width = 50, int height = 50)
    {
        this.width = width;
        this.height = height;

        tiles = new Tile[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                tiles[x, y] = new Tile(this, x, y);
            }
        }

        Debug.Log("World created with " + (width * height) + " tiles.");
    }

    public void RandomizeTiles()
    {
        Debug.Log("Tiles Randomized");
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int randomtile = Random.Range(0, 4);
                if (randomtile == 0)
                {
                    tiles[x, y].Type = Tile.TileType.Dirt;
                }
                if (randomtile == 1)
                {
                    tiles[x, y].Type = Tile.TileType.Snow;
                }
                if (randomtile == 2)
                {
                    tiles[x, y].Type = Tile.TileType.Stone;
                }
                if (randomtile == 3)
                {
                    tiles[x, y].Type = Tile.TileType.Wood;
                }
            }
        }
    }

    public Tile GetTileAt(int x, int y)
    {
        if (x > width || x < 0 || y > height || y < 0)
        {
            Debug.LogError("Tile (" + x + "," + y + ") is out of range");
            return null;
        }
        return tiles[x, y];
    }
}
