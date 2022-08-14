using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimmyGuessManager : MonoBehaviour
{
    public int numPegs = 3;
    private UITilesManager uTilesManager;
    public float timeUntilPick = 5f;
    public float pickTimer = 0f;

    private List<(int, int)> pickedList = new List<(int, int)>();

    private void Start()
    {
        uTilesManager = GetComponent<UITilesManager>();
    }

    private void Update()
    {
        pickTimer += Time.deltaTime;
        if(pickTimer >= timeUntilPick)
        {
            uTilesManager.activeTile.OnReveal();
            pickedList.Add(uTilesManager.activeTile.tilePos);
            bool foundRandomTile = false;
            do
            {
                (int, int) randomTile = (Random.Range(0, 8), Random.Range(0, 7));
                if(!pickedList.Contains(randomTile))
                {
                    foundRandomTile = true;
                    uTilesManager.tiles[randomTile.Item1, randomTile.Item2].MakeActiveTile();
                }
            }
            while (!foundRandomTile);
            pickTimer = 0f;
        }
    }
}
