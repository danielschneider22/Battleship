using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public GameObject brandonBox;
    public GameObject timmyBox;
    public Animator brandonAnimator;
    public Animator timmyAnimator;
    public TextMeshPro brandon;
    public TextMeshPro timmy;
    public bool didMoveShip;
    public bool didJump;
    public bool didAvoidExplosion;
    public bool didGrabPeg;
    public bool didPutPegInBasket;
    public TilesAttackManager attackManager;

    public float speechTimer = 21223429093f;

    public string speechId = "";
    public GameObject startGameCanvas;

    public StartGameManager startGameManager;
    public bool inTutorial;
    public bool messedUpMissingExplosive;

    public void StartTutorial()
    {
        startGameCanvas.SetActive(false);

        brandon.text = "Hey Loser, you wanna play battleship?";
        speechTimer = 5f;
        speechId = "timmy1";
        brandonBox.SetActive(true);
        brandonAnimator.enabled = true;
        startGameManager.ChangeMusic();
        inTutorial = true;
    }

    private void setBrandonTalking()
    {
        brandonBox.SetActive(true);
        brandonAnimator.enabled = true;
    }
    private void setTimmyTalking()
    {
        timmyBox.SetActive(true);
        timmyAnimator.enabled = true;
    }

    private void stopBrandonTalking()
    {
        brandonBox.SetActive(false);
        brandonAnimator.enabled = false;
    }
    private void stopTimmyTalking()
    {
        timmyBox.SetActive(false);
        timmyAnimator.enabled = false;
    }

    private void SpeechBasedOnId()
    {
        switch (speechId)
        {
            case "timmy1":
            {
                setTimmyTalking();
                stopBrandonTalking();
                timmy.text = "You're on! You always cheat, but this time I have a surprise up my sleeve!";
                speechTimer = 5f;
                speechId = "timmy2";
                break;
            }
            case "timmy2":
            {
                timmy.text = "Jammo! Come online! Jump for me (SPACE) to show you're operational!";
                speechTimer = 3f;
                speechId = "timmy3";
                break;
            }
            case "timmy3":
            {
                if(didJump)
                {
                    timmy.text = "Try moving some of those ships. (Grab with E, release with E). You can move ships through other ships while dragging.";
                    speechTimer = 3f;
                    speechId = "brandon2";
                }
                break;
            }
            case "brandon2":
            {
                if (didMoveShip)
                {
                    stopTimmyTalking();
                    setBrandonTalking();
                    brandon.text = "Enough fooling around dweeb! Let's see how you like a busted battleship!";
                    speechTimer = 1.5f;
                    speechId = "timmy4";
                }
                break;
            }
            case "timmy4":
            {
                setTimmyTalking();
                timmy.text = "Oh no Jammo! Get my battleship out of the way!";
                speechTimer = 3f;
                speechId = "explosionWait";
                break;
            }
            case "explosionWait":
            {
                attackManager.MakeAttack();
                speechId = "timmy5";
                break;
            }

            case "timmy5":
            {
                if(didAvoidExplosion)
                {
                    stopBrandonTalking();
                    timmy.text = messedUpMissingExplosive ? "Looks like the explosion hit! We're gonna act a bit like it didn't for the tutorial though. Please move this ship." : "Nice dodge! If you look you can see his miss left us some ammo!";
                    speechId = "timmy6";
                    speechTimer = messedUpMissingExplosive ? 8f : 6f;
                }
                break;
            }
            case "timmy6":
            {
                if (didAvoidExplosion)
                {
                    timmy.text = "Pickup the floating ammo that the missed explosion left behind! (run into the floating bubble)" ;
                    speechId = "timmy7";
                    speechTimer = 3f;
                }
                break;
            }
            case "timmy7":
            {
                if (didGrabPeg)
                {
                    timmy.text = "Perfect! Let's store that ammo. We can use it against that bully in the next round!";
                    speechId = "timmy8";
                    speechTimer = 6f;
                }
                break;
            }
            case "timmy8":
            {

                timmy.text = "Go over to the marked basket and press E to store it, so our cannons can use it next round!";
                speechId = "timmy9";
                speechTimer = 3f;
                break;
            }
            case "timmy9":
            {
                if(didPutPegInBasket)
                {
                    timmy.text = "Nice! Try and hold your own, and when he's out of ammo we will go on the offensive!";
                    speechId = "brandon10";
                    speechTimer = 2.5f;
                }
                
                break;
            }
            case "brandon10":
            {
                setBrandonTalking();
                brandon.text = "Not if I kill all your ships in the very first level!";
                speechId = "startRound";
                speechTimer = 5f;
                break;
            }
            case "startRound":
            {
                inTutorial = false;
                attackManager.StartRound();
                speechId = "clearText1";
                speechTimer = 3f;
                break;
            }
            case "clearText1":
            {
                stopBrandonTalking();
                stopTimmyTalking();
                speechId = "fake1";
                speechTimer = 3f;
                break;
            }
            case "wongame":
            {
                setTimmyTalking();
                setBrandonTalking();
                    timmy.text = "You did it! Great job!";
                    brandon.text = "I lost? How could I lose?!";
                speechTimer = 0f;
                break;
            }

        }
    }

    private void Update()
    {
        speechTimer -= Time.deltaTime;
        if(speechTimer <= 0f)
        {
            SpeechBasedOnId();
        }
    }
}
