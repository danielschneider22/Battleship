using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public GameObject explosion;
    public Material blueMaterial;
    public Material darkBlueMaterial;
    private TilesManager tilesManager;

    public (int, int) tilePos;
    private ParticleSystem explosionParticleSystem;

    private void Start()
    {
        tilesManager = transform.parent.GetComponent<TilesManager>();
        explosionParticleSystem = explosion.transform.GetChild(0).GetComponent<ParticleSystem>();
    }
    public void doExplosion()
    {
        explosion.SetActive(true);
        if (tilesManager.isOtherShipCoord(tilePos.Item1, tilePos.Item2))
        {
            transform.GetChild(0).GetComponent<MeshRenderer>().material = darkBlueMaterial;
        } else
        {
            transform.GetChild(0).GetComponent<MeshRenderer>().material = blueMaterial;
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
