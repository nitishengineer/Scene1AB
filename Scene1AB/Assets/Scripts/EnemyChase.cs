using UnityEngine;
using UnityEngine.SceneManagement; // Required to restart the scene
using System.Collections; // Required for the countdown delay

public class EnemyChase : MonoBehaviour
{
    [Header("Settings")]
    public Transform player; // Drag your Player GameObject here in Inspector
    public float moveSpeed = 3.5f; // Enemy speed (try slightly slower than player)

    private Rigidbody rb;
    private bool isCaught = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // If player is caught, stop moving
        if (isCaught || player == null) return;

        // Calculate direction: if player is to the right, move positive X, etc.
        float direction = Mathf.Sign(player.position.x - transform.position.x);

        // Apply movement to Rigidbody
        Vector3 newVelocity = rb.linearVelocity;
        newVelocity.x = direction * moveSpeed;
        newVelocity.z = 0; // Ensure enemy stays on the 2D plane
        rb.linearVelocity = newVelocity;
    }

    // Detects physical collision with another object
    void OnCollisionEnter(Collision collision)
    {
        // Check if the object we hit has the "Player" tag
        if (collision.gameObject.CompareTag("Player"))
        {
            isCaught = true;
            Debug.Log("Player Caught! Restarting in 2 seconds...");

            // Start the countdown to restart
            StartCoroutine(RestartSceneRoutine());
        }
    }

    // A coroutine allows us to wait for a few seconds before doing something
    IEnumerator RestartSceneRoutine()
    {
        // Wait for 2 seconds
        yield return new WaitForSeconds(2f);

        // Reload the currently active scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}