using UnityEngine;

public class WorldController : MonoBehaviour
{
    public Sprite dirtSprite;
    public Sprite stoneSprite;
    public Sprite snowSprite;
    public Sprite woodSprite;

    World world;

    void Start()
    {
        //Creation of the world
        world = new World();

        //Creation of a GameObject for each tiles so they show visually
        for (int x = 0; x < world.Width; x++)
        {
            for (int y = 0; y < world.Height; y++)
            {
                Tile tile_data = world.GetTileAt(x, y);
                GameObject tile_go = new GameObject();
                tile_go.name = "Tile_" + x + "_" + y;
                tile_go.transform.position = new Vector3(tile_data.X, tile_data.Y, 0);
                tile_go.transform.SetParent(this.transform, true);

                tile_go.AddComponent<SpriteRenderer>();

                tile_data.RegisterTileTypeChangedCallback((tile) => { OnTileTypeChanged(tile, tile_go); });
            }
        }
        world.RandomizeTiles();
    }


    void Update()
    {

    }

    void OnTileTypeChanged(Tile tile_data, GameObject tile_go)
    {
        if (tile_data.Type == Tile.TileType.Dirt)
        {
            tile_go.GetComponent<SpriteRenderer>().sprite = dirtSprite;
        }
        else if (tile_data.Type == Tile.TileType.Stone)
        {
            tile_go.GetComponent<SpriteRenderer>().sprite = stoneSprite;
        }
        else if (tile_data.Type == Tile.TileType.Snow)
        {
            tile_go.GetComponent<SpriteRenderer>().sprite = snowSprite;
        }
        else if (tile_data.Type == Tile.TileType.Wood)
        {
            tile_go.GetComponent<SpriteRenderer>().sprite = woodSprite;
        }
        else
        {
            Debug.LogError("unrecognized tile type");
        }
    }
}
