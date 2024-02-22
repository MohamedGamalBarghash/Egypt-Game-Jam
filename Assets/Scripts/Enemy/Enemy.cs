using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private PlayerMovement playerMovement; // Reference to the PlayerMovement script
    public float moveSpeed = 5f; // The movement speed of the enemy

    private void Start()
    {
        playerMovement = FindObjectOfType<PlayerMovement>(); // Find the PlayerMovement script in the scene
    }

    // Update is called once per frame
    void Update()
    {
        if (playerMovement != null)
        {
            // Get the player position from the PlayerMovement script
            Vector3 playerPosition = playerMovement.transform.position;

            // Calculate the direction from the enemy to the player
            Vector3 direction = playerPosition - transform.position;
            direction.Normalize();

            // Calculate the enemy's new position
            Vector3 newPosition = transform.position + direction * moveSpeed * Time.deltaTime;

            // Move the enemy towards the player
            transform.position = newPosition;
            StartCoroutine(WaitAndIncreaseCounter());
        }
    }
    private IEnumerator WaitAndIncreaseCounter()
    {
        yield return new WaitForSeconds(4f);
        GameObject.Destroy(gameObject);
    }
    }