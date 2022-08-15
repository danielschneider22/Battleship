using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.ParticleSystem;

public class GameOverManager : MonoBehaviour
{
    private bool hasWon = false;
    private float seeVictoryTimer = 0f;
    private float seeVictoryTimer2 = 0f;
    public Button nextRoundButton;
    public GameObject EnemyTiles;
    public Transform enemyShips;
    private AudioManager audiomanager;
    public TutorialManager tutorialManager;
    public void DoQuit()
    {
        Application.Quit();
    }
    public void DoRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void DoWin()
    {
        hasWon = true;
        nextRoundButton.interactable = false;
    }
    private void Start()
    {
        audiomanager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
    }
    public void Update()
    {
        if(hasWon && seeVictoryTimer > 3f)
        {
            audiomanager.Play("Victory", false);
            EnemyTiles.SetActive(false);
            foreach(Transform child in enemyShips)
            {
                ShipController control = child.GetComponent<ShipController>();
                foreach (Transform peg in control.PegSpots.transform)
                {
                    MainModule mod = peg.GetChild(1).GetChild(0).GetComponent<ParticleSystem>().main;
                    mod.startLifetime = 3f;
                    peg.GetChild(1).GetChild(0).GetComponent<ParticleSystem>().Play();
                }
                control.transform.GetChild(0).GetComponent<Animator>().SetTrigger("ShipDeath");
                
            }
            
        }
        if (hasWon && seeVictoryTimer2 > 6f)
        {
            GetComponent<Canvas>().enabled = true;
        }
        if (hasWon)
        {
            tutorialManager.speechId = "wongame";
            seeVictoryTimer += Time.deltaTime;
            seeVictoryTimer2 += Time.deltaTime;
        }
        
    }
}
