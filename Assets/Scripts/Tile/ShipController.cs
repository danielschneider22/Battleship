using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    public int numPegs;
    public List<int> pegHits;
    public List<(int, int)> shipCoord = new List<(int, int)>();
}
