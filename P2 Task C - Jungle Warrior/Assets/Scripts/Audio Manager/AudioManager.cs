using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*******************************************************
/ Title: SoundManager class (adapted from) 
/ Author: Unity Learn Tutorial site
/ Availability: https://unity3d.com/learn/tutorials/projects/2d-roguelike-tutorial/audio-and-sound-manager
********************************************************/

// This class handles all the audio for the game.
public class AudioManager : MonoBehaviour {

    public AudioSource musicSource;             // Reference to the audio source which will play the music.
    public AudioSource playerfxSource;          // Reference to the audio source which will play the player effects.
    public AudioSource enemyfxSource;           // Reference to the audio source which will play the enemy effects.
    public static AudioManager instance = null; // Allows other scripts to call functions from AudioManager.

    public AudioClip track1;                    // Reference to the audio clip which will play a music track. 
    public AudioClip track2;                    // Reference to the audio clip which will play a music track.
    public AudioClip track3;                    // Reference to the audio clip which will play a music track.

    void Awake() {

        // Check if there is already an instance of AudioManager
        if (instance == null)
            instance = this;                    // If not, set it to this.

        //If instance already exists.
        else if (instance != this)
            Destroy(gameObject);                // Destroy this, so there can only be one instance of AudioManager.

        DontDestroyOnLoad(gameObject);          // Set AudioManager to DontDestroyOnLoad so it won't be destroyed when reloading scenes.
    }

    void Start() {

        MusicClip(track1);                      // Play the clip.
    }

    void Update() {

        // To switch between audio tracks: 
        // Check if the key pressed down is the alpha1 key
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            MusicClip(track1);                              // Play the clip for track1.

        // If the key pressed down is the alpha2 key
        } else if ((Input.GetKeyDown(KeyCode.Alpha2))) {
            MusicClip(track2);                              // Play the clip for track2.

        // If the key pressed down is the alpha3 key
        } else if ((Input.GetKeyDown(KeyCode.Alpha3))) {
            MusicClip(track3);                              // Play the clip for track3.        
        }
    }    
    
    // Used to play single music tracks.
    public void MusicClip(AudioClip clip) {

        try {
            musicSource.clip = clip;                                     // Set the clip of the music source to the clip passed in as a parameter.
            musicSource.Play();                                          // Play the clip.
        } catch {
            Debug.LogError("In AudioManager Script - MusicClip()");      // Error handling detailing the problem lies in the AudioManager Script, within the MusicClip function.
        }
    }

    // Used to play single sound clips for the Player.
    public void PlayerPlayClip(AudioClip clip) {

        try {
            playerfxSource.clip = clip;                                  // Set the clip of the audio source to the clip passed in as a parameter.

            playerfxSource.Play();                                       // Play the clip.
        } catch {
            Debug.LogError("In AudioManager Script - PlayerPlayClip()"); // Error handling detailing the problem lies in the AudioManager Script, within the PlayerPlayClip function.
        }
    }

    // Used to play single sound clips for the Enemy.
    public void EnemyPlayClip(AudioClip clip) {
        try {
            enemyfxSource.clip = clip;                                   // Set the clip of the audio source to the clip passed in as a parameter.

            enemyfxSource.Play();                                        // Play the clip.
        } catch {
            Debug.LogError("In AudioManager Script - EnemyPlayClip()");  // Error handling detailing the problem lies in the AudioManager Script, within the EnemyPlayClip function.
        }
    }
}
