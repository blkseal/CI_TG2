using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public Transform ingredientPanel;
    public List<GameObject> ingredientPrefabs; // Prefabs for all ingredients

    void Start()
    {
        var difficulty = DifficultySelector.selectedDifficulty;
        var ingredientsToSpawn = GetRandomIngredients(5); // Example: 5 ingredients

        foreach (var prefab in ingredientsToSpawn)
        {
            GameObject obj = Instantiate(prefab, ingredientPanel);
            var ing = obj.GetComponent<Ingredient>();
            ing.SetVisual(difficulty);
        }
    }

    List<GameObject> GetRandomIngredients(int count)
    {
        List<GameObject> copy = new List<GameObject>(ingredientPrefabs);
        List<GameObject> result = new List<GameObject>();

        for (int i = 0; i < count && copy.Count > 0; i++)
        {
            int index = Random.Range(0, copy.Count);
            result.Add(copy[index]);
            copy.RemoveAt(index);
        }

        return result;
    }
}
