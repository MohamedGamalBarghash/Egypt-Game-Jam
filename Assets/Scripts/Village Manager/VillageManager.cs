using System.Collections;
using UnityEngine;

public class VillageManager : MonoBehaviour
{
    [Header("Choose the number of the Villages")]
    public int numberOfVillages = 3;

    // Array to hold the size of each village
    [Header("Set the size for each village ")]
    public Vector2[] villageSizes;

    [Header("Put the prefab of the village")]
    public GameObject villagePrefab;

    [Header("Minimum distance between villages")]
    public float minDistanceBetweenVillages = 5f;

    void Start()
    {
        SpawnVillages();
    }

    void SpawnVillages()
    {
        // Ensure the array sizes match the number of villages
        if (villageSizes.Length != numberOfVillages)
        {
            Debug.LogError("Please set the correct number of village sizes in the inspector.");
            return;
        }

        for (int i = 0; i < numberOfVillages; i++)
        {
            Vector3 randomPosition = GetRandomPosition();

            // Check if the position is too close to existing villages
            while (IsTooCloseToOtherVillages(randomPosition))
            {
                randomPosition = GetRandomPosition();
            }

            GameObject villageObject = Instantiate(villagePrefab, randomPosition, Quaternion.identity);
            Village village = villageObject.GetComponent<Village>();

            // Check if the village script is attached
            if (village != null)
            {
                // Set boundaries for the village using the array
                village.villageBoundaryMinX = -villageSizes[i].x / 2f;
                village.villageBoundaryMaxX = villageSizes[i].x / 2f;
                village.villageBoundaryMinY = -villageSizes[i].y / 2f;
                village.villageBoundaryMaxY = villageSizes[i].y / 2f;
            }
            else
            {
                Debug.LogError("Village script not found on the instantiated village. Make sure the Village script is attached to the village prefab.");
            }

            // Attach any additional setup or customization logic for each village
        }
    }

    Vector3 GetRandomPosition()
    {
        // Logic to get a random position within the specified village boundaries
        float randomX = Random.Range(-10, 10f);  // Adjust as needed
        float randomZ = Random.Range(-10f, 10f);  // Adjust as needed

        return new Vector3(randomX, 0f, randomZ);
    }

    bool IsTooCloseToOtherVillages(Vector3 position)
    {
        // Check if the new position is too close to any existing villages
        GameObject[] villages = GameObject.FindGameObjectsWithTag("Village");

        foreach (var village in villages)
        {
            if (Vector3.Distance(position, village.transform.position) < minDistanceBetweenVillages)
            {
                return true;
            }
        }

        return false;
    }
}
