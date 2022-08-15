using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectPeg : MonoBehaviour
{
    private TimmyGuessManager timmyGuessManager;
    public ParticleSystem pickupParticleSystem;
    private AudioManager audiomanager;

    private void Start()
    {
        timmyGuessManager = GameObject.FindGameObjectWithTag("UI Tiles").GetComponent<TimmyGuessManager>();
        audiomanager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name.Contains("Jammo"))
        {
            timmyGuessManager.IncreasePegsHeld();
            gameObject.SetActive(false);
            pickupParticleSystem.transform.position = transform.position;
            pickupParticleSystem.Play();
            audiomanager.Play("CollectPeg", false);
        }
    }

    public void DisableIfTooLong()
    {
        gameObject.SetActive(false);
    }

}
