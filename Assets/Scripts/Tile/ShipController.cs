using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    public int numPegs;
    public List<int> pegHits;
    public List<(int, int)> shipCoord = new List<(int, int)>();
    public TilesManager tilesManager;

    public GameObject PegSpots;
    public Material lightBlue;
    public Material red;

    public bool shouldClearCoor;
    public bool isEnemy;

    public void DoHit((int,int) location)
    {
        int i = 0;
        foreach((int, int) coor in shipCoord)
        {
            if(coor.Item1 == location.Item1 && coor.Item2 == location.Item2 && !pegHits.Contains(i))
            {
                pegHits.Add(i);
                // ship.GetComponent<Animator>().enabled = false;
                PegSpots.transform.GetChild(i).gameObject.SetActive(true);
                // gameObject.AddComponent<Rigidbody>();
                // GetComponent<Rigidbody>().isKinematic = false;
                // GetComponent<Rigidbody>().useGravity = true;
                // GetComponent<Rigidbody>().AddForce(transform.up * 500f);
                if(pegHits.Count == numPegs)
                {
                    transform.GetChild(0).GetComponent<Animator>().SetTrigger("ShipDeath");
                    shouldClearCoor = true;
                }
            }
            i = i + 1;
        }
    }

    public void ClearShipCoor()
    {
        if (shouldClearCoor)
        {
            foreach ((int, int) coor in shipCoord)
            {
                var renderer = tilesManager.tiles[coor.Item1, coor.Item2].transform.GetChild(0).GetComponent<MeshRenderer>();
                if (renderer.material.name.Contains("RedMaterial"))
                {
                    renderer.material = red;
                }
                else
                {
                    renderer.material = lightBlue;
                }
            }
            shipCoord.Clear();
        }
    }
}
