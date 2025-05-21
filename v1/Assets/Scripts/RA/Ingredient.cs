using UnityEngine;
using UnityEngine.UI;
using TMPro; // Para suporte ao TextMeshPro

public class Ingredient : MonoBehaviour
{
    public string category;
    public TMP_Text labelText;
    public Image labelBackground;

    public void SetVisual(DifficultySelector.Difficulty difficulty)
    {
        switch (difficulty)
        {
            case DifficultySelector.Difficulty.Easy:
                labelText.gameObject.SetActive(true);
                labelText.color = GetCategoryColor(category); // Color the label
                break;
            case DifficultySelector.Difficulty.Medium:
                labelText.gameObject.SetActive(true);
                labelText.color = Color.gray; // Gray label for medium
                break;
            case DifficultySelector.Difficulty.Hard:
                labelText.gameObject.SetActive(false);
                break;
        }
    }

    Color GetCategoryColor(string cat)
    {
        return cat switch
        {
            "Fruta" => new Color(0.56f, 0.93f, 0.56f), // light green
            "Prote�na" => new Color(0.86f, 0.08f, 0.24f), // red (crimson)
            "Latic�nios" => new Color(0.65f, 0.16f, 0.16f), // brownish red (saddle brown)
            "Cereais e derivados, tub�rculos" => new Color(1.0f, 0.92f, 0.016f), // yellow (gold)
            "Leguminosas" => new Color(1.0f, 0.55f, 0.0f), // orange
            "Hort�colas" => new Color(0.0f, 0.39f, 0.0f), // dark green
            "Gorduras e �leos" => new Color(0.55f, 0.27f, 0.07f), // brown
            "�gua" => Color.cyan,
            _ => Color.white,
        };
    }
}