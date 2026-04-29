using UnityEngine;

public class AnswerButton : MonoBehaviour
{
    public Questions_Manager questionsManager; // Referencia al Questions_Manager
    public int optionIndex; // Índice de la opción (0, 1 o 2)

    // Este método se llama cuando se presiona el botón
    public void OnButtonPressed()
    {
        if (questionsManager != null)
        {
            questionsManager.CheckAnswer(optionIndex);
        }
    }
}