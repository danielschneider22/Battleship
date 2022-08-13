using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TilesManager : MonoBehaviour
{
    public TileManager[,] tiles = new TileManager[8, 7];
    public Transform ships;
    public Material darkBlue;
    public Material lightBlue;

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
        closestChild.GetChild(0).GetComponent<MeshRenderer>().material = darkBlue;
        (int startX, int startY) = findTilePos(closestChild);
        ShipController shipController = ship.GetComponent<ShipController>();
        for(int y = startY; y <= startY + (shipController.numPegs - 1); y++ )
        {
            if(y < 7)
            {
                tiles[startX, y].transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material = darkBlue;
            }
            
        }
    }
    private (int, int) findTilePos(Transform tile)
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
            x = x + 1;
            if(x > 7)
            {
                x = 0;
                y = y + 1;
            }
        }

        foreach (Transform child in transform)
        {
            child.GetChild(0).GetComponent<MeshRenderer>().material = lightBlue;
        }

        foreach (Transform child in ships)
        {
            PlaceShipProperly(child.gameObject);
        }
    }
}
