using UnityEngine;

public class DinoSpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    public GameObject[] dinoPrefabs;
    public float minSpawnTime = 1f;
    public float maxSpawnTime = 2.5f;
    public float spawnXOffset = 25f; 
    public float spawnYPosition = -2f; 

    [Header("Anti-Clump Settings")]
    public int maxStreak = 2;
    
    [Tooltip("Minimum distance REQUIRED between enemies so they don't overlap.")]
    public float minDistanceBetweenDinos = 8f;

    private float timer;
    private float currentSpawnDelay;
    private Transform mainCamera;

    private int lastSpawnIndex = -1;
    private int currentStreak = 0;
    
    // Tracks the physical object of the last spawned dino
    private GameObject lastSpawnedDino; 

    void Start()
    {
        mainCamera = Camera.main.transform;
        SetNextSpawnTime();
    }

    void Update()
    {
        // If time is frozen (game is paused or game over), don't update the spawn timer!
        if (Time.timeScale == 0f) return;

        timer += Time.deltaTime;
        
        if (timer >= currentSpawnDelay)
        {
            // Only spawn if the gap is wide enough!
            if (CanSpawnDino())
            {
                SpawnDino();
                SetNextSpawnTime();
                timer = 0; // Reset timer only if we successfully spawned[cite: 1]
            }
        }
    }

    private bool CanSpawnDino()
    {
        // If there is no previous dino (it died or hasn't spawned), we are clear to spawn
        if (lastSpawnedDino == null) return true;

        // Calculate the physical distance between the spawn point and the last dino
        float spawnPointX = mainCamera.position.x + spawnXOffset;
        float distance = Mathf.Abs(spawnPointX - lastSpawnedDino.transform.position.x);

        // Return true only if the distance is greater than our required gap
        return distance >= minDistanceBetweenDinos;
    }

    private void SetNextSpawnTime()
    {
        currentSpawnDelay = Random.Range(minSpawnTime, maxSpawnTime);
    }

    private void SpawnDino()
    {
        if (dinoPrefabs.Length == 0) return;

        int randomIndex = Random.Range(0, dinoPrefabs.Length);

        // --- STREAK BREAKER ---
        if (randomIndex == lastSpawnIndex)
        {
            currentStreak++;
            if (currentStreak >= maxStreak)
            {
                randomIndex = (randomIndex + 1) % dinoPrefabs.Length;
                currentStreak = 1; 
            }
        }
        else
        {
            currentStreak = 1;
        }

        lastSpawnIndex = randomIndex;

        // --- SPAWN & TRACK ---
        Vector3 spawnPos = new Vector3(mainCamera.position.x + spawnXOffset, spawnYPosition, 0f);
        
        // Save the newly spawned dino into our tracking variable
        lastSpawnedDino = Instantiate(dinoPrefabs[randomIndex], spawnPos, Quaternion.identity);
        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlayDinoSpawnSound();
        }
    }
}