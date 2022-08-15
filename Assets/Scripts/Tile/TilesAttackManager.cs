using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilesAttackManager : MonoBehaviour
{
    public UITilesManager uitilesManager;
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
    public GameObject EnemyPegsRemainingOtherStuff;
    public GameObject RoundOverRemainingOtherStuff;
    public GameObject RoundOverBackdrop;
    public AnimationAndMovementController animController;
    public Sprite reticleSearching;
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

    private void MakeBadAttack()
    {
        (int, int) randomTile = (-1, -1);
        if (attackingList.Count < 20)
        {
            do
            {
                randomTile = (Random.Range(0, 8), Random.Range(0, 7));
            } while (attackingList.Contains(randomTile));

            attackingList.Add(randomTile);
            GameObject tileObject = tilesManager.tiles[randomTile.Item1, randomTile.Item2].transform.GetChild(0).gameObject;
            if(tilesManager.getShipControllerIfActiveCoord(randomTile.Item1, randomTile.Item2) != null)
            {
                tileObject.GetComponent<MeshRenderer>().material = darkRed;
                tileObject.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = reticleLocked;
            } else
            {
                tileObject.GetComponent<MeshRenderer>().material = red;
                tileObject.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = reticleSearching;
            }
            
            tileObject.transform.GetChild(0).gameObject.SetActive(true);
            tileObject.transform.parent.GetComponent<Animator>().SetTrigger("Explode");

        }

    }

    private void MakeRowAttack()
    {
        (int, int) randomTile = (-1, -1);
        if (attackingList.Count < 20)
        {
            int randomRow = Random.Range(0, 8);

            for (var y = 0; y < 7; y++ )
            {
                randomTile = (randomRow, y);
                if (!attackingList.Contains(randomTile))
                {
                    attackingList.Add(randomTile);
                    TileManager tileManager = tilesManager.tiles[randomTile.Item1, randomTile.Item2];
                    GameObject tileObject = tileManager.transform.GetChild(0).gameObject;
                    if (tilesManager.getShipControllerIfActiveCoord(randomTile.Item1, randomTile.Item2) != null)
                    {
                        tileObject.GetComponent<MeshRenderer>().material = darkRed;
                        tileObject.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = reticleLocked;
                    }
                    else
                    {
                        tileObject.GetComponent<MeshRenderer>().material = red;
                        tileObject.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = reticleSearching;
                    }

                    tileObject.transform.GetChild(0).gameObject.SetActive(true);
                    tileObject.transform.parent.GetComponent<Animator>().SetTrigger("Explode");
                    tileManager.doNotCreatePeg = Random.Range(0, 4) > 0;
                }
                
            }


        }

    }
    private void MakeColumnAttack()
    {
        (int, int) randomTile = (-1, -1);
        if (attackingList.Count < 20)
        {
            int randomCol = Random.Range(0, 7);

            for (var x = 0; x < 8; x++)
            {
                randomTile = (x, randomCol);
                if (!attackingList.Contains(randomTile))
                {
                    attackingList.Add(randomTile);
                    TileManager tileManager = tilesManager.tiles[randomTile.Item1, randomTile.Item2];
                    GameObject tileObject = tileManager.transform.GetChild(0).gameObject;
                    if (tilesManager.getShipControllerIfActiveCoord(randomTile.Item1, randomTile.Item2) != null)
                    {
                        tileObject.GetComponent<MeshRenderer>().material = darkRed;
                        tileObject.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = reticleLocked;
                    }
                    else
                    {
                        tileObject.GetComponent<MeshRenderer>().material = red;
                        tileObject.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = reticleSearching;
                    }

                    tileObject.transform.GetChild(0).gameObject.SetActive(true);
                    tileObject.transform.parent.GetComponent<Animator>().SetTrigger("Explode");
                    tileManager.doNotCreatePeg = Random.Range(0, 4) > 0;
                }

            }


        }

    }

    public void Start()
    {
        // StartRound();
        FinishRound();
    }

    public void FinishRound()
    {
        foreach(Transform child in uitilesManager.transform)
        {
            child.gameObject.SetActive(true);
        }
        uitilesManager.GetComponent<TimmyGuessManager>().UpdateInventoryText();
        EnemyPegsRemaining.gameObject.SetActive(false);
        EnemyPegsRemainingOtherStuff.SetActive(false);
        RoundOverRemainingOtherStuff.SetActive(true);
        RoundOverBackdrop.SetActive(true);
        animController.enabled = false;
    }

    public void StartRound()
    {
        foreach (Transform child in uitilesManager.transform)
        {
            child.gameObject.SetActive(false);
        }
        EnemyPegsRemaining.gameObject.SetActive(true);
        EnemyPegsRemainingOtherStuff.SetActive(true);
        RoundOverRemainingOtherStuff.SetActive(false);
        RoundOverBackdrop.SetActive(false);
        animController.enabled = true;

        round = round + 1;
        pegsAdded = 0;
        inAttackRound = true;
        if (round == 1)
        {
            numPegsForRound = 5;
            
        } else if(round == 2)
        {
            numPegsForRound = 8;
            howLongToWait = 7f;
        }
        else if (round == 3)
        {
            numPegsForRound = 30;
            howLongToWait = 8f;
        }
        else if (round == 4)
        {
            numPegsForRound = 30;
            howLongToWait = 7f;
        }

        for (var i = 0; i < numPegsForRound; i++)
        {
            EnemyPegsRemaining.GetChild(i).gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(attackTimer <= 0f && inAttackRound)
        {
            switch (round)
            {
                case (1):
                {
                    MakeAttack();
                    break;
                }
                case (2):
                {
                    MakeAttack();
                    if(Random.Range(0, 4) == 0)
                    {
                        MakeBadAttack();
                    }
                    break;
                }
                case (3):
                {
                    if (Random.Range(0, 2) == 0)
                    {
                        MakeRowAttack();
                    }
                    else
                    {
                        MakeColumnAttack();
                    }
                    MakeBadAttack();
                    
                    
                    break;
                }
                case (4):
                default:
                {
                    if (Random.Range(0, 2) == 0)
                    {
                        if (Random.Range(0, 2) == 0)
                        {
                            MakeRowAttack();
                        }
                        else
                        {
                            MakeColumnAttack();
                        }
                        MakeAttack();
                    }
                    else
                    {
                        MakeBadAttack();
                        MakeBadAttack();
                        MakeBadAttack();
                    }

                    break;
                }
            }
            
            attackTimer = howLongToWait;
        }
        
        overallTimer += Time.deltaTime;
        attackTimer -= Time.deltaTime;
    }
}
