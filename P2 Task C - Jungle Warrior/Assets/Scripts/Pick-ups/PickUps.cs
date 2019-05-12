using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// This class manages the behaviours for all pick-ups within the game.
// The ammo and health pick-ups are handles within this script.
// AddAmmo(), AddHealth() are included within this script.
public class PickUps : MonoBehaviour {

    PlayerScript playerScript;                            // Reference for the PlayerScript script. 

    public AudioClip collected;                           // Reference to the audio clip which will play when a pick up is collected.

    public int pickupAmmo = 25;                           // The amount of ammo that will be collected.
    [HideInInspector] public int pickupHealth = 50;       // The amount of health that will be collected.


    // Use this for initialization
    void Start () {

        // Set up a reference
        playerScript = GameObject.Find("Player").GetComponent<PlayerScript>();
    }
	
	// Update is called once per frame
	void Update () {
        
    }

    void OnTriggerEnter(Collider other) {        

        // Check if the collider of the other gameObject is called player and the object attached to this script is named Ammo PickUp.
        if (other.gameObject.name == "Player" && gameObject.name == "Ammo PickUp") {
            AudioManager.instance.PlayerPlayClip(collected);            // The pick ups collected clip plays.
            GetComponent<Collider>().enabled = false;                   // Disable the collider to eliminate further collisions.
            AddAmmo(pickupAmmo);                                        // Execute the AddAmmo function passing the pickUpAmmo in its parametre.
            gameObject.GetComponentInChildren<ParticleSystem>().Play(); // Play the particle effect to indicate this pick up has been collected.  
            Destroy(gameObject,1.5f);                                   // Destroy the pick up after one and a half seconds.
        }

        // If the collider of the other gameObject is called player and the object attached to this script is named Health PickUp.
        if (other.gameObject.name == "Player" && gameObject.name == "Health PickUp") {
            AudioManager.instance.PlayerPlayClip(collected);            // The pick ups collected clip plays.
            GetComponent<Collider>().enabled = false;                   // Disable the collider to eliminate further collisions.
            AddHealth(pickupHealth);                                    // Execute the AddHealth function passing the pickUpHealth in its parametre.
            gameObject.GetComponentInChildren<ParticleSystem>().Play(); // Play the particle effect to indicate this pick up has been collected. 
            Destroy(gameObject,1.5f);                                   // Destroy the pick up after one and a half seconds.
        }

    }

    // Used for adding ammunition to the players' ammunition inventory.
    public void AddAmmo(int pickupAmmo) {

        try {
            playerScript.playerAmmo += pickupAmmo;                      // Add the ammo pick up value to the players' current ammo quantity.
        } catch {
            Debug.LogError("In PickUps Script - AddAmmo()");            // Error handling detailing the problem lies in the PickUps Script, within the AddAmmo function.
        }
    }

    // Used for adding health to the players' health inventory.
    public void AddHealth(int pickupHealth) {

        try {
            playerScript.playerHealth += pickupHealth;                  // Add the health pick up value to the players' current health quantity.
        } catch {
            Debug.LogError("In PickUps Script - AddHealth()");          // Error handling detailing the problem lies in the PickUps Script, within the AddHealth function.
        }
        // If the players health becomes greater than 100.
        if (playerScript.playerHealth > 100) {          
            playerScript.playerHealth = 100;                            // Set the health of the player to 100.
        }
    }
}
