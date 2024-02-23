using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Village : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip kidnapSound;
    public TMP_Text populationText;
    [Header("Write the initial value for Population and Resources")]
    public int initialPeople = 10;
    public int initialResources = 100;
    [HideInInspector] public int peopleIndicator;
    [HideInInspector] public int resourcesBank;

    private bool gameLost = false;

    // Prefab for the person
    public GameObject personPrefab;

    // boundaries of the village
    [HideInInspector] public float villageBoundaryMinX;
    [HideInInspector] public float villageBoundaryMaxX;
    [HideInInspector] public float villageBoundaryMinY;
    [HideInInspector] public float villageBoundaryMaxY;

    [Header("write the min of distance between spawned people")]
    // Minimum distance between spawned people
    public float minDistanceBetweenPeople = 1f;

    void Start()
    {
        // peopleIndicator = initialPeople;
        // resourcesBank = initialResources;

        audioSource = GetComponent<AudioSource>();

        // RespawnPeopleRandomly();
    }

    void Update () {
        populationText.text = "Money: " + resourcesBank;
    }

    void RespawnPeopleRandomly()
    {
        for (int i = 0; i < initialPeople; i++)
        {
            RespawnPerson();
        }
    }

    public void KidnapPerson()
    {
        if (peopleIndicator > 0)
        {
            peopleIndicator--;
            Debug.Log("Population is: " + peopleIndicator);

            if (peopleIndicator == 0 && !gameLost)
            {
                // Handle losing condition here (e.g., show game over screen)
                Debug.Log("Game Over - All people kidnapped!");
                gameLost = true;
            }
        }
    }

    public void StealResources(int amount)
    {
        if (resourcesBank >= amount)
        {
            resourcesBank -= amount;
            Debug.Log("Resources is: " + resourcesBank);
        }
        else
        {
            resourcesBank = 0;
            // Handle losing condition here (e.g., show game over screen)
            if (resourcesBank == 0 && !gameLost)
            {
                Debug.Log("Game Over - Resources depleted!");
                // End Game
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+2);
                gameLost = true;
            }
        }
        audioSource.PlayOneShot(kidnapSound);
    }

    public void ReturnResources(int amount)
    {
        resourcesBank += amount;
    }

    void RespawnPerson()
    {
        float randomX, randomY;
        Vector3 respawnPosition;

        // Try to respawn a person until a suitable position is found
        do
        {
            randomX = Random.Range(villageBoundaryMinX, villageBoundaryMaxX);
            randomY = Random.Range(villageBoundaryMinY, villageBoundaryMaxY);

            respawnPosition = new Vector3(randomX, randomY, 0f);
            respawnPosition = transform.TransformPoint(respawnPosition);
        }
        while (IsTooCloseToOtherPeople(respawnPosition));

        InstantiatePerson(respawnPosition);
    }

    bool IsTooCloseToOtherPeople(Vector3 position)
    {
        // Check if the new position is too close to any existing people
        GameObject[] people = GameObject.FindGameObjectsWithTag("Person");

        foreach (var person in people)
        {
            if (Vector3.Distance(position, person.transform.position) < minDistanceBetweenPeople)
            {
                return true;
            }
        }

        return false;
    }

    void InstantiatePerson(Vector3 position)
    {
        // Ensure personPrefab is assigned in the inspector
        if (personPrefab != null)
        {
            // Instantiate a new person from the prefab at the specified position
            GameObject newPerson = Instantiate(personPrefab, position, Quaternion.identity);

            // You can customize or initialize other properties of the person GameObject if needed
        }
        else
        {
            Debug.LogError("Person prefab is not assigned in the inspector.");
        }
    }


    // Function to return an indicator for both resources and population
    public int GetPopulationIndicators()
    {
        return peopleIndicator;
    }
    public int GetResourcesIndicators()
    {
        return resourcesBank;
    }
}
