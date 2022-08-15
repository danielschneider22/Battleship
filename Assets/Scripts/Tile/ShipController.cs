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
    public Material redCapsuleMaterial;

    public bool shouldClearCoor;
    public bool isEnemyShip;
    public Canvas gameOverCanvas;
    public Canvas winningCanvas;
    public TimmyGuessManager timmyGuessManager;
    private AudioManager audiomanager;

    private void Start()
    {
        audiomanager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
    }

    public void DoHit((int,int) location)
    {
        int i = 0;
        foreach((int, int) coor in shipCoord)
        {
            if(coor.Item1 == location.Item1 && coor.Item2 == location.Item2 && !pegHits.Contains(i))
            {
                // audiomanager.Play("ShipDeath", false);
                pegHits.Add(i);
                // ship.GetComponent<Animator>().enabled = false;
                PegSpots.transform.GetChild(i).gameObject.SetActive(true);
                PegSpots.transform.GetChild(i).gameObject.GetComponent<MeshRenderer>().material = redCapsuleMaterial;
                // gameObject.AddComponent<Rigidbody>();
                // GetComponent<Rigidbody>().isKinematic = false;
                // GetComponent<Rigidbody>().useGravity = true;
                // GetComponent<Rigidbody>().AddForce(transform.up * 500f);
                if (pegHits.Count == numPegs)
                {
                    transform.GetChild(0).GetComponent<Animator>().enabled = true;
                    if(!isEnemyShip)
                    {
                        transform.GetChild(0).GetComponent<Animator>().SetTrigger("ShipDeath");
                        bool gameover = true;
                        foreach (Transform ship in tilesManager.ships)
                        {
                            var theController = ship.GetComponent<ShipController>();
                            if (theController.numPegs != theController.pegHits.Count)
                            {
                                gameover = false;
                            }
                        }
                        if(gameover)
                        {
                            gameOverCanvas.enabled = true;
                            timmyGuessManager.enabled = false;
                        }
                    } else
                    {
                        bool gameover = true;
                        foreach (Transform ship in transform.parent)
                        {
                            var theController = ship.GetComponent<ShipController>();
                            if (theController.numPegs != theController.pegHits.Count)
                            {
                                gameover = false;
                            }
                        }
                        if (gameover)
                        {
                            winningCanvas.GetComponent<GameOverManager>().DoWin();
                            audiomanager.Play("Victory", false);
                            // timmyGuessManager.enabled = false;
                        }
                    }
                    shouldClearCoor = true;
                }
                if(isEnemyShip)
                {
                    // trigger explosion on enemy board
                    PegSpots.transform.GetChild(i).GetChild(1).gameObject.SetActive(true);
                }
            }
            i = i + 1;
        }
        if(shouldClearCoor)
        {
            ClearShipCoor();
        }
    }

    public void ClearShipCoor()
    {
        if (!isEnemyShip)
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
