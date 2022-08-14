using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilesAttackManager : MonoBehaviour
{
    public TilesManager tilesManager;
    public Material red;

    private float attackTimer = 4f;
    private void MakeAttack()
    {
        (int, int) randomTile = (-1, -1);
        List<(int, int)> allCoord = new List<(int, int)>();
        foreach(Transform child in tilesManager.ships)
        {
            ShipController controller = child.GetComponent<ShipController>();
            int i = 0;
            foreach ((int, int) coor in controller.shipCoord)
            {
                if(!controller.pegHits.Contains(i))
                {
                    allCoord.Add(coor);
                }
            }
        }
        randomTile = allCoord[Random.Range(0, allCoord.Count)];
        tilesManager.tiles[randomTile.Item1, randomTile.Item2].transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material = red;
    }

    // Update is called once per frame
    void Update()
    {
        if(attackTimer <= 0f)
        {
            MakeAttack();
            attackTimer = 4f;
        }
        attackTimer -= Time.deltaTime;
    }
}
