using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public GameObject explosion;
    public Material blueMaterial;
    public Material darkBlueMaterial;
    public GameObject PegCollider;
    public bool doNotCreatePeg;
    public bool doNotAddPeg;
    private TilesManager tilesManager;

    public (int, int) tilePos;
    private ParticleSystem explosionParticleSystem;
    private TilesAttackManager tilesAttackManager;
    private AudioManager audiomanager;
    public TutorialManager tutorialManager;

    private void Start()
    {
        tilesManager = transform.parent.GetComponent<TilesManager>();
        tilesAttackManager = transform.parent.GetComponent<TilesAttackManager>();
        explosionParticleSystem = explosion.transform.GetChild(0).GetComponent<ParticleSystem>();
        audiomanager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
    }

    public void playDanger()
    {
        if (tilesAttackManager.inAttackRound || tutorialManager.inTutorial) { 
            audiomanager.Play("Danger", true);
        }
    }
    public void doExplosion()
    {
        audiomanager.Stop("Danger");
        if (tilesAttackManager.inAttackRound || tutorialManager.inTutorial)
        {
            explosion.SetActive(true);
        }
        
        ShipController shipController = tilesManager.getShipControllerIfActiveCoord(tilePos.Item1, tilePos.Item2);
        tilesAttackManager.attackingList.Remove((tilePos.Item1, tilePos.Item2));
        if(tilesAttackManager.inAttackRound)
        {
            if (!doNotAddPeg)
            {
                tilesAttackManager.EnemyPegsRemaining.GetChild(tilesAttackManager.pegsAdded).gameObject.SetActive(false);
                tilesAttackManager.pegsAdded = tilesAttackManager.pegsAdded + 1;
                if (tilesAttackManager.pegsAdded == tilesAttackManager.numPegsForRound)
                {
                    tilesAttackManager.inAttackRound = false;
                    tilesAttackManager.FinishRound();
                }
            }
            else
            {
                doNotAddPeg = false;
            }
        }


        if (shipController != null && !tutorialManager.inTutorial)
        {
            transform.GetChild(0).GetComponent<MeshRenderer>().material = darkBlueMaterial;
            if (tilesAttackManager.inAttackRound) { 
                shipController.DoHit((tilePos.Item1, tilePos.Item2));
            }
        } else
        {
            if(shipController != null)
            {
                tutorialManager.messedUpMissingExplosive = true;
            }
            tutorialManager.didAvoidExplosion = true;
            transform.GetChild(0).GetComponent<MeshRenderer>().material = blueMaterial;
            if (tilesAttackManager.inAttackRound || tutorialManager.inTutorial)
            {
                if (doNotCreatePeg)
                {
                    doNotCreatePeg = false;
                }
                else
                {
                    PegCollider.SetActive(true);
                    if(tutorialManager.inTutorial) {
                        PegCollider.GetComponent<Animator>().enabled = false;
                    } else
                    {
                        PegCollider.GetComponent<Animator>().enabled = true;
                    }
                }
            }
        }
        transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
        if (tilesAttackManager.inAttackRound || tutorialManager.inTutorial)
        {
            audiomanager.Play("Explosion", false);
        }
    }

    private void Update()
    {
        if (explosion.activeSelf)
        {
            if (!explosionParticleSystem.isPlaying)
            {
                explosion.SetActive(false);
            }
        }
    }
}
