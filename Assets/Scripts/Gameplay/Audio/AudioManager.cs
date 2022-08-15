using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    public static AudioManager instance;

    public Slider slider;
    public bool shouldDestroy;

    private void Awake()
    {

        if (!shouldDestroy)
        {
            if (instance == null)
                instance = this;
            else
            {
                DestroyImmediate(gameObject);
                return;
            }
        }
        
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.clip = s.clip;
        }
        // if(!shouldDestroy)
        // {
        //     DontDestroyOnLoad(this);
        // }
        
    }

    public void Play(string name, bool playLooped)
    {
        try{
            Sound s = Array.Find(sounds, sound => sound.name == name);
            if (s == null)
                return;
            float origVol = s.volume;
            if(slider != null)
            {
                s.source.volume = origVol * slider.value;
            }
            s.source.loop = playLooped;
            s.source.Play();
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }

    }

    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
            return;
        s.source.Stop();
    }

    public void StopAll()
    {
        foreach (Sound sound in sounds)
        {
            sound.source.Stop();
        }
    }
}
