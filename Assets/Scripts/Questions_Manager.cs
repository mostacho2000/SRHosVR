using UnityEngine;
using System.Collections.Generic;
using TMPro;
using System.Linq;

public class Questions_Manager : MonoBehaviour
{
    [System.Serializable]
    public class Question
    {
        public TMP_Text questionTextMesh;
        public TMP_Text[] optionMeshes = new TMP_Text[3]; // 3 opciones
        public int correctAnswerIndex; // 0-2

        // Propiedades para obtener el texto de los TextMeshPro
        public string QuestionText => questionTextMesh != null ? questionTextMesh.text : "";
        public string[] Options => optionMeshes.Select(t => t != null ? t.text : "").ToArray();
    }

    [Tooltip("Lista de preguntas para la trivia")]
    public List<Question> questions = new List<Question>();

    [Tooltip("Objetos que se activarán secuencialmente al acertar cada pregunta")]
    public GameObject[] objectsToActivate = new GameObject[3];

    [Tooltip("Índice de la pregunta actual")]
    public int currentQuestionIndex = 0;

    [Tooltip("Estado de la trivia: true si está activa")]
    public bool triviaActive = true;

    [Tooltip("Quest Manager para actualizar el estado de la misión")]
    public Quest_Manager questManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Inicializar: desactivar todos los objetos
        for (int i = 0; i < objectsToActivate.Length; i++)
        {
            if (objectsToActivate[i] != null)
            {
                objectsToActivate[i].SetActive(false);
            }
        }
        currentQuestionIndex = 0;
        triviaActive = false; // La trivia no está activa hasta que se llame StartTrivia()
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Método para comprobar la respuesta seleccionada
    public void CheckAnswer(int selectedIndex)
    {
        if (!triviaActive || currentQuestionIndex >= questions.Count) return;

        if (selectedIndex == questions[currentQuestionIndex].correctAnswerIndex)
        {
            // Acertó: ocultar el objeto actual
            if (objectsToActivate[currentQuestionIndex] != null)
            {
                objectsToActivate[currentQuestionIndex].SetActive(false);
            }

            // Activar el siguiente objeto si existe
            if (currentQuestionIndex + 1 < objectsToActivate.Length && objectsToActivate[currentQuestionIndex + 1] != null)
            {
                objectsToActivate[currentQuestionIndex + 1].SetActive(true);
            }

            bool wasThirdQuestion = currentQuestionIndex == 2;
            currentQuestionIndex++;

            if (wasThirdQuestion && questManager != null)
            {
                questManager.SetQuestState(2, true);
            }

            // Si ya no hay más preguntas, desactivar trivia
            if (currentQuestionIndex >= questions.Count)
            {
                triviaActive = false;
            }
        }
        else
        {
            // Falló: no hacer nada, o puedes agregar lógica aquí (ej. mensaje de error)
            Debug.Log("Respuesta incorrecta. Intenta de nuevo.");
        }
    }

    // Método para obtener la pregunta actual
    public Question GetCurrentQuestion()
    {
        if (currentQuestionIndex < questions.Count)
        {
            return questions[currentQuestionIndex];
        }
        return null;
    }

    // Método para iniciar la trivia
    public void StartTrivia()
    {
        if (questions.Count == 0) return;

        currentQuestionIndex = 0;
        triviaActive = true;

        // Activar el primer objeto
        if (objectsToActivate.Length > 0 && objectsToActivate[0] != null)
        {
            objectsToActivate[0].SetActive(true);
        }
    }

    // Método para resetear la trivia
    public void ResetTrivia()
    {
        currentQuestionIndex = 0;
        triviaActive = false;
        for (int i = 0; i < objectsToActivate.Length; i++)
        {
            if (objectsToActivate[i] != null)
            {
                objectsToActivate[i].SetActive(false);
            }
        }
    }
}
