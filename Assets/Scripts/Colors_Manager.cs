using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class Colors_Manager : MonoBehaviour
{
    [System.Serializable]
    public class ButtonData
    {
        public GameObject button;
        public int currentColorIndex;
        public string CurrentColor => colorNames[currentColorIndex];
    }

    public List<ButtonData> buttonDatas = new List<ButtonData>();

    [Tooltip("Asignar aquí la combinación correcta usando índices: 0=Green, 1=Blue, 2=Pink, 3=Red, 4=Yellow")]
    public List<int> targetCombination = new List<int> { 0, 0, 0, 0, 0 };

    [Tooltip("GameObject cuyo material cambiará a verde cuando la combinación sea correcta")]
    public GameObject targetObject;

    [Tooltip("Indica si la combinación actual coincide con la combinación correcta")]
    public bool combinationMatched;

    [Tooltip("Texto para ver el estado actual de la combinación en el Inspector")]
    public string combinationStatus;

    public Quest_Manager questManager; // Referencia al Quest_Manager para actualizar el estado de las quests


    private Color targetObjectOriginalColor = Color.white;

    private static readonly string[] colorNames = { "Green", "Blue", "Pink", "Red" };
    private static readonly Color[] colors = { Color.green, new Color (0f,0f, 1f), new Color(1f, 0.5f, 0.8f), Color.red, new Color(1f, 1f, 0f) };


    void Start()
    {
        InitializeButtonColors();
        StoreTargetObjectOriginalColor();
        UpdateCombinationState();
    }

    private void InitializeButtonColors()
    {
        for (int i = 0; i < buttonDatas.Count; i++)
        {
            if (buttonDatas[i].button != null)
            {
                Image image = buttonDatas[i].button.GetComponent<Image>();
                if (image != null)
                {
                    image.color = colors[buttonDatas[i].currentColorIndex];
                }
            }
        }
    }

    private void StoreTargetObjectOriginalColor()
    {
        if (targetObject == null) return;

        Image uiImage = targetObject.GetComponent<Image>();
        if (uiImage != null)
        {
            targetObjectOriginalColor = uiImage.color;
            return;
        }

        Renderer renderer = targetObject.GetComponent<Renderer>();
        if (renderer != null)
        {
            targetObjectOriginalColor = renderer.material.color;
        }
    }

    public void ChangeColor(int index)
    {
        if (index < 0 || index >= buttonDatas.Count) return;

        buttonDatas[index].currentColorIndex = (buttonDatas[index].currentColorIndex + 1) % colors.Length;
        UpdateButtonColorVisual(index);
        UpdateCombinationState();
    }

    private void UpdateButtonColorVisual(int index)
    {
        if (buttonDatas[index].button == null) return;

        Image image = buttonDatas[index].button.GetComponent<Image>();
        if (image != null)
        {
            image.color = colors[buttonDatas[index].currentColorIndex];
        }
    }

    private void UpdateCombinationState()
    {
        combinationMatched = IsCombinationCorrect();
        combinationStatus = combinationMatched ? "Correct combination" : "Incorrect combination";
        UpdateTargetObjectColor();

        if (combinationMatched && questManager != null)
        {
            questManager.SetQuestState(1, true);
        }
    }

    private bool IsCombinationCorrect()
    {
        if (buttonDatas.Count != targetCombination.Count)
        {
            return false;
        }

        for (int i = 0; i < buttonDatas.Count; i++)
        {
            if (buttonDatas[i].currentColorIndex != targetCombination[i])
            {
                return false;
            }
        }

        return true;
    }

    private void UpdateTargetObjectColor()
    {
        if (targetObject == null) return;

        Image uiImage = targetObject.GetComponent<Image>();
        if (uiImage != null)
        {
            uiImage.color = combinationMatched ? Color.green : targetObjectOriginalColor;
            return;
        }

        Renderer renderer = targetObject.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = combinationMatched ? Color.green : targetObjectOriginalColor;
        }
    }

    public string GetCurrentColor(int index)
    {
        if (index < 0 || index >= buttonDatas.Count) return "Invalid";
        return buttonDatas[index].CurrentColor;
    }

    public Color GetCurrentColorValue(int index)
    {
        if (index < 0 || index >= buttonDatas.Count) return Color.white;
        return colors[buttonDatas[index].currentColorIndex];
    }
}

