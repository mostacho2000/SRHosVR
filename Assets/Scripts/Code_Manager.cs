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
    }

    // Método para agregar un número al input field
    private void AddNumber(string num)
    {
        inputField.text += num;
    }

    // Método para verificar el código
    private void CheckCode()
    {
        if (inputField.text.Length == correctCode.Length)
        {
            if (inputField.text == correctCode)
            {
                onCorrect.Invoke();
                Debug.Log("Código correcto");
            }
            else
            {
                onIncorrect.Invoke();
                Debug.Log("Código incorrecto");
            }
            // Opcional: resetear el input después de verificar
            // inputField.text = "";
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
