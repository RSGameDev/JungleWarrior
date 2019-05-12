using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// This class is created from the start of the game.
// The scores are set, tracked and updated in the GameManager script.
// HighScore() function is included in this script.
public class GameManager : MonoBehaviour {

    bool isSceneChanged = false;               // is this scene different to the previous scene. 
    
    string lastSceneName;                      // The name for the last scene will be stored here.
    string currentSceneName;                   // The name for the current scene will be stored here.

    public static int score;                   // The value of the score for the player.

    Text highScore = null;                     // Reference to the text component initially set to null. As it's only required on the GameOverScene.
                                               // This is to avoid an error message appearing before the required scene. 
    Text combinedHighScore = null;             // Reference to the text component initially set to null. As it's only required on the GameOverScene.
                                               // This is to avoid an error message appearing before the required scene. 
    Text endLevelNumber;                       // Reference to the text component that will display custom created string.
    Text endLevelNumberGameOver;               // Reference to the text component that will display custom created string. 

    public static GameManager instance = null; // Allows other scripts to call functions from GameManager.

    void Awake() {

        score = 0;                             // Setup the starting score for the player at the start of the level.

        // Check if there is already an instance of GameManager.
        if (instance == null)
            instance = this;                    // If not, set it to this.

        // If instance already exists.
        else if (instance != this)
            Destroy(gameObject);                // Destroy this, so there can only be one instance of GameManager.

        DontDestroyOnLoad(gameObject);          // Set GameManager to DontDestroyOnLoad so it won't be destroyed when reloading scenes.

    }

    // Use this for initialization
    void Start() {   

        // Set up the references.
        currentSceneName = SceneManager.GetActiveScene().name;
        lastSceneName = currentSceneName;        
    }


