using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSoundManager : MonoBehaviour
{
    //Clips
    private AudioClip music;
    private AudioClip[] ambientSFX;

    //Sources
    private AudioSource musicTrack, ambientTrack;


    // Use this for initialization
    private void Start()
    {
        music = AudioManager.Instance.GetAudioClip("Music name goes here");
        ambientSFX = AudioManager.Instance.GetAudioClips("ambient or something");

        ambientTrack.clip = ambientSFX[Random.Range(0, ambientSFX.Length - 1)];
        ambientTrack.loop = false;
        ambientTrack.Play();
        musicTrack.clip = music;
        musicTrack.loop = true;
        musicTrack.Play();
    }

    private void Update()
    {
        if (!ambientTrack.isPlaying)
        {
            AudioClip newClip;
            do
            {
                newClip = ambientSFX[Random.Range(0, ambientSFX.Length - 1)];
            }
            while (ambientTrack.clip == newClip);

            ambientTrack.Play();
        }
    }
}
