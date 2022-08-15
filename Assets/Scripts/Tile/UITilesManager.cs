using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITilesManager : MonoBehaviour
{
    public UITileManager[,] tiles = new UITileManager[8, 7];
    public Transform ships;
    public Sprite hitSprite;
    public UITileManager activeTile;

    private void PlaceShip(ShipController shipController)
    {
        bool placedShip = false;
        do 
        {
            int randomX = Random.Range(0, 8);
            int randomY = Random.Range(0, 7);
            string direction = Random.Range(0, 2) == 0 ? "right" : "down";
            if (direction == "right")
            {
                bool isValid = true;
                for (int x = randomX; x < shipController.numPegs + randomX; x++)
                {
                    if (x > 7)
                    {
                        isValid = false;
                    }
                    else if (tiles[x, randomY].shipController != null)
                    {
                        isValid = false;
                    }
                }
                if (isValid)
                {
                    for (int x = randomX; x < shipController.numPegs + randomX; x++)
                    {
                        shipController.shipCoord.Add((x, randomY));
                        tiles[x, randomY].shipController = shipController;
                        tiles[x, randomY].spriteRenderer.sprite = hitSprite;
                    }
                    placedShip = true;
                }
            }
            else
            {
                bool isValid = true;
                for (int y = randomY; y < shipController.numPegs + randomY; y++)
                {
                    if (y > 6)
                    {
                        isValid = false;
                    }
                    else if (tiles[randomX, y].shipController != null)
                    {
                        isValid = false;
                    }
                }
                if (isValid)
                {
                    for (int y = randomY; y < shipController.numPegs + randomY; y++)
                    {
                        shipController.shipCoord.Add((randomX, y));
                        tiles[randomX, y].shipController = shipController;
                        tiles[randomX, y].spriteRenderer.sprite = hitSprite;
                    }
                    placedShip = true;
                }
            }

        } while (!placedShip);

    }

    private void Start()
    {
        int x = 0;
        int y = 0;
        foreach (Transform child in transform)
        {
            UITileManager t = child.GetComponent<UITileManager>();
            tiles[x, y] = t;
            t.tilePos = (x, y);
            x = x + 1;
            if (x > 7)
            {
                x = 0;
                y = y + 1;
            }
        }

        foreach (Transform child in ships)
        {
            PlaceShip(child.gameObject.GetComponent<ShipController>());
        }

        // (int, int) randomTile = (Random.Range(0, 8), Random.Range(0, 7));
        // tiles[randomTile.Item1, randomTile.Item2].MakeActiveTile();
    }
}
