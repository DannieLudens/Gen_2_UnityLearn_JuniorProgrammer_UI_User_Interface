using System;
using UnityEngine;                    // Core Unity API (MonoBehaviour, Rigidbody, Vector3...)
using UnityEngine.InputSystem;        // New Input System (Mouse.current, keyboard, etc.)
using Random = UnityEngine.Random;    // Makes clear "Random" means Unity's, not System.Random

// Attached to each prefab (Good 1-3, Bad 1): tosses the object upward and destroys it when clicked
public class Target : MonoBehaviour
{
    [SerializeField] private float maxSpeed = 16;     // Maximum upward launch force
    [SerializeField] private float minSpeed = 12;     // Minimum upward launch force
    [SerializeField] private float maxTorque = 10;    // Maximum spin (per axis) so it rotates in the air
    [SerializeField] private float xRange = 4;        // Horizontal spawn range: between -4 and 4
    [SerializeField] private float ySpawnPos = -6;    // Spawn height (below the screen)

    private Rigidbody _targetRb;                      // Reference to the Rigidbody so we can apply forces
    private GameManager gameManager;                 // Reference to the GameManager so we can update the score
    public int pointValue;                               // Points earned for clicking this target

    // Runs once when the object appears: positions it and launches it with random force and spin
    private void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();                    // Finds the GameManager in the scene
        _targetRb = GetComponent<Rigidbody>();                                                        // Gets the Rigidbody on this same GameObject
        _targetRb.AddForce(RandomForce(), ForceMode.Impulse);                                         // Instant upward push
        _targetRb.AddTorque(RandomTorque(), RandomTorque(), RandomTorque(), ForceMode.Impulse);       // Instant spin on X, Y and Z
        transform.position = RandomSpawnPos();                                                        // Moves it to a random X below the screen
    }

    // Runs every frame: checks whether THIS object was clicked
    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)                                  // Was the left mouse button pressed this frame?
        {
            Debug.Log("Mouse Clicked");                                                    // Console message for debugging
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());   // Builds a ray from the camera through the mouse pointer
            Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red, 2.0f); // Draws the ray in red in the Scene view for 2 s

            if (Physics.Raycast(ray, out RaycastHit hit))                                  // Casts the ray; if it hits a collider, the result is stored in "hit"
            {
                if (hit.transform == transform)                                            // If ray hit this enemy, destroy it
                {
                    Destroy(gameObject);                                                   // Removes the object from the scene
                    gameManager.UpdateScore(5);                                            // Updates the score from the GameManager class and adds 5 points
                }
            }

        }
    }

    // Returns an upward vector with a random magnitude between minSpeed and maxSpeed
    private Vector3 RandomForce()
    {
        return Vector3.up * Random.Range(minSpeed, maxSpeed);   // (0,1,0) multiplied by the random force
    }

    // Returns a random spin value between -maxTorque and maxTorque
    private float RandomTorque()
    {
        return Random.Range(-maxTorque, maxTorque);
    }

    // Returns a spawn position: random X, fixed low Y, Z = 0
    private Vector3 RandomSpawnPos()
    {
        return new Vector3(Random.Range(-xRange, xRange), ySpawnPos);   // Z is omitted, so it defaults to 0
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DestroyZone"))
        {
            Destroy(gameObject);
        }
    }
}
