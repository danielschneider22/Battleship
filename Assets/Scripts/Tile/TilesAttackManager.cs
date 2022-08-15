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
    private float howLongToWait = 8f;
    private float overallTimer = 0f;
    private string difficulty = "really easy";
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
        if(allCoord.Count > 0)
        {
            randomTile = allCoord[Random.Range(0, allCoord.Count)];
        } else
        {
            do
            {
                randomTile = (Random.Range(0, 8), Random.Range(0, 7));
            } while (attackingList.Contains(randomTile));
            
        }
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
            attackTimer = howLongToWait;
        }
        if (overallTimer > 90f)
        {
            howLongToWait = 5f;
            difficulty = "hard";
        }
        else if (overallTimer > 60f)
        {
            howLongToWait = 6f;
            difficulty = "medium";
        }
        else if (overallTimer > 30f)
        {
            howLongToWait = 7f;
            difficulty = "easy";
        }
        
        
        overallTimer += Time.deltaTime;
        attackTimer -= Time.deltaTime;
    }
}
