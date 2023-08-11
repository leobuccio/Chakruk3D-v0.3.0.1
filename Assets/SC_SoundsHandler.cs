using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SC_SoundsHandler : MonoBehaviour
{
    [SerializeField] private AudioSource okeySound;
    [SerializeField] private AudioSource highlightSound;

    [SerializeField] private readonly AudioClip[] okeySounds;
    [SerializeField] private readonly AudioClip[] highlightSounds;

    public void Start()
    {
        okeySound = gameObject.AddComponent<AudioSource>();
        highlightSound = gameObject.AddComponent<AudioSource>();

        okeySound.playOnAwake = false;
        highlightSound.playOnAwake = false;

        okeySound.clip = okeySounds[0];
        highlightSound.clip = highlightSounds[0];
    }

    public void PlayOkey()
    {
        
        okeySound.Play();
    }

    public void PlayHighlight()
    {
        
        highlightSound.Play();
    }

}
