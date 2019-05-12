using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// This class performs the event for when the player reaches the end of the level.
public class FinishLine : MonoBehaviour {


    public Text levelCompleted;     // Reference to the text component that will display once the level has been completed.

    public AudioClip finishLine;    // Reference to the audio clip which will play when the player reaches the end of the level. 

    void Awake() {             
    }

    // Use this for initialization
    void Start() {
        
        // For ever GameObejct with the tag Enemy attached to it.
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy")) {
            // Ignore the range collider for the enemy so it will not collide with the finish lines collider.
            Physics.IgnoreCollision(enemy.GetComponent<SphereCollider>(), GetComponent<Collider>());
        }
    }
	
	// Update is called once per frame
	void Update () {		
	}

    void OnTriggerEnter(Collider other) {

        other.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = false; // Disable movement for the player.

        WinLoseConditions.levelCompleted = true;                                    // Set the level completed as being true in the WinLoseConditions script.
        try {
            GameObject.Find("Menu System").GetComponent<MenuControls>().TryAgain(); // Execute the TryAgain function located in the MenuControls script found on the Menu System GameObject.
        } catch {
            Debug.LogError("In FinishLine Script - <MenuControls>().TryAgain()");   // Error handling detailing the problem lies in the FinishLine Script. Within the TryAgain function found in the MenuControls Script.
        }
        levelCompleted.GetComponent<Text>().enabled = true;                         // Display the level completed text on the screen.
        AudioManager.instance.musicSource.Stop();                                   // Stop playing the music track. 
        AudioManager.instance.PlayerPlayClip(finishLine);                           // The finish line clip plays.
        AudioManager.instance.musicSource.PlayDelayed(5f);                          // The music source plays again after a 5 second delay.
        try {
            FinishedLevel();                                                        // Execute the FinishedLevel function.
        } catch {
            Debug.LogError("In FinishLine Script - FinishedLevel()");               // Error handling detailing the problem lies in the FinishLine Script, within the FinishedLevel function.
        }
    }

    void FinishedLevel() {        
        StartCoroutine(NextLevel());            // Start the NextLevel coroutine.                                                
    }

    IEnumerator NextLevel() {

        yield return new WaitForSeconds(5f);    // A delay of 5 seconds occur.          
        SceneManager.LoadScene(5);              // The level at index 5 in the build settings will load, the Victory Screen.        
    }
}
