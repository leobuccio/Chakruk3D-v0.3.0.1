using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SC_MusicHandler : MonoBehaviour
{
    [SerializeField] Slider volumeSlider;
    [SerializeField] private GameObject Audio;
    [SerializeField] private AudioSource SceneAudio;
    [SerializeField] private float soundFactor;

    public void Start()
    {
        SceneAudio = Audio.GetComponent<AudioSource>();
        volumeSlider.value = SceneAudio.volume;
        
    }

    public void ChangeVolume ()
    {
        SceneAudio.volume = volumeSlider.value * soundFactor;
    }

}