    // Update is called once per frame
    void Update() {
        
        currentSceneName = SceneManager.GetActiveScene().name;      // Continually check the scene that is running.        

        // USED DURING IMPLEMENTATION OF THE HIGHSCORE FEATURE IN THE GAME.
        // Before submission of this task. I used this function once then commented out to disable the feature.
        // If the I key is pressed, wipe all the highscores for the game.
        //if (Input.GetKeyDown(KeyCode.I)) {              
        //    Debug.Log("Clear scores");                              // Outputs to the Unity console "Clear scores".
        //    PlayerPrefs.DeleteKey("HighScore");                     // Removes the value from the key "HighScore".      
        //    PlayerPrefs.DeleteKey("Level 1 HighScore");             // Removes the value from the key "Level 1 HighScore". 
        //    PlayerPrefs.DeleteKey("Level 2 HighScore");             // Removes the value from the key "Level 2 HighScore". 
        //    PlayerPrefs.DeleteKey("Level 3 HighScore");             // Removes the value from the key "Level 3 HighScore". 
        //    PlayerPrefs.DeleteKey("Total HighScore");               // Removes the value from the key "Total HighScore". 
        //}

        // To stop an error occuring from the scene not being able to access the highScore text component.
        // When the relevant scene is active. The value for highscore can be displayed on the screen:
        // Check is the last scene name different to the current scene name. 
        if (lastSceneName != currentSceneName) {
            isSceneChanged = true;                                  // Set the scene is changed flag to commence the following check.
        }

        // If the scene has changed.
        if (isSceneChanged) {

            // If the scene name is Start Menu.
            if (currentSceneName == "Start Menu") {
                highScore = null;                                   // The highScore text remains as null.
                combinedHighScore = null;                           // The combinedHighScore text remains as null.
                return;

              // If the scene name is Level Select Menu.
            } else if (currentSceneName == "Level Select Menu") {
                highScore = null;                                   // The highScore text remains as null.
                combinedHighScore = null;                           // The combinedHighScore text remains as null.
                return;

              // If the scene name is Game Over Menu.
            } else if (currentSceneName == "Game Over Menu") {

                GameObject endLevelNumberGO = GameObject.Find("Game Over Score Text");      // Find the game object named Game Over Score Text.
                endLevelNumberGameOver = endLevelNumberGO.GetComponent<Text>();             // Reference the text component of the object to endLevelNumberGameOver.
                GameObject highScoreGo = GameObject.Find("High Score");                     // Find the game object named High Score
                highScore = highScoreGo.GetComponent<Text>();                               // Reference the text component of the object to highScore.

                // If the last scene name is Level 1.
                if (lastSceneName == "Level 1") {
                    endLevelNumberGameOver.text = ("Level 1 HighScore");                    // Assign the string value to endLevelNumberGameOver.text. 
                    highScore.text = PlayerPrefs.GetInt("Level 1 HighScore", 0).ToString(); // Return the value corresponding to Level 1 HighScore otherwise use the default assigned.
                                                                                            // Using ToString as conversion of data type is required.
                // If the last scene name is Level 2.
                } else if (lastSceneName == "Level 2") {
                    endLevelNumberGameOver.text = ("Level 2 HighScore");                    // Assign the string value to endLevelNumberGameOver.text. 
                    highScore.text = PlayerPrefs.GetInt("Level 2 HighScore", 0).ToString(); // Return the value corresponding to Level 2 HighScore otherwise use the default assigned.
                                                                                            // Using ToString as conversion of data type is required.
                // If the last scene name is Level 3.
                } else if (lastSceneName == "Level 3") {
                    endLevelNumberGameOver.text = ("Level 3 HighScore");                    // Assign the string value to endLevelNumberGameOver.text. 
                    highScore.text = PlayerPrefs.GetInt("Level 3 HighScore", 0).ToString(); // Return the value corresponding to Level 3 HighScore otherwise use the default assigned.
                                                                                            // Using ToString as conversion of data type is required.
                }

            // If the scene name is Victory Menu.
            } else if (currentSceneName == "Victory Menu") {
                
                GameObject endLevelNumberGO = GameObject.Find("High Score Text");           // Find the game object named High Score Text.
                endLevelNumber = endLevelNumberGO.GetComponent<Text>();                     // Reference the text component of the object to endLevelNumber.
                GameObject highScoreGo = GameObject.Find("High Score");                     // Find the game object named High Score.
                highScore = highScoreGo.GetComponent<Text>();                               // Reference the text component of the object to highScore.
                GameObject combinedHighScoreGo = GameObject.Find("Total High Score Text");  // Find the game object named Total High Score Text.           
                GameObject combinedHighScoreText = GameObject.Find("Total High Score");     // Find the game object named Total High Score. 
                combinedHighScore = combinedHighScoreText.GetComponent<Text>();             // Reference the text component of the object to highScore.
                GameObject nextLevelButton = GameObject.Find("Next Level Button");          // Find the game object named Next Level Button. 

                // If the last scene name is Level 1 or Development Level 1.
                if (lastSceneName == "Level 1" || lastSceneName == "Development Level 1") {

                    endLevelNumber.text = ("Level 1 HighScore");                            // Assign the string value to endLevelNumber.text. 
                    combinedHighScoreGo.SetActive(false);                                   // Disable the referenced game object as it is not required until the last level of the game.
                    highScore.text = PlayerPrefs.GetInt("Level 1 HighScore", 0).ToString(); // Return the value corresponding to HighScore otherwise use the default assigned.
                                                                                            // Using ToString as conversion of data type is required.
                                                                                            // If the current scene name is Level 2 or Development Level 2.
                } else if (lastSceneName == "Level 2" || lastSceneName == "Development Level 2") {

                    endLevelNumber.text = ("Level 2 HighScore");                            // Assign the string value to endLevelNumber.text. 
                    combinedHighScoreGo.SetActive(false);                                   // Disable the referenced game object as it is not required until the last level of the game.
                    highScore.text = PlayerPrefs.GetInt("Level 2 HighScore", 0).ToString(); // Return the value corresponding to HighScore otherwise use the default assigned.
                                                                                            // Using ToString as conversion of data type is required. 
                    if (lastSceneName == "Development Level 2") {
                        combinedHighScoreGo.SetActive(true);                                // Enables the referenced game object as this is the last test level in the game.
                        nextLevelButton.SetActive(false);                                   // Disable the referenced game object as the next level button is no longer required.    

                        var one = PlayerPrefs.GetInt("Level 1 HighScore", 0);               // Store the value corresponding to Level 1 HighScore otherwise use the default assigned.
                        var two = PlayerPrefs.GetInt("Level 2 HighScore", 0);               // Store the value corresponding to Level 2 HighScore otherwise use the default assigned.
                        var total = one + two;                                              // Combine the scores from levels 1 and 2 for a complete total.

                        PlayerPrefs.SetInt("Total HighScore", total);                                   // Set total to the Total HighScore key indentifier.
                        combinedHighScore.text = PlayerPrefs.GetInt("Total HighScore", 0).ToString();   // Return the value corresponding to Total HighScore otherwise use the default assigned.

                    }
                 
               
                    // If the current scene name is Level 3.
                } else if (lastSceneName == "Level 3") {

                    endLevelNumber.text = ("Level 3 HighScore");                            // Assign the string value to endLevelNumber.text. 
                    highScore.text = PlayerPrefs.GetInt("Level 3 HighScore", 0).ToString(); // Return the value corresponding to HighScore otherwise use the default assigned.
                                                                                            // Using ToString as conversion of data type is required.
                    nextLevelButton.SetActive(false);                                       // Disable the referenced game object as the next level button is no longer required.    

                    var one = PlayerPrefs.GetInt("Level 1 HighScore", 0);                   // Store the value corresponding to Level 1 HighScore otherwise use the default assigned.
                    var two = PlayerPrefs.GetInt("Level 2 HighScore", 0);                   // Store the value corresponding to Level 2 HighScore otherwise use the default assigned.
                    var three = PlayerPrefs.GetInt("Level 3 HighScore", 0);                 // Store the value corresponding to Level 3 HighScore otherwise use the default assigned.
                    var total = one + two + three;                                          // Combine the scores from levels 1 to 3 for a complete total.

                    PlayerPrefs.SetInt("Total HighScore", total);                                   // Set total to the Total HighScore key indentifier.
                    combinedHighScore.text = PlayerPrefs.GetInt("Total HighScore", 0).ToString();   // Return the value corresponding to Total HighScore otherwise use the default assigned.

                }
            }
                lastSceneName = currentSceneName;                       // Change last scene name to the current scene name.
                isSceneChanged = false;                                 // Set the scene is changed flag to false again.
        }
    }

