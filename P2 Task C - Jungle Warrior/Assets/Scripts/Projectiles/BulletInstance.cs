using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class carries the characteristics of the bullet instances.
// Such as the damage the projectile makes when shot by the enemy and when shot by the player.
public class BulletInstance : MonoBehaviour {
  
    public int damageToEnemy = 34;         // The damage a bullet can take off from the enemies health.
    public int damageToPlayer = 10;        // The damage a bullet can take off from the players health. 

    // Use this for initialization
    void Start () {
        
        // For ever GameObject with the tag Enemy attached to it.
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy")) {
            // Ignore the range collider for the enemy otherwise the bullet will get destroyed before having a chance to reaching the enemy.
            Physics.IgnoreCollision(enemy.GetComponent<SphereCollider>(), GetComponent<Collider>());
        }

        foreach (GameObject waterWall in GameObject.FindGameObjectsWithTag("Water collider")) {
            // Ignore the water wall collider so that projectiles can be shot over water.
            Physics.IgnoreCollision(waterWall.GetComponent<BoxCollider>(), GetComponent<Collider>());
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider other) {

        // Check if the other gameObject from the other collider is called Player.
        if (other.gameObject.name == "Player") {

            GetComponent<SphereCollider>().enabled = false;                                     // Disable the bullets collider.
            try {
                other.GetComponent<PlayerScript>().DamageTaken(damageToPlayer);                 // Execute the damageTaken function in the PlayerScript due to being hit.
            } catch {
                Debug.LogError("In BulletInstance Script - <PlayerScript>().DamageTaken()");    // Error handling detailing the problem lies in the BulletInstance Script. Within the DamageTaken function found in the PlayerScript.           
            }
            GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);                          // Stop the momentum of the bullet due to having made contact.
            GetComponent<MeshRenderer>().enabled = false;                                       // Disable the visuals of the bullet.
            gameObject.GetComponentInChildren<ParticleSystem>().Play();                         // Play the particle effect to indicate the bullet had made contact. 
            Destroy(gameObject);                                                                // Destroy the bullet instance.

        // Check if the other gameObject from the other collider is called Enemy and this gameObject (not the other) has a Player bullet tag.
        } else if (other.gameObject.name == "Enemy" && tag == "Player bullet") { 
           
            GetComponent<SphereCollider>().enabled = false;                                     // Disable the bullets collider.
            try {
                other.GetComponent<EnemyScript>().DamageTaken(damageToEnemy);                   // Execute the damageTaken function in the EnemyScript due to being hit.
            } catch {
                Debug.LogError("In BulletInstance Script - <EnemyScript>().DamageTaken()");     // Error handling detailing the problem lies in the BulletInstance Script. Within the DamageTaken function found in the EnemyScript.  
            }
            GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);                          // Stop the momentum of the bullet due to having made contact.
            GetComponent<MeshRenderer>().enabled = false;                                       // Disable the visuals of the bullet.
            gameObject.GetComponentInChildren<ParticleSystem>().Play();                         // Play the particle effect to indicate the bullet had made contact. 
            Destroy(gameObject);                                                                // Destroy the bullet instance.
        }

        Destroy(gameObject);                                                                    // Destroy the bullet instance.
    }    
}
