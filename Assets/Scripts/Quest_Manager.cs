using UnityEngine;

public class Quest_Manager : MonoBehaviour
{
    public Renderer[] questRenderers = new Renderer[5]; // Asigna los 5 renderers de los objetos aquí
    private bool[] questStates = new bool[5]; // false = incompleto, true = completo

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < questRenderers.Length; i++)
        {
            if (questRenderers[i] != null)
            {
                questStates[i] = false; // Inicia en incompleto
                questRenderers[i].material.EnableKeyword("_EMISSION");
                questRenderers[i].material.SetColor("_EmissionColor", Color.red);
            }
        }
    }

    // Método para cambiar el estado de una quest
    public void SetQuestState(int index, bool complete)
    {
        if (index >= 0 && index < questRenderers.Length && questRenderers[index] != null)
        {
            questStates[index] = complete;
            Color emissionColor = complete ? Color.green : Color.red;
            questRenderers[index].material.SetColor("_EmissionColor", emissionColor);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
