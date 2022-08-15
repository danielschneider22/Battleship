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
    private int round = 0;
    public List<(int, int)> attackingList = new List<(int, int)>();
    public int numPegsForRound;
    public Transform EnemyPegsRemaining;
    public int pegsAdded = 0;
    public bool inAttackRound = false;
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
        if(attackingList.Count < 20)
        {
            if (allCoord.Count > 0)
            {
                randomTile = allCoord[Random.Range(0, allCoord.Count)];
            }
            else
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

    }

    public void Start()
    {
        StartRound();
    }

    public void StartRound()
    {
        round = round + 1;
        pegsAdded = 0;
        inAttackRound = true;
        if (round == 1)
        {
            numPegsForRound = 4;
            for(var i = 0; i< numPegsForRound; i++)
            {
                EnemyPegsRemaining.GetChild(i).gameObject.SetActive(true);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(attackTimer <= 0f && inAttackRound)
        {
            MakeAttack();
            attackTimer = howLongToWait;
        }
        
        overallTimer += Time.deltaTime;
        attackTimer -= Time.deltaTime;
    }
}
