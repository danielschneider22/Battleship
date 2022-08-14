using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilesAttackManager : MonoBehaviour
{
    public TilesManager tilesManager;
    public Material red;
    public Material darkRed;
    public Sprite reticleLocked;

    private float attackTimer = 4f;
    public List<(int, int)> attackingList = new List<(int, int)>();
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
                i = i + 1;
            }
        }
        foreach((int, int) alreadyAttacking in attackingList)
        {
            allCoord.Remove(alreadyAttacking);
        }
        randomTile = allCoord[Random.Range(0, allCoord.Count)];
        attackingList.Add(randomTile);
        GameObject tileObject = tilesManager.tiles[randomTile.Item1, randomTile.Item2].transform.GetChild(0).gameObject;
        tileObject.GetComponent<MeshRenderer>().material = darkRed;
        tileObject.transform.GetChild(0).gameObject.SetActive(true);
        tileObject.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = reticleLocked;
        tileObject.transform.parent.GetComponent<Animator>().SetTrigger("Explode");
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