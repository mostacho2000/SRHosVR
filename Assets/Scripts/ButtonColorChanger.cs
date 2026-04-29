using UnityEngine;
using UnityEngine.UI;

public class ButtonColorChanger : MonoBehaviour
{
    public Colors_Manager colorsManager; // Referencia al script Colors_Manager
    public int buttonIndex; // Índice del botón (0 para el primero, 1 para el segundo, etc.)

    // Este método se llama cuando se presiona el botón
    public void OnButtonPressed()
    {
        if (colorsManager != null)
        {
            colorsManager.ChangeColor(buttonIndex);
        }
    }
}