using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TilesManager : MonoBehaviour
{
    public TileManager[,] tiles = new TileManager[8, 7];

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
        Debug.Log(tiles);
    }
}
