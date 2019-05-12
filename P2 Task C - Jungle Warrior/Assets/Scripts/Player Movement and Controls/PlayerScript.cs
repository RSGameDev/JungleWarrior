using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// This class contains attributes and operations for the player in the game.
// The players health, shooting attributes, audioclips are included in this script. 
// Reload(), DamageTaken(), GameOver() functions are all included in this script.
public class PlayerScript : MonoBehaviour {

    [HideInInspector] public string playingScene;   // The name of the current scene being played.
    [HideInInspector] public int playingSceneIndex; // The index number for the current scene being played.

    public int playerHealth = 100;                  // The current health for the player.
        
    public int playerAmmo = 50;                     // The total ammo for the player.
    public int playerCurrentAmmo = 5;               // The current ammo the players' gun has.

    int ammoCounter;                                // The amount of bullers the players' gun contains.
    
    public Text healthText;                         // Reference to the text that displays the health value for the player.
    public Text ammoText;                           // Reference to the text that displays the ammo value for the player.
    public Text reloadingText;                      // Reference to the text that displays when the player is reloading.    
    public Text noAmmoText;                         // Reference to the text that displays when the player has no ammunition.

    public float timePerShot = 0.15f;               // How long the player waits before they can shoot again.    
    float timer;                                    // A general timer for the game.

    public AudioClip shoot;                         // Reference to the audio clip which will play when the player is shot.
    public AudioClip hitByBullet;                   // Reference to the audio clip which will play when the player is hit by a bullet.
    public AudioClip deathClip;                     // Reference to the audio clip which will play when the player has been killed.
    public AudioClip reload;                        // Reference to the audio clip which will play when the player is reloading.    
    public AudioClip gameOver;                      // Reference to the audio clip which will play when it is game over.

    EnemyScript enemy;                              // Reference to EnemyScript.
    GameObject deathScreen;                         // Reference to the death screen GameObject.

    [HideInInspector] public bool isPlayerDead = false; // A flag that indicates if the player is dead, set at false. 
    bool isReloading = false;                           // A flag that indicates if the player is reloading, set at false. 
    bool isNoAmmo = false;                              // A flag that indicates when the player has no more bullets in his weapon.

    void Awake() {

        // Setting a reference
        isPlayerDead = false;
    }

    // Use this for initialization
    void Start () {

        // Setting a reference
        playingScene = SceneManager.GetActiveScene().name;
        playingSceneIndex = SceneManager.GetActiveScene().buildIndex;                   

        // Setting a reference
        deathScreen = GameObject.Find("Death Screen");
        deathScreen.SetActive(false);                           // Disable the red overlay screen until it is required.
        
    }

    // Update is called once per frame
    void Update() {         

        ammoText.text = playerCurrentAmmo.ToString() + " / " + playerAmmo.ToString(); // Display the ammo in the players' clip and the ammo in total.
        healthText.text = "" + playerHealth.ToString();         // Display the health of the player.

        timer += Time.deltaTime;                                // Timer will keep track of the time since game start. To help with how often the player can shoot.


        // If the left mouse button is pressed and the player is reloading with ammo still available.
        if (Input.GetMouseButtonDown(0) && (isReloading == true && playerCurrentAmmo > 0)) {

            reloadingText.GetComponent<Text>().enabled = true;  // Display the reloading text on the screen.
        }

        //If the left mouse button is pressed and has no more ammunition in possession.
        if (Input.GetMouseButtonDown(0) && isNoAmmo == true) {

            noAmmoText.GetComponent<Text>().enabled = true;     // Display the no ammo left text on the screen.
            StartCoroutine(AmmoTextOff());                      // Start the coroutine to remove the previous text from the screen after a short delay.
        }

        // Check if the the left mouse button is pressed while the player has ammunition and is allowed to shoot. While the player is not reloading.        
        if (Input.GetMouseButtonDown(0) && playerCurrentAmmo > 0 && timer >= timePerShot && !isReloading) {

            timer = 0f;                                         // Reset the timer in preparation for when the next shot will be fired.
            playerCurrentAmmo--;                                // Decrement the players' ammo by one.
            ammoCounter++;                                      // Increment the ammo counter by one. This is so the script knows when a reload needs to occur where appropriate.
            AudioManager.instance.PlayerPlayClip(shoot);        // The player shoot clip plays.
            try {
                GetComponentInChildren<Projectile>().PlayerFire();  // The player shoots. Using the PlayerFire function found in the Projectile script located in a child Object.
            } catch {
                Debug.LogError("In PlayerScript - <Projectile>().PlayerFire()");    // Error handling detailing the problem lies in the PlayerScript. Within the PlayerFire function found in the Projectile Script.
            }
        }

        // If the the left mouse button is pressed when the player has no ammo in their gun. Whilst they are permitted to shoot and are not currently reloading.
        if (Input.GetMouseButtonDown(0) && playerCurrentAmmo <= 0 && timer >= timePerShot && isReloading == false) {

                isReloading = true;                             // Set the is reloading flag to true.

            // If the has more than or equal to five bullets.
            if (playerAmmo >= 5) {
                playerCurrentAmmo = +5;                         // Reset the ammo in the players' gun cartridge to five.
                playerAmmo -= 5;                                // Deduct what was reloaded into the gun from the players total gun ammo quantity.      
                AudioManager.instance.PlayerPlayClip(reload);   // The player reload clip plays.
                StartCoroutine(Reload());                       // Start the Reload coroutine.
            }
        }

        

        // If the R key is pressed. This is to reload
        if (Input.GetKeyDown(KeyCode.R)) {

            // If the has more than or equal to five bullets in inventory. Also while not having a full ammo clip already.
            if (playerAmmo >= 5 && playerCurrentAmmo <= 4) {
                playerCurrentAmmo = +5;                         // Reset the ammo in the players' gun cartridge to five.
                playerAmmo -= 5;                                // Deduct what was reloaded into the gun from the players total gun ammo quantity.      
                ammoCounter = 0;                                // Reset the ammo counter ad the player has a full ammo cartridge again.
                AudioManager.instance.PlayerPlayClip(reload);   // The player reload clip plays.
                return;
            // If there are no bullets in the gun AND there are no more bullets in the inventory also.
            } else if (playerCurrentAmmo <= 0 && playerAmmo <= 0) {
                noAmmoText.GetComponent<Text>().enabled = true; // Displays the has no ammo text on the screen.
                return;
            }
        }

        // If the player has no ammunition in their gun or in general.
        if (playerCurrentAmmo <= 0 && playerAmmo <= 0) {
            isNoAmmo = true;                                    // Set the, player has no ammunition flag to true.
        }

        // If the player has ammunition in their gun.
        if (playerCurrentAmmo > 0) {
            isNoAmmo = false;                                   // Set the, player has no ammunition flag to false.
        }      

    }    