    // Used to set the HighScore
    public void HighScore() {
        
        int timerScore = GameObject.Find("WinLoseConditions").GetComponent<WinLoseConditions>().wlcTimerScore; // Access timerScore from WinLoseConditions script and pass the data to timerScore in this script.
                
        int theScore = score + (timerScore*4);                          // Combine the score from enemy kills with the score from the game timer (multiplied by four to make timerScore more meaningful).

        Debug.Log("GameManager timervalue " + timerScore);              // Outputs to the Unity console the value for the 'timerScore' variable. This aided me during development of the game.
        Debug.Log("GameManager score " + score);                        // Outputs to the Unity console the value for the 'score' variable. This aided me during development of the game.
        Debug.Log("GameManager the score " + theScore);                 // Outputs to the Unity console the value for the 'theScore' variable. This aided me during development of the game.

        // If the current scene name is Level 1 or Development Level 1.
        if (currentSceneName == "Level 1" || currentSceneName == "Development Level 1") {
            // If the highscore is greater than the assigned PlayerPref key or the default value.
            if (theScore >= PlayerPrefs.GetInt("Level 1 HighScore", 0)) {   
                PlayerPrefs.SetInt("Level 1 HighScore", theScore);          // Set the larger combined score to the HighScore key indentifier.
            }
        }

        // If the current scene name is Level 2 or Development Level 2.
        if (currentSceneName == "Level 2" || currentSceneName == "Development Level 2") {
            // If the highscore is greater than the assigned PlayerPref key or the default value.
            if (theScore >= PlayerPrefs.GetInt("Level 2 HighScore", 0)) {
                PlayerPrefs.SetInt("Level 2 HighScore", theScore);          // Set the larger combined score to the HighScore key indentifier.
            }
        }

        // If the current scene name is Level 3.
        if (currentSceneName == "Level 3") {
            // If the highscore is greater than the assigned PlayerPref key or the default value.
            if (theScore >= PlayerPrefs.GetInt("Level 3 HighScore", 0)) {
                PlayerPrefs.SetInt("Level 3 HighScore", theScore);          // Set the larger combined score to the HighScore key indentifier.
            }
        }                
    }  
}

