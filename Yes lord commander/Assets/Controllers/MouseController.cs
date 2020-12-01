using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEditor.UI;
using UnityEditor.Events;

public class MouseController : MonoBehaviour
{

    public GameObject cursorPrefab;

    bool buildModeisFurniture = false;

    TileType buildModeTile = TileType.Dirt;
    string buildModeFurnitureType;

    //World position of the mouse last frame
    Vector3 lastFramePosition;
    Vector3 currFramePosition;

    //World position start of our left-mouse drag
    Vector3 dragStartPosition;
    List<GameObject> dragPreviewGameObjects;

    void Start()
    {
        dragPreviewGameObjects = new List<GameObject>();   
    }

    void Update()
    {
        currFramePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        currFramePosition.z = 0;

        //UpdateCursor();
        UpdateDragging();
        UpdateCameraMovement();

        lastFramePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        lastFramePosition.z = 0;
    }

    /*
    //Cursor position on tiles
    void UpdateCursor()
    {
        //Update cursor position
        Tile tileUnderMouse = WorldController.Instance.GetTileAtWorldCoord(currFramePosition);
        if (tileUnderMouse != null)
        {
            Cursor.SetActive(true);
            Vector3 cursorPosition = new Vector3(tileUnderMouse.X, tileUnderMouse.Y, 0);
            Cursor.transform.position = cursorPosition;
        }
        else
        {
            Cursor.SetActive(false);
        }
    }
    */

    //Tile painting
    void UpdateDragging()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        int start_x = Mathf.FloorToInt(dragStartPosition.x);
        int end_x = Mathf.FloorToInt(currFramePosition.x);
        int start_y = Mathf.FloorToInt(dragStartPosition.y);
        int end_y = Mathf.FloorToInt(currFramePosition.y);

        if (end_x < start_x)
        {
            int tmp = end_x;
            end_x = start_x;
            start_x = tmp;
        }
        if (end_y < start_y)
        {
            int tmp = end_y;
            end_y = start_y;
            start_y = tmp;
        }

        //Start drag, can't if you're over a UI element
        if (Input.GetMouseButtonDown(0))
        {
            dragStartPosition = currFramePosition;
        }

        //Refresh the preview of the drag area
        while (dragPreviewGameObjects.Count>0)
        {
            GameObject go = dragPreviewGameObjects[0];
            dragPreviewGameObjects.RemoveAt(0);
            SimplePool.Despawn(go);
        }

        //Display a preview of the drag area
        if (Input.GetMouseButton(0))
        {
            for (int x = start_x; x <= end_x; x++)
            {
                for (int y = start_y; y <= end_y; y++)
                {
                    Tile t = WorldController.Instance.World.GetTileAt(x, y);
                    if (t != null)
                    {
                        GameObject go = SimplePool.Spawn(cursorPrefab, new Vector3(x, y, 0), Quaternion.identity);
                        go.transform.SetParent(this.transform, true);
                        dragPreviewGameObjects.Add(go);
                    }
                }
            }
        }

        //End drag
        if (Input.GetMouseButtonUp(0))
        {
            for (int x = start_x; x <= end_x; x++)
            {
                for (int y = start_y; y <= end_y; y++)
                {
                    Tile t = WorldController.Instance.World.GetTileAt(x, y);
                    if (t != null)
                    {
                        if (buildModeisFurniture == true)
                        {
                            //create the Furnitures and assign it to the tile
                            WorldController.Instance.World.PlaceFurniture(buildModeFurnitureType, t);
                        }
                        else
                        {
                            // Tile changing mode
                            t.Type = buildModeTile;
                        }
                        
                    }
                }
            }
        }
    }

    //Handle screen dragging
    void UpdateCameraMovement()
    {
        if (Input.GetMouseButton(1) || Input.GetMouseButton(2))
        {
            Vector3 diff = lastFramePosition - currFramePosition;
            Camera.main.transform.Translate(diff);
        }

        Camera.main.orthographicSize -= Camera.main.orthographicSize * Input.GetAxis("Mouse ScrollWheel");

        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, 3f, 20f);
    }

    public void SetMode_BuildDirt()
    {
        buildModeisFurniture = false;
        buildModeTile = TileType.Dirt;
    }
    public void SetMode_BuildWood()
    {
        buildModeisFurniture = false;
        buildModeTile = TileType.Wood;
    }
    public void SetMode_BuildStone()
    {
        buildModeisFurniture = false;
        buildModeTile = TileType.Stone;
    }
    public void SetMode_BuildSnow()
    {
        buildModeisFurniture = false;
        buildModeTile = TileType.Snow;
    }
    public void SetMode_Erase()
    {
        buildModeisFurniture = false;
        buildModeTile = TileType.Empty;
    }
    public void SetMode_BuildFurniture(string FurnitureType)
    {
        buildModeisFurniture = true;
        buildModeFurnitureType = FurnitureType;
    }

}
