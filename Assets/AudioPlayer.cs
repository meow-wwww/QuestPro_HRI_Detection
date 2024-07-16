using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioPlayer : MonoBehaviour
{
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponentInChildren<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayAudio(String audioClipName, bool loop = false)
    {
        AudioClip audioClip = Resources.Load<AudioClip>(audioClipName);
        audioSource.clip = audioClip;
        if (audioSource != null && audioSource.clip != null)
        {
            if (loop)
                audioSource.loop = true;
            audioSource.Play();
        }
        else
        {
            Debug.LogError("AudioSource or AudioClip not set");
        }
    }

    public void StopAudio()
    {
        audioSource.Stop();
    }
}
