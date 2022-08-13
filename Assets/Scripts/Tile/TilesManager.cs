using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TilesManager : MonoBehaviour
{
    public TileManager[,] tiles = new TileManager[8, 7];

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
