using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.Characters.FirstPerson;

// This class contains the functionality for the menu systems within the game.
// This consists of the pre game menus and the in game menus.
public class MenuControls : MonoBehaviour {
                       
    Canvas gameUI;                         // Reference to the canvaas for the in game UI.
    Canvas inGameMenu;                     // Reference to the canvas for the in game Menu.
    string sceneName;                      // The name of the current scene. 
    static string pastLevel;               // The name of the previous scene.
    static int pastLevelIndex;             // The index number for the previous level will be stored here. 

    FirstPersonController fPController;    // Reference for the FirstPersonController script.
    PlayerScript playerScript;             // Reference for the PlayerScript script. 

    void Awake() {        
    }

    // Use this for initialization
    void Start() {

        // Setting up a reference    
        sceneName = SceneManager.GetActiveScene().name;

        // Check if the scene name is called Level 1, Level 2, Level 3, Development Level 1 (for game testing purposes), Development Level 2 (for game testing purposes).
        if (sceneName == "Level 1" || sceneName == "Level 2" || sceneName == "Level 3" || sceneName == "Development Level 1" || sceneName == "Development Level 2") {

            // Setting up references 
            playerScript = GameObject.Find("Player").GetComponent<PlayerScript>();
            gameUI = GameObject.Find("Game UI").GetComponent<Canvas>();                      
            inGameMenu = GameObject.Find("In Game Menu").GetComponent<Canvas>();
            inGameMenu.enabled = false;                                 // Disable the canvas of inGameMenu.

            // Setting up a reference 
            fPController = GameObject.Find("Player").GetComponent<FirstPersonController>();
            Time.timeScale = 1f;                                        // The actual game time is set to realtime speed.
        }

        // If the scene name is called Game Over Menu.
        if (sceneName == "Game Over Menu") {
            Cursor.visible = true;                                      // The cursor is revealed
            Cursor.lockState = CursorLockMode.Confined;                 // Brings the cursor to the game window.
            Cursor.lockState = CursorLockMode.None;                     // Removes cursor lock.
        }

        if (sceneName == "Victory Menu") {
            Cursor.visible = true;                                      // The cursor is revealed
            Cursor.lockState = CursorLockMode.Confined;                 // Brings the cursor to the game window.
            Cursor.lockState = CursorLockMode.None;                     // Removes cursor lock.
        }
    }    

    // Update is called once per frame
    void Update() {        

        // Check if the key pressed down is the Escape key.
        if (Input.GetKeyDown(KeyCode.Escape)) {
            playerScript.enabled = false;                               // Disable the players ability to shoot during the time the in game menu is on display.
            gameUI.enabled = false;                                     // Turn off the game UI.
            inGameMenu.enabled = true;                                  // Enable the inGameMenu as it is required now.
            Time.timeScale = 0f;                                        // Pause the game.
            fPController.enabled = false;                               // Disable movement of the player character.
            Cursor.visible = true;                                      // The cursor is revealed.
            Cursor.lockState = CursorLockMode.Confined;                 // Brings the cursor to the game window. ------
            Cursor.lockState = CursorLockMode.None;                     // Removes cursor lock. --------
        }        
    }

    // Used to quit the game.
    public void ExitGame() {
        Application.Quit();
    }    

    // Used for resuming the game.
    public void ResumeGame() {
        try {
            playerScript.enabled = true;                                // Enable the players ability to shoot now the game has resumed.
            gameUI.enabled = true;                                      // Turn on the game UI.
            inGameMenu.enabled = false;                                 // Disable the inGameMenu as it is no longer required now.
            Time.timeScale = 1f;                                        // Un-pause the game.
            fPController.enabled = true;                                // Enable movement of the player character.
            Cursor.visible = false;                                     // The cursor becomes invisible.
        } catch {
            Debug.LogError("In MenuControls Script - ResumeGame()");    // Error handling detailing the problem lies in the MenuControls Script, within the ResumeGame function.
        }
    }

    // Used for loading scene for the game.
    public void LoadLevel(string name) {
        try {
            SceneManager.LoadScene(name);                               // Loads the scene by name passed in through its parametre.
        } catch {
            Debug.LogError("In MenuControls Script - LoadLevel()");     // Error handling detailing the problem lies in the MenuControls Script, within the LoadLevel function.
        }
    }

    // Used for loading the next level in the game.
    public void NextLevel() {
        try {
            SceneManager.LoadScene(pastLevelIndex + 1);                 // Load the next level in the game.
        }
        catch {
            Debug.LogError("In MenuControls Script - NextLevel()");     // Error handling detailing the problem lies in the MenuControls Script, within the NextLevel function.
        }
    }

    // Used when the player is retrying a level.
    public void TryAgain() {

        // Check if the scene name is called Game Over Menu OR Victory Menu.
        if (sceneName == "Game Over Menu" || sceneName == "Victory Menu") {

            SceneManager.LoadScene(pastLevel);                          // Load the last scene the player was on. Using the past level variable.
        
        // Otherwise...        
        } else {

            // Set up a reference
            pastLevel = GameObject.Find("Player").GetComponent<PlayerScript>().playingScene;           // This will retrieve the level name for if the player wants to try the level again.
            pastLevelIndex = GameObject.Find("Player").GetComponent<PlayerScript>().playingSceneIndex; // This will retrieve the current level's index for possible future use later.
        }
    }
}


 
   