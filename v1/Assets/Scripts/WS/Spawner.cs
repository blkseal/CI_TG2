using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public GameObject[] itemPrefabs; // Regular items first, then power-ups as last two
    public RectTransform canvasRect;
    public float spawnInterval = 1f;
    public float spawnMargin = 100f; // Margin from canvas edge
    public GameObject playerEasyPrefab;
    public GameObject playerMediumPrefab;
    public GameObject playerHardPrefab;
    public Transform playerParent; // Assign the canvas or a UI parent

    private GameObject playerInstance;

    // Adjust these to control power-up spawn rates (lower = rarer)
    [Range(0f, 1f)] public float powerUpChance = 0.15f; // Total chance for any power-up
    [Range(0f, 1f)] public float firstPowerUpRatio = 0.5f; // Split between the two power-ups

    void Start()
    {
        // Set spawn interval and player prefab based on difficulty
        switch (DifficultySelector.selectedDifficulty)
        {
            case DifficultySelector.Difficulty.Easy:
                spawnInterval = 1f;
                playerInstance = Instantiate(playerEasyPrefab, playerParent);
                break;
            case DifficultySelector.Difficulty.Medium:
                spawnInterval = 0.7f;
                playerInstance = Instantiate(playerMediumPrefab, playerParent);
                break;
            case DifficultySelector.Difficulty.Hard:
                spawnInterval = 0.5f;
                playerInstance = Instantiate(playerHardPrefab, playerParent);
                break;
        }

        InvokeRepeating(nameof(SpawnItem), 1f, spawnInterval);
    }

    void SpawnItem()
    {
        int regularCount = itemPrefabs.Length - 2; // Last two are power-ups
        int index;

        float roll = Random.value;
        if (roll < powerUpChance && itemPrefabs.Length >= 2)
        {
            // Spawn a power-up
            if (Random.value < firstPowerUpRatio)
                index = itemPrefabs.Length - 2; // First power-up
            else
                index = itemPrefabs.Length - 1; // Second power-up
        }
        else
        {
            // Spawn a regular item
            index = Random.Range(0, regularCount);
        }

        GameObject item = Instantiate(itemPrefabs[index], transform);
        float halfWidth = canvasRect.rect.width / 2f;
        float x = Random.Range(-halfWidth + spawnMargin, halfWidth - spawnMargin);
        item.GetComponent<RectTransform>().anchoredPosition = new Vector2(x, canvasRect.rect.height / 2f + 50f);
    }
}
