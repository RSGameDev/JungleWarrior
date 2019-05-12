using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// This class handles the characteristics for the enemies in the game.
// Such as health, score value, audioclips. 
// DamageTaken(), EnemyScore(), Defeated() are included in this class.
public class EnemyScript : MonoBehaviour {
    
    public int startHealth = 100;                   // The amount of health the enemy starts with.
    public int currentHealth;                       // The curren health of the enemy.
    public int score = 50;                          // The score value when an enemy is defeated.
    
    public AudioClip damageClip;                    // Reference to the audio clip which will play when the enemy is shot.
    public AudioClip deathClip;                     // Reference to the audio clip which will play when the enemy dies.        

    
    // Use this for initialization
    void Start () {

        // Setting up the references
        currentHealth = startHealth;                         // Set the initial health of the enemy.        
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    // Used for when the enemy takes damage.
    public void DamageTaken(int damage) {

        AudioManager.instance.EnemyPlayClip(damageClip);     // The enemy damage sound clip plays. 

        currentHealth -= damage;                             // Deduct the damage value from the enemies current health.

        // Check is the enemy has no or less health.
        if (currentHealth <= 0) {
            
            GetComponent<NavMeshAgent>().enabled = false;    // Disable movement for the enemy.
            GetComponent<EnemyAI>().enabled = false;         // Disable the EnemyAI script for the purpose of not allowing the enemy to shoot again.   
            GetComponent<CapsuleCollider>().enabled = false; // Remove the enemies collider to disable future collisions.
            WinLoseConditions.enemyCount--;                  // Deduct 1 from the enemy count for the level. This will update on the game display also.
            Defeated();                                      // Execute the Defeated function.
            EnemyScore(score);                               // Execute the EnemyScore function.            
                
        }
    }    

    // Used for processing the score obtained from an enemy having died.
    public void EnemyScore (int score) {

        try {
            GameManager.score += score;                      // Give the score from an enemy dieing to the score variable in the Win lose conditions script.
        } catch {
            Debug.LogError("In EnemyScript - EnemyScore()"); // Error handling detailing the problem lies in the EnemyScript, within the EnemyScore function.    
        }
    }

    // Used when an enemy has died.
    void Defeated() {

        try {
            AudioManager.instance.EnemyPlayClip(deathClip);  // Play the death audio clip as the enemy has died.            
            GetComponentInChildren<ParticleSystem>().Play(); // Play the particle effect to indicate to the player the enemy is truly defeated.  
            StartCoroutine(EnemyGameOver());                 // Start the EnemyGameOver coroutine.
        } catch {
            Debug.LogError("In EnemyScript - Defeated()");   // Error handling detailing the problem lies in the EnemyScript, within the Defeated function.  
        }
    }

    IEnumerator EnemyGameOver() {

        yield return new WaitForSeconds(1f);                 // A delay of 1 second will occur.
        Destroy(gameObject);                                 // Destroy the enemy gameObject.
    }
}
