using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGameManager : MonoBehaviour
{
    public TilesAttackManager tilesAttackManager;
    public AdjustMusicVolume adjustMusicVolume;
    public AudioClip startGameMusic;
    public void SkipTutorialsStart()
    {
        tilesAttackManager.round = 1;
        tilesAttackManager.StartRound();
        GetComponent<Canvas>().enabled = false;
        adjustMusicVolume.audioSource.Stop();
        adjustMusicVolume.audioSource.clip = startGameMusic;
        adjustMusicVolume.audioSource.Play();
    }

    public void ChangeMusic()
    {
        adjustMusicVolume.audioSource.Stop();
        adjustMusicVolume.audioSource.clip = startGameMusic;
        adjustMusicVolume.audioSource.Play();
    }
}
