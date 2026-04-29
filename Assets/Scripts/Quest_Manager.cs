using UnityEngine;

public class Quest_Manager : MonoBehaviour
{
    [Header("Quest Renderers")]
    public Renderer[] questRenderers = new Renderer[5]; // Asigna los 5 renderers de los objetos aquí

    [Header("Quest States (debug)")]
    public bool[] questStates = new bool[5]; // false = incompleto, true = completo

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
    }

    public void SetQuestState(int index, bool complete)
    {
        if (index >= 0 && index < questStates.Length)
        {
            questStates[index] = complete;
            UpdateQuestVisual(index);
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
}
