using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// This class displays the timer for the level, the current score and enemy count for the level.
// In the Update() function, when the timer reaches a set limit the game ends for the player.
public class WinLoseConditions : MonoBehaviour {
        
    int score;

    public Text timer;                         // Reference to the text that displays the timer in for the game level in play.
    public Text timeLimit;                     // Reference to the text that displays the time to complete the game level in.
    public Text currentScore;                  // Reference to the text that displays the currentScore of the player for that level.
    public Text enemiesLeft;                   // Reference to the text that displays how many enemies are remaining in the level that is being played.

    public int timerLimit;                     // The amount in seconds for how long the player will have to complete the level in.
    int timerValue;                            // The value for the time that has passed in the level.
    public static int enemyCount;              // The counter for how many enemies are in the current level.
    public int wlcTimerScore = 0;              // The value of the score the time the player has completed the level in comes to.

    bool isPlayerDeadOther;                    // A flag that indicates if the player is dead.
    bool triggered = false;                    // A flag to indicate if it has been triggered.
    public static bool levelCompleted = false; // A flag to indicate when the level has been completed.
    bool runOnce = false;                      // A flag to indicate a procedure has occured.
    bool isTimerReached = false;               // A flag for when the timer has reached the timer limit for the level.  

    PlayerScript playerScript;                 // Reference for the PlayerScript script.  

    void Awake() {
        
        levelCompleted = false;                               // Setting the value for the levelCompleted boolean.
    }

    // Use this for initialization
    void Start() {

        // Set up the references.
        enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
        playerScript = GameObject.Find("Player").GetComponent<PlayerScript>();

        timeLimit.text = timerLimit.ToString();               // The amount of time for the player to complete the level in displays here.
    }

    // Update is called once per frame
    void Update() {        

        float timerValue = Mathf.Round(Time.timeSinceLevelLoad);  // A stopwatch timer for the player.        

        isPlayerDeadOther = GameObject.Find("Player").GetComponent<PlayerScript>().isPlayerDead; // Access an is the player dead variable from PlayerScript.

        enemiesLeft.text = ": " + enemyCount;                 // The number of enemies in the level will be displayed here.
                
        // Check if the player is not dead.
        if (!isPlayerDeadOther && !triggered) {
            score = GameManager.score;
            currentScore.text = " Score: " + score;           // The score is displayed to the allocated reference.
            timer.text = " Time :  " + timerValue.ToString(); // The stopwatch timer is displayed to it's designated reference using ToString for data type conversion.
        }

        // If the time in game is the same as the time limit for the level AND the timer reached flag is false.
        if (timerValue == timerLimit && !isTimerReached) {
            isTimerReached = true;                            // Set the isTimerReached to true.
            playerScript.GameOver();                          // Execute the GameOver function within PlayerScript.
            Debug.Log("game over");                           // Outputs to the Unity Console "game over" when the above if statement is met. This aided me during development of the game.
        }

        // If the player is dead and the boolean trigger is false.
        if (levelCompleted && !runOnce) {

            triggered = true;                                 // The trigger flag is switched to true.
            runOnce = true;                                   // the runOnce flag is switched to true.

            wlcTimerScore = timerLimit - (int) timerValue;    // The WinLoseCondition Timer Score is calculated using casting as necessary.   

            try {
                GameManager.instance.HighScore();             // Execute the HighScore function found within the GameManager script.
            } catch {
                Debug.LogError("In WinLoseConditions Script - GameManager.instance.HighScore()");   // Error handling detailing the problem lies in the WinLoseConditions Script. Within the HighScore function found in the GameManager Script.
            }
        }       
    }            
}

