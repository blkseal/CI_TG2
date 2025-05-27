using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public GameObject[] itemPrefabs; // Assign good and bad prefabs
    public RectTransform canvasRect;
    public float spawnInterval = 1f;
    public float spawnMargin = 100f; // Margin from canvas edge
    public GameObject playerEasyPrefab;
    public GameObject playerMediumPrefab;
    public GameObject playerHardPrefab;
    public Transform playerParent; // Assign the canvas or a UI parent

    private GameObject playerInstance;

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
        int index = Random.Range(0, itemPrefabs.Length);
        GameObject item = Instantiate(itemPrefabs[index], transform);
        float halfWidth = canvasRect.rect.width / 2f;
        float x = Random.Range(-halfWidth + spawnMargin, halfWidth - spawnMargin);
        item.GetComponent<RectTransform>().anchoredPosition = new Vector2(x, canvasRect.rect.height / 2f + 50f);
    }
}