    IEnumerator AmmoTextOff() { 
        yield return new WaitForSeconds(2f);                    // A delay of two seconds occurs.
        noAmmoText.GetComponent<Text>().enabled = false;        // The no ammunition text is removed from the screen.  
    }

    IEnumerator Reload() {        
        yield return new WaitForSeconds(4f);                    // A delay of four seconds occurs.
        isReloading = false;                                    // The is reloading flag is switched to false now it has finished reloading.
        reloadingText.GetComponent<Text>().enabled = false;     // The reloading text is removed from the screen.  
        ammoCounter = 0;                                        // Reset the ammo counter ad the player has a full ammo cartridge again.
    }
    
            
    // Used for when the player has takes damages.
    public void DamageTaken(int damage) {

        // Check if the players' health is more than zero.
        if (playerHealth > 0) {
            AudioManager.instance.PlayerPlayClip(hitByBullet);  // The hit by a bullet clip is played.
        }

        playerHealth -= damage;                                 // Deduct the damage of the bullet value from the current health of the player.
        Debug.Log("player health: " + playerHealth);            // Outputs to the Unity console the value for the 'playerHealth' variable. This aided me during development of the game.

        // If the current health of the player is less than or equal to zero.
        if (playerHealth <= 0) {

            try {
                GameOver();                                     // Execute the GameOver function.
            } catch {
                Debug.LogError("In PlayerScript - GameOver()"); // Error handling detailing the problem lies in the PlayerScript, within the GameOver function.
            }
        }
    }

    // Used for when the player has been defeated.
    public void GameOver() {

        StartCoroutine(Defeated());                             // Start the Defeated coroutine.
    }

    IEnumerator Defeated() {
        AudioManager.instance.PlayerPlayClip(deathClip);                        // Play the deathClip now the player has died.
        yield return new WaitForSeconds(0.3f);                                  // There will be a delay of 0.3 seconds so the death clip is not interrupted.
        isPlayerDead = true;                                                    // The is player dead flag is seitched to true. To enable certain operations within the script to activate if required.        

        GetComponent<PlayerScript>().enabled = false;                           // Disable the PlayerScript to stop these functions as well.
        GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = false; // Disable the FirstPersonController script so the player can no longer move.
        GetComponent<CharacterController>().enabled = false;                    // Disable the CharacterController.
        GetComponent<Rigidbody>().detectCollisions = false;                     // Disable collisions so the enemy can pass through the player.

        AudioManager.instance.musicSource.Stop();                               // Stop playing the music track to indicate to the player their game is over.
        AudioManager.instance.PlayerPlayClip(gameOver);                         // Play the gameOver clip. Without music occuring too this helps the 2 clips not clashing.
        AudioManager.instance.musicSource.PlayDelayed(7f);                      // Restart the music track after 7 seconds.
        GameObject.Find("Menu System").GetComponent<MenuControls>().TryAgain(); // Execute the TryAgain function from the MenuControls Script. This is so the current scene name can be saved if the player wants to replay the level.
        deathScreen.SetActive(true);                                            // Activates the death screen game object.
        yield return new WaitForSeconds(7f);                                    // The red death screen overlay will remain for seven seconds.
        GameManager.instance.HighScore();                                       // Execute the HighScore function in the GameManager script.
        SceneManager.LoadScene("Game Over Menu");                               // Load the scene called Game Over Menu.            
    }           
}
