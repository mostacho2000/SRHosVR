using UnityEngine;

public class Quest_Manager : MonoBehaviour
{
    [Header("Quest Renderers")]
    public Renderer[] questRenderers = new Renderer[5]; // Asigna los 5 renderers de los objetos aquí

    [Header("Quest States (debug)")]
    public bool[] questStates = new bool[5]; // false = incompleto, true = completo

    [Header("GameObjects Visibility")]
    [SerializeField] private GameObject incompletQuestsObject; // GameObject que se muestra cuando faltan retos
    [SerializeField] private GameObject completedQuestsObject; // GameObject que se muestra cuando se completan los retos
    [SerializeField] private int requiredObjectivesCount = 3; // Número de retos que deben completarse
    [SerializeField] private GameObject colliderObjectToDisable; // GameObject con collider que se desactiva al completar objetivos

    [Header("Color Change Object")]
    [SerializeField] private Renderer colorChangeRenderer; // Renderer que cambiará de color al completar objetivos
    [SerializeField] private Color completedColor = Color.green; // Color cuando se completan todos los objetivos
    [SerializeField] private Color incompleteColor = Color.red; // Color cuando faltan objetivos

    void Awake()
    {
        if (questStates == null || questStates.Length != questRenderers.Length)
        {
            questStates = new bool[questRenderers.Length];
        }
    }

    void Start()
    {
        ResetQuestVisuals();
        UpdateGameObjectsVisibility();
    }

    void OnValidate()
    {
        if (questRenderers == null)
        {
            return;
        }

        if (questStates == null || questStates.Length != questRenderers.Length)
        {
            bool[] newStates = new bool[questRenderers.Length];
            for (int i = 0; i < Mathf.Min(questStates?.Length ?? 0, newStates.Length); i++)
            {
                newStates[i] = questStates[i];
            }
            questStates = newStates;
        }

        UpdateAllQuestVisuals();
        UpdateGameObjectsVisibility();
    }

    public void SetQuestState(int index, bool complete)
    {
        if (index >= 0 && index < questStates.Length)
        {
            questStates[index] = complete;
            UpdateQuestVisual(index);
            UpdateGameObjectsVisibility();
        }
    }

    public bool GetQuestState(int index)
    {
        if (index >= 0 && index < questStates.Length)
        {
            return questStates[index];
        }
        return false;
    }

    private void ResetQuestVisuals()
    {
        for (int i = 0; i < questStates.Length; i++)
        {
            questStates[i] = false;
        }
        UpdateAllQuestVisuals();
    }

    private void UpdateAllQuestVisuals()
    {
        for (int i = 0; i < questRenderers.Length; i++)
        {
            UpdateQuestVisual(i);
        }
    }

    private void UpdateQuestVisual(int index)
    {
        if (index < 0 || index >= questRenderers.Length || questRenderers[index] == null)
        {
            return;
        }

        questRenderers[index].material.EnableKeyword("_EMISSION");
        Color emissionColor = questStates[index] ? Color.green : Color.red;
        questRenderers[index].material.SetColor("_EmissionColor", emissionColor);
    }

    /// <summary>
    /// Cuenta cuántos retos han sido completados
    /// </summary>
    private int CountCompletedObjectives()
    {
        int completedCount = 0;
        for (int i = 0; i < questStates.Length; i++)
        {
            if (questStates[i])
            {
                completedCount++;
            }
        }
        return completedCount;
    }

    /// <summary>
    /// Actualiza la visibilidad de los GameObjects basado en el número de retos completados
    /// </summary>
    private void UpdateGameObjectsVisibility()
    {
        int completedCount = CountCompletedObjectives();
        bool allObjectivesCompleted = completedCount >= requiredObjectivesCount;

        // Activar/desactivar el GameObject de retos incompletos
        if (incompletQuestsObject != null)
        {
            incompletQuestsObject.SetActive(!allObjectivesCompleted);
        }

        // Activar/desactivar el GameObject de retos completados
        if (completedQuestsObject != null)
        {
            completedQuestsObject.SetActive(allObjectivesCompleted);
        }

        if (colliderObjectToDisable != null)
        {
            colliderObjectToDisable.SetActive(!allObjectivesCompleted);
        }

        // Cambiar el color del objeto basado en el progreso
        UpdateObjectMaterialColor(allObjectivesCompleted);
    }

    /// <summary>
    /// Cambia el color del material del objeto basado en si se completaron todos los objetivos
    /// </summary>
    private void UpdateObjectMaterialColor(bool isCompleted)
    {
        if (colorChangeRenderer == null)
        {
            return;
        }

        Color targetColor = isCompleted ? completedColor : incompleteColor;
        colorChangeRenderer.material.color = targetColor;
    }
}
