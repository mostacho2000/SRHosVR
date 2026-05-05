using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

public class Code_Manager : MonoBehaviour
{
    public TMP_InputField inputField; // Asigna el Input Field aquí
    public Button[] numberButtons = new Button[10]; // Asigna los botones 0-9 aquí (índice 0 = botón 0, etc.)
    public string correctCode = "1234"; // Código correcto, asignable en Inspector
    public UnityEvent onCorrect; // Evento para código correcto
    public UnityEvent onIncorrect; // Evento para código incorrecto
    public GameObject objectToActivate; // Objeto a activar cuando el código sea correcto

    [Header("Debug / UI")]
    public TMP_Text correctMessageText; // Texto que se muestra al acertar
    public TMP_Text incorrectMessageText; // Texto que se muestra al fallar
    public float messageDisplayTime = 2f; // Tiempo en segundos que se muestra el texto

    public Quest_Manager questManager; // Referencia al Quest_Manager para actualizar el estado de las quests

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Asignar listeners a los botones numéricos
        for (int i = 0; i < numberButtons.Length; i++)
        {
            int number = i; // Capturar el valor para el closure
            numberButtons[i].onClick.AddListener(() => AddNumber(number.ToString()));
        }

        // Verificar el código cuando cambie el valor del input field
        inputField.onValueChanged.AddListener(delegate { CheckCode(); });

        HideMessageText(correctMessageText);
        HideMessageText(incorrectMessageText);
    }

    // Método para agregar un número al input field
    private void AddNumber(string num)
    {
        if (inputField.text.Length < 5) // Limitar a 5 dígitos
        {
            inputField.text += num;
        }
    }

    // Método para verificar el código
    private void CheckCode()
    {
        if (inputField.text.Length == 5)
        {
            if (inputField.text == correctCode)
            {
                onCorrect.Invoke();
                ShowTemporaryMessage(correctMessageText);

                if (objectToActivate != null)
                {
                    objectToActivate.SetActive(true);
                }

                if (questManager != null)
                {
                    questManager.SetQuestState(0, true);
                }

                Debug.Log("Código correcto");
                
                // No resetear el input cuando es correcto
            }
            else
            {
                onIncorrect.Invoke();
                ShowTemporaryMessage(incorrectMessageText);
                inputField.text = ""; // Resetear el input cuando es incorrecto
                Debug.Log("Código incorrecto");
            }
        }
    }

    // Método público para resetear el input field externamente
    public void ResetInput()
    {
        inputField.text = "";
    }

    private void ShowTemporaryMessage(TMP_Text messageText)
    {
        if (messageText == null)
        {
            return;
        }

        messageText.gameObject.SetActive(true);
        StopCoroutine("HideMessageAfterDelay");
        StartCoroutine(HideMessageAfterDelay(messageText, messageDisplayTime));
    }

    private System.Collections.IEnumerator HideMessageAfterDelay(TMP_Text messageText, float delay)
    {
        yield return new WaitForSeconds(delay);
        HideMessageText(messageText);
    }

    private void HideMessageText(TMP_Text messageText)
    {
        if (messageText != null)
        {
            messageText.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
