using UnityEngine;
using UnityEngine.SceneManagement; // Necesario para cambiar de escena

public sealed class MenuManagerVR : MonoBehaviour
{
    [Header("Paneles del Menú")]
    public GameObject panelPrincipal;
    public GameObject panelVolumen;
    public GameObject panelCreditos;

    [Header("Nombre de la Escena de Juego")]
    public string nombreEscenaJuego = "Prueba1";

    void Start()
    {
        // Al empezar, nos aseguramos de que solo el principal esté activo
        RegresarAlMenu();
    }

    // --- NAVEGACIÓN ---

    public void Jugar()
    {
        SceneManager.LoadScene(nombreEscenaJuego);
    }

    public void Salir()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }

    // --- CAMBIO DE PANELES ---

    public void MostrarVolumen()
    {
        ActivarSoloEstePanel(panelVolumen);
    }

    public void MostrarCreditos()
    {
        ActivarSoloEstePanel(panelCreditos);
    }

    public void RegresarAlMenu()
    {
        ActivarSoloEstePanel(panelPrincipal);
    }

    // Función auxiliar para no repetir código
    private void ActivarSoloEstePanel(GameObject panelAActivar)
    {
        panelPrincipal.SetActive(false);
        panelVolumen.SetActive(false);
        panelCreditos.SetActive(false);

        panelAActivar.SetActive(true);
    }
}
