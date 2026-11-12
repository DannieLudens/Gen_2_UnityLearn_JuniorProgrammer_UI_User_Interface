using UnityEngine;                    // Core Unity API
using System.Collections;             // Needed for IEnumerator (coroutines)
using System.Collections.Generic;     // Needed for List<T>

// Attached to the "Game Manager" object in the scene: spawns targets at a fixed interval
public class GameManager : MonoBehaviour
{
    public List<GameObject> targets;  // Prefabs to spawn (filled in the Inspector by dragging Good 1-3 and Bad 1)
    private float _spawnRate = 1.0f;   // Seconds to wait between spawns

    // Runs once when the game starts: kicks off the spawning coroutine
    private void Start()
    {
        StartCoroutine(SpawnTarget());   // Starts SpawnTarget alongside the rest of the game
    }

    // Coroutine: infinite loop that waits _spawnRate seconds and spawns a random target
    IEnumerator SpawnTarget()
    {
        while (true)                                         // Repeats for as long as the game is running
        {
            yield return new WaitForSeconds(_spawnRate);      // Pauses the coroutine (not the game) for _spawnRate seconds
            int index = Random.Range(0, targets.Count);      // Random index from 0 to Count-1 (with ints, the max is exclusive)
            Instantiate(targets[index]);                     // Creates a copy of the chosen prefab in the scene
        }
    }

}
