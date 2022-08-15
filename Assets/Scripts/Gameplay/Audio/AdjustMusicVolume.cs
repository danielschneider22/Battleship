using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdjustMusicVolume : MonoBehaviour
{
    public AudioSource audioSource;
    public Slider slider;

    float origVolume;

    private void Awake()
    {
        origVolume = audioSource.volume;
        audioSource.volume = slider.value * origVolume;
    }

    public void ChangeMusicVolume()
    {
        audioSource.volume = slider.value * origVolume;
    }
}
