using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    public int numPegs;
    public List<int> pegHits;
    public List<(int, int)> shipCoord = new List<(int, int)>();

    public GameObject PegSpots;

    public void DoHit((int,int) location)
    {
        int i = 0;
        foreach((int, int) coor in shipCoord)
        {
            if(coor.Item1 == location.Item1 && coor.Item2 == location.Item2)
            {
                pegHits.Add(i);
                // ship.GetComponent<Animator>().enabled = false;
                PegSpots.transform.GetChild(i).gameObject.SetActive(true);
                GetComponent<Rigidbody>().useGravity = true;
                GetComponent<Rigidbody>().AddForce(transform.up * 500f);
            }
            i = i + 1;
        }
    }
}
