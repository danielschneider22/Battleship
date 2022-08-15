using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public GameObject explosion;
    public Material blueMaterial;
    public Material darkBlueMaterial;
    public GameObject PegCollider;
    private TilesManager tilesManager;

    public (int, int) tilePos;
    private ParticleSystem explosionParticleSystem;
    private TilesAttackManager tilesAttackManager;

    private void Start()
    {
        tilesManager = transform.parent.GetComponent<TilesManager>();
        tilesAttackManager = transform.parent.GetComponent<TilesAttackManager>();
        explosionParticleSystem = explosion.transform.GetChild(0).GetComponent<ParticleSystem>();
    }
    public void doExplosion()
    {
        explosion.SetActive(true);
        ShipController shipController = tilesManager.getShipControllerIfActiveCoord(tilePos.Item1, tilePos.Item2);
        tilesAttackManager.attackingList.Remove((tilePos.Item1, tilePos.Item2));
        tilesAttackManager.EnemyPegsRemaining.GetChild(tilesAttackManager.pegsAdded).gameObject.SetActive(false);
        tilesAttackManager.pegsAdded = tilesAttackManager.pegsAdded + 1;
        if (tilesAttackManager.pegsAdded == tilesAttackManager.numPegsForRound)
        {
            tilesAttackManager.inAttackRound = false;
            tilesAttackManager.FinishRound();
        }
        

        if (shipController != null)
        {
            transform.GetChild(0).GetComponent<MeshRenderer>().material = darkBlueMaterial;
            shipController.DoHit((tilePos.Item1, tilePos.Item2));
        } else
        {
            transform.GetChild(0).GetComponent<MeshRenderer>().material = blueMaterial;
            PegCollider.SetActive(true);
        }
        transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
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
