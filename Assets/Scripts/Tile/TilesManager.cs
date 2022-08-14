using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TilesManager : MonoBehaviour
{
    public TileManager[,] tiles = new TileManager[8, 7];
    public Transform ships;
    public Material darkBlue;
    public Material lightBlue;
    public Material orange;
    public Material red;
    public Material darkRed;

    public Sprite warning;
    public Sprite reticleSearching;
    public Sprite reticleLocked;

    public bool badPlacement;
    private List<(int, int)> badPlacementTiles = new List<(int, int)>();

    public void PlaceShipProperly(GameObject ship)
    {
        Transform topPegSpot = ship.transform.GetChild(1).GetChild(0);
        float minDistance = 10000f;
        Transform closestChild = null;
        foreach (Transform child in transform)
        {
            float dist = Vector3.Distance(child.position, topPegSpot.position);
            if(minDistance > dist)
            {
                minDistance = dist;
                closestChild = child;
            }
        }
        Vector3 distBetween = closestChild.GetChild(3).position - topPegSpot.position;
        ship.transform.position = distBetween + ship.transform.position;
        (int startX, int startY) = findTilePos(closestChild);
        ShipController shipController = ship.GetComponent<ShipController>();
        shipController.shipCoord.Clear();
        for (int y = startY; y <= startY + (shipController.numPegs - 1); y++ )
        {
            if(y < 7)
            {
                MeshRenderer renderer = tiles[startX, y].transform.GetChild(0).gameObject.GetComponent<MeshRenderer>();
                if(!renderer.material.name.Contains("RedMaterial"))
                {
                    renderer.material = darkBlue;
                } else
                {
                    renderer.material = darkRed;
                    renderer.transform.GetChild(0).gameObject.SetActive(true);
                    renderer.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = reticleLocked;
                }
                shipController.shipCoord.Add((startX, y));
            }
            
        }
    }

    public ShipController getShipControllerIfActiveCoord(int x, int y)
    {
        foreach (Transform child in ships)
        {
            ShipController shipController = child.gameObject.GetComponent<ShipController>();
            List<(int,int)> myList = shipController.shipCoord;
            if(myList.Contains((x, y)))
            {
                return shipController;
            }
        }
        return null;
    }

    public void DrawDarkBlueOnActiveShip(GameObject ship)
    {
        Transform topPegSpot = ship.transform.GetChild(1).GetChild(0);
        float minDistance = 10000f;
        Transform closestChild = null;
        foreach (Transform child in transform)
        {
            float dist = Vector3.Distance(child.position, topPegSpot.position);
            if (minDistance > dist)
            {
                minDistance = dist;
                closestChild = child;
            }
        }
        (int startX, int startY) = findTilePos(closestChild);
        ShipController shipController = ship.GetComponent<ShipController>();

        // if position hasn't changed don't do anything
        if (startX == shipController.shipCoord[0].Item1 && startY == shipController.shipCoord[0].Item2)
        {
            return;
        } else // otherwise replace all bad tiles (that were orange) with light blue
        {
            foreach((int, int) badTile in badPlacementTiles)
            {
                MeshRenderer renderer = tiles[badTile.Item1, badTile.Item2].transform.GetChild(0).gameObject.GetComponent<MeshRenderer>();
                renderer.transform.GetChild(0).gameObject.SetActive(false);
                if (getShipControllerIfActiveCoord(badTile.Item1, badTile.Item2) != null)
                {
                    if(renderer.material.name.Contains("RedMaterial"))
                    {
                        renderer.material = darkRed;
                        renderer.transform.GetChild(0).gameObject.SetActive(true);
                        renderer.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = reticleLocked;
                    } else
                    {
                        renderer.material = darkBlue;
                        renderer.transform.GetChild(0).gameObject.SetActive(false);
                    }
                    
                }
                else if(!renderer.material.name.Contains("RedMaterial"))
                {
                    renderer.material = lightBlue;
                }
                else
                {
                    renderer.material = red;
                    renderer.transform.GetChild(0).gameObject.SetActive(true);
                    renderer.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = reticleSearching;
                }
                
                
            }
            badPlacementTiles.Clear();

            // set all of the previous coordinate tiles to light blue
            for (int y = shipController.shipCoord[0].Item2; y <= shipController.shipCoord[0].Item2 + (shipController.numPegs - 1); y++)
            {
                if (y < 7 && getShipControllerIfActiveCoord(shipController.shipCoord[0].Item1, y) == null)
                {
                    var renderer = tiles[shipController.shipCoord[0].Item1, y].transform.GetChild(0).gameObject.GetComponent<MeshRenderer>();
                    if(!renderer.material.name.Contains("RedMaterial"))
                    {
                        renderer.material = lightBlue;
                        renderer.transform.GetChild(0).gameObject.SetActive(false);
                    } else
                    {
                        renderer.material = red;
                        renderer.transform.GetChild(0).gameObject.SetActive(true);
                        renderer.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = reticleSearching;
                    }
                    
                }
            }
        }
        
        shipController.shipCoord.Clear();
        badPlacement = false;
        for (int y = startY; y <= startY + (shipController.numPegs - 1); y++)
        {
            if (y < 7)
            {
                var renderer = tiles[startX, y].transform.GetChild(0).gameObject.GetComponent<MeshRenderer>();
                if(!renderer.material.name.Contains("RedMaterial"))
                {
                    renderer.material = darkBlue;
                } else
                {
                    renderer.material = darkRed;
                    renderer.transform.GetChild(0).gameObject.SetActive(true);
                    renderer.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = reticleLocked;
                }
                
                shipController.shipCoord.Add((startX, y));
            } 
            if((getShipControllerIfActiveCoord(startX, y) != null) || y >= 7)
            {
                badPlacement = true;
            }

        }
        if(badPlacement)
        {
            for (int y = startY; y <= startY + (shipController.numPegs - 1); y++)
            {
                if (y < 7)
                {
                    badPlacementTiles.Add((startX, y));
                    var renderer = tiles[startX, y].transform.GetChild(0).gameObject.GetComponent<MeshRenderer>();
                    if(!renderer.material.name.Contains("RedMaterial"))
                    {
                        renderer.material = orange;
                        renderer.transform.GetChild(0).gameObject.SetActive(true);
                        renderer.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = warning;
                    }
                }

            }
        }
    }
    public (int, int) findTilePos(Transform tile)
    {
        for(int x=0; x <= 7; x++)
        {
            for (int y = 0; y <= 6; y++)
            {
                if(tiles[x,y].transform.position == tile.position)
                {
                    return (x, y);
                }
            }
        }
        return (-1, -1);
    }
    private void Start()
    {
        int x = 0;
        int y = 0;
        foreach(Transform child in transform)
        {
            TileManager t = child.GetComponent<TileManager>();
            tiles[x, y] = t;
            t.tilePos = (x, y);
            x = x + 1;
            if(x > 7)
            {
                x = 0;
                y = y + 1;
            }
        }

        foreach (Transform child in ships)
        {
            PlaceShipProperly(child.gameObject);
        }
    }
}
