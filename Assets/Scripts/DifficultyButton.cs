using UnityEngine;      // Core Unity API
using UnityEngine.UI;   // Needed for Button

// Attached to each difficulty button (Easy, Medium, Hard): starts the game with its difficulty
public class DifficultyButton : MonoBehaviour
{
    [SerializeField] private int difficulty = 1;   // Divides the spawn rate: 1 = Easy, 2 = Medium, 3 = Hard

    private Button _button;                         // The Button component on this same GameObject
    private GameManager _gameManager;               // Reference to the GameManager to start the game

    // Runs once at the start: finds the references and hooks the click from code
    private void Start()
    {
        _button = GetComponent<Button>();                                            // Gets the Button on this GameObject
        _gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();  // Finds the GameManager in the scene
        _button.onClick.AddListener(SetDifficulty);                                  // Calls SetDifficulty when the button is clicked
    }

    // Called when the button is clicked: starts the game with this button's difficulty
    private void SetDifficulty()
    {
        Debug.Log(gameObject.name + " was clicked");   // Console message for debugging
        _gameManager.StartGame(difficulty);            // Starts the game
    }
}
