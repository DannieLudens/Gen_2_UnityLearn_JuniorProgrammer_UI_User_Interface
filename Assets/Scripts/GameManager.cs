using UnityEngine;                    // Core Unity API
using System.Collections;             // Needed for IEnumerator (coroutines)
using System.Collections.Generic;     // Needed for List<T>
using TMPro;                          // Needed for TextMeshProUGUI
using UnityEngine.SceneManagement;    // Needed for SceneManager (restart)
using UnityEngine.UI;                 // Needed for Button
using Random = UnityEngine.Random;    // Makes clear "Random" means Unity's, not System.Random

// Attached to the "Game Manager" object in the scene: controls score, spawning, game over and restart
public class GameManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;     // Reference to the score UI text
    [SerializeField] private TextMeshProUGUI gameOverText;  // Reference to the Game Over UI text
    [SerializeField] private Button restartButton;          // Button shown on game over to play again
    [SerializeField] private GameObject titleScreen;        // Title + difficulty buttons, hidden when the game starts

    public List<GameObject> targets;  // Prefabs to spawn (filled in the Inspector by dragging Good 1-3 and Bad 1)

    public bool IsGameActive { get; private set; }  // Other scripts can read it, only GameManager can change it

    private float _spawnRate = 1.0f;   // Seconds to wait between spawns
    private int _score;                // Current score

    // Called by the difficulty buttons: starts the game with a faster spawn rate for higher difficulty
    public void StartGame(int difficulty)
    {
        IsGameActive = true;                    // Game is running: targets can be clicked and spawned
        _score = 0;                             // Initializes the score to 0
        _spawnRate /= difficulty;               // Easy = 1 s, Medium = 0.5 s, Hard = 0.33 s
        StartCoroutine(SpawnTarget());          // Starts SpawnTarget alongside the rest of the game
        UpdateScore(0);                         // Updates the score text to show 0
        titleScreen.SetActive(false);           // Hides the title screen
    }

    // Coroutine: while the game is active, waits _spawnRate seconds and spawns a random target
    private IEnumerator SpawnTarget()
    {
        while (IsGameActive)                                 // Stops spawning once the game is over
        {
            yield return new WaitForSeconds(_spawnRate);     // Pauses the coroutine (not the game) for _spawnRate seconds
            int index = Random.Range(0, targets.Count);      // Random index from 0 to Count-1 (with ints, the max is exclusive)
            Instantiate(targets[index]);                     // Creates a copy of the chosen prefab in the scene
        }
    }

    // Adds points to the score and refreshes the score text
    public void UpdateScore(int scoreToAdd)
    {
        _score += scoreToAdd;                          // Adds the points to the score
        scoreText.text = "Score: " + _score;           // Updates the score text
    }

    // Ends the game: shows Game Over and the restart button
    public void GameOver()
    {
        IsGameActive = false;                         // Stops spawning and clicking
        gameOverText.gameObject.SetActive(true);      // Shows the Game Over text
        restartButton.gameObject.SetActive(true);     // Shows the Restart button
    }

    // Hooked to the Restart button's OnClick: reloads the current scene from scratch
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);   // Loading the same scene resets everything
    }
}
