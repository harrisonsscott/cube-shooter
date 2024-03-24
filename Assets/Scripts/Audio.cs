using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// place in the Audio empty game object

public class Audio : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip pop; // played when blocks are destroyed
    public AudioClip hit; // played when the player dies
    public AudioClip shoot; // played when a bullet is shot
    public AudioClip coin; // played when the player picks up a coin


    private void Start() {
        audioSource = GetComponent<AudioSource>();
    }

    public void Play(AudioClip clip){ // plays an audio clip
        audioSource.clip = clip;
        audioSource.Play();
    }
}
