using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectPeg : MonoBehaviour
{
    private TimmyGuessManager timmyGuessManager;

    private void Start()
    {
        timmyGuessManager = GameObject.FindGameObjectWithTag("UI Tiles").GetComponent<TimmyGuessManager>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name.Contains("Jammo"))
        {
            timmyGuessManager.IncreasePegsHeld();
            gameObject.SetActive(false);
        }
    }
}