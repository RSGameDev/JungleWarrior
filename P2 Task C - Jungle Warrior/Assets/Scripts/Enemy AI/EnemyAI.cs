using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/****************************************************
 * Title: Patrol class (adapted from)
 * Author: Unity Learn Manual Documentation 
 * Availability: https://docs.unity3d.com/Manual/nav-AgentPatrol.html
 ****************************************************/
/****************************************************
* Title: Enemy Sight class (adapted from)
* Author: Official Unity Youtube channel  
* Availability: https://www.youtube.com/watch?v=mBGUY7EUxXQ
****************************************************/

// This class manages the navigation and shooting mechanics for the enemy objects within the game.
// The enemy Reload() function can be found within this script.
public class EnemyAI : MonoBehaviour {

    public GameObject player;                   // Reference to the player GameObject.
    Transform playerTransform;                  // Reference to the player Transform.

    [HideInInspector] public float fieldOfViewAngle = 110f;       // The field of view that the enemy has.
    SphereCollider col;                                           // Reference to the enemies SphereCollider

    NavMeshAgent nav;                           // Reference to the enemies NavMeshAgent.
    public Transform[] points;                  // A reference to the enemies points they can travel to.
    int destPoint = 0;                          // The initial value for the destination point of the enenmy.

    float timer;                                // A general timer for the game.
    
    public AudioClip shoot;                     // Reference to the audio clip which will play a shoot audio clip.
    public AudioClip reload;                    // Reference to the audio clip which will play a reload audio clip.

    public int ammo = 4;                        // The amount of ammunition the enemy starts with.
    bool isReload = false;                      // Is the enemy currently reloading.
    
    PlayerScript playerScript;                  // Reference to the PlayerScript. 
    bool isPlayerDeadAccess;                    // Access to the isPlayerDead variable in PlayerScript.
    
    public float timePerShot = 1.5f;            // How long the enemy waits before they can shoot again.


    void Awake() {

        // Setting up the references
        nav = GetComponent<NavMeshAgent>();     
        col = GetComponent<SphereCollider>();
    }
    // Use this for initialization
    void Start() {
              
        GotoNextPoint();

        // Setting up the references
        playerTransform = player.GetComponent<Transform>();

        playerScript = player.GetComponent<PlayerScript>();
        isPlayerDeadAccess = playerScript.isPlayerDead;

    }

    // Update is called once per frame
    void Update() {
        
        isPlayerDeadAccess = playerScript.isPlayerDead;         // Access isPlayerDead variable from PlayerScript.

        timer += Time.deltaTime;                                // Timer will keep track of the time since game start. To help with how often the enemy can shoot.

        // Choose the next destination point when the agent
        // gets close to the current one.
        if (!nav.pathPending && nav.remainingDistance < 0.5f)         
            GotoNextPoint();
    }

    /**********************************************************************************
    * Adapted code using the Enemy Sight class from Unitys' official Youtube channel
    ***********************************************************************************/

    void OnTriggerStay(Collider other) {

        // Check if the other colliders' gameObject has the player reference attached to it.
        if (other.gameObject == player) {             

            // Setting up the references
            Vector3 direction = other.transform.position - transform.position;
            float angle = Vector3.Angle(direction, transform.forward);

            // If the angle is less than half the field of view
            if (angle < fieldOfViewAngle * 0.5f) {                 

                RaycastHit hit;

                // If the raycast hits anything within it's shooting range (it's sphere collider).
                if (Physics.Raycast(transform.position, direction.normalized, out hit, col.radius)) {

                    // If the RaycastHit collides with a gameObject that has the player reference attached to it.
                    if (hit.collider.gameObject == player) {                        
                                                
                        transform.LookAt(playerTransform);                                          // Rotate the enemy so it faces the players current position.

                        // Calculate a new path at the position of the player gameObject.
                        nav.SetDestination(player.transform.position);                        

                        // If the timer is greater than allowed time to shoot and still has ammunition whilst the player is still alive.
                        if (timer >= timePerShot && ammo > 0 && !isPlayerDeadAccess) {
                            timer = 0f;                                                             // Reset the timer to zero in preparation for the next shot.
                            ammo--;                                                                 // Decrement 1 from the enemies ammo variable.
                            AudioManager.instance.EnemyPlayClip(shoot);                             // Play the enemy shoot clip. Using the function found within thee AudioManager script.
                            try {
                                GetComponentInChildren<Projectile>().EnemyFire();                   // The enemy shoots. Using the EnemyFire function found in the Projectile script located in a child Object.
                            } catch {
                                Debug.LogError("In EnemyAI Script - <Projectile>().EnemyFire()");   // Error handling detailing the problem lies in the EnemyAI Script. Within the EnemyFire function found in the Projectile Script.
                            }
                            return;

                        // If the timer is greater than allowed time to shoot but has no ammunition whilst the player is still alive. Plus the is not reloading.
                        } else if (timer >= timePerShot && ammo <= 0 && !isPlayerDeadAccess && isReload == false) {
                            isReload = true;                                                        // Set the is reloading flag to true.
                            try {
                                StartCoroutine(Reload());                                           // Start the Reload coroutine.
                            } catch {
                                Debug.LogError("In EnemyAI Script - StartCoroutine(Reload())");     // Error handling detailing the problem lies in the EnemyAI Script, within the Reload coroutine.
                            }
                            return;                         
                        }
                    }

                }
            }
        }        
    }

    /*End of adapted code**************************************************************/

    IEnumerator Reload() {
        nav.speed = 0f;                                 // The enemy comes to a halt in order to reload.
        AudioManager.instance.EnemyPlayClip(reload);    // The enemy reload sound clip plays.
        yield return new WaitForSeconds(4);             // A delay of 4 second occurs.
        nav.speed = 3.5f;                               // The enemy comes back to normal speed again.
        ammo = 5;                                       // The enemy has a full ammunition once again.
        isReload = false;                               // The is reloading flag is switched to false now it has finished reloading.
    }

    // Used to determine the next destination.
    void GotoNextPoint() {

        try {
            // Returns if no points have been set up
            if (points.Length == 0)
                return;

            // Set the agent to go to the currently selected destination.
            nav.destination = points[destPoint].position;

            // Choose the next point in the array as the destination, cycling to the start if necessary.
            destPoint = (destPoint + 1) % points.Length;
        } catch {
            Debug.LogError("In EnemyAI Script - GotoNextPoint()");  // Error handling detailing the problem lies in the EnemyAI Script, within the GotoNextPoint function.      
        }  
    }
}   
    
