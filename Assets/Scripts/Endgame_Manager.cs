using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Endgame_Manager : MonoBehaviour
{
    [Header("Audio Settings")]
    public AudioClip endgameSound; // Sonido a reproducir al entrar
    private AudioSource audioSource;

    [Header("Countdown Settings")]
    public TMP_Text countdownText; // TextMeshPro para mostrar la cuenta atrás
    public float countdownTime = 10f; // Tiempo de cuenta atrás en segundos
    private float currentCountdown;
    private bool countdownStarted = false;

    [Header("Scene Settings")]
    public int sceneIndexToLoad = 0; // Índice de la escena a cargar

    void Start()
    {
        // Obtener o agregar AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Inicializar cuenta atrás
        currentCountdown = countdownTime;

        // Ocultar texto inicialmente si está asignado
        if (countdownText != null)
        {
            countdownText.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (countdownStarted && currentCountdown > 0)
        {
            currentCountdown -= Time.deltaTime;

            // Actualizar el texto de la cuenta atrás
            if (countdownText != null)
            {
                countdownText.text = Mathf.Ceil(currentCountdown).ToString();
            }

            // Cuando llegue a 0, cargar la escena
            if (currentCountdown <= 0)
            {
                LoadNextScene();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Verificar si el objeto que entra es el player (asumiendo tag "Player")
        if (other.CompareTag("Player") && !countdownStarted)
        {
            StartCountdown();
        }
    }

    private void StartCountdown()
    {
        countdownStarted = true;

        // Reproducir sonido
        if (audioSource != null && endgameSound != null)
        {
            audioSource.PlayOneShot(endgameSound);
        }

        // Mostrar el texto de cuenta atrás
        if (countdownText != null)
        {
            countdownText.gameObject.SetActive(true);
        }
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(sceneIndexToLoad);
    }
}
