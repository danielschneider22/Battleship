using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    private AdjustMusicVolume adjustMusicVolume;
    private Canvas canvas;
    public GameObject MusicSlider;
    public GameObject SoundSlider;

    void Start()
    {
        canvas = GetComponent<Canvas>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) == true)
        {
            canvas.enabled = !canvas.enabled;
            if(canvas)
            {
                MusicSlider.SetActive(true);
                SoundSlider.SetActive(true);
            } else
            {
                MusicSlider.SetActive(false);
                SoundSlider.SetActive(false);
            }
        }
    }


    public void CloseMenu()
    {
        canvas.enabled = false;
        MusicSlider.SetActive(false);
        SoundSlider.SetActive(false);
    }

    public void ChangeMusicVolume()
    {
        adjustMusicVolume = GameObject.FindGameObjectWithTag("GameplayMusic").GetComponent<AdjustMusicVolume>();
        adjustMusicVolume.ChangeMusicVolume();
    }

    public void DoQuit()
    {
        Application.Quit();
    }
}
