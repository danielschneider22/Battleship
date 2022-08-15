using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdjustMusicVolume : MonoBehaviour
{
    public AudioSource audioSource;
    private Slider slider;

    float origVolume;

    private void Awake()
    {
        slider = GameObject.FindGameObjectWithTag("MusicSlider").GetComponent<Slider>();
        origVolume = audioSource.volume;
        audioSource.volume = slider.value * origVolume;
    }

    public void ChangeMusicVolume()
    {
        audioSource.volume = slider.value * origVolume;
    }
}
