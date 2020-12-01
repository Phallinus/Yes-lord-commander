using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{

    float soundCooldown = 0;

    void Start()
    {
        WorldController.Instance.World.RegisterFurnitureCreated(OnFurnitureCreated);

        WorldController.Instance.World.RegisterTileChanged(OnTileTypeChanged);
    }


    void Update()
    {
        soundCooldown -= Time.deltaTime;
    }

    void OnTileTypeChanged(Tile tile_data)
    {
        if (soundCooldown> 0)
        {
            return;
        } 
        AudioClip ac = Resources.Load<AudioClip>("Sounds/Dirt_Oncreated");
        AudioSource.PlayClipAtPoint(ac, Camera.main.transform.position);
        soundCooldown = 0.1f;
    }

    public void OnFurnitureCreated(Furnitures furn)
    {
        if (soundCooldown > 0)
        {
            return;
        }
        AudioClip ac = Resources.Load<AudioClip>("Sounds/WoodWall_Oncreated");
        AudioSource.PlayClipAtPoint(ac, Camera.main.transform.position);
        soundCooldown = 0.1f;
    }
}
