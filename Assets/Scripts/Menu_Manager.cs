using UnityEngine;
using UnityEngine.XR;

public class Menu_Manager : MonoBehaviour
{
    public enum MenuState
    {
        Closed,
        Main,
        Volume,
        Credits
    }

    [Header("Menu Panels")]
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject volumePanel;
    [SerializeField] private GameObject creditsPanel;

    [Header("Pause Input")]
    [Tooltip("Nombre del botón de entrada clásico configurado en Input Manager.")]
    [SerializeField] private string pauseButtonName = "Menu";
    [Tooltip("Usar entrada XR para pausar con el botón del controlador VR.")]
    [SerializeField] private bool useXRInput = true;
    [SerializeField] private XRNode pauseControllerNode = XRNode.RightHand;
    [SerializeField] private XRButtonType pauseButtonType = XRButtonType.PrimaryButton;
    [SerializeField] private bool debugInput = true;

    private bool lastPauseButtonPressed = false;
    private bool isPaused = false;
    private MenuState currentMenuState = MenuState.Closed;

    private void Start()
    {
        if (debugInput)
        {
            Debug.Log("[Menu_Manager] Starting Menu Manager. Pause controller: " + pauseControllerNode + " / Button: " + pauseButtonType);
            if (mainPanel == null) Debug.LogWarning("[Menu_Manager] mainPanel no está asignado.");
            if (volumePanel == null) Debug.LogWarning("[Menu_Manager] volumePanel no está asignado.");
            if (creditsPanel == null) Debug.LogWarning("[Menu_Manager] creditsPanel no está asignado.");
        }
        ResumeGame();
    }

    private void Update()
    {
        if (CheckPauseInput())
        {
            TogglePause();
        }
    }

    private bool CheckPauseInput()
    {
#if ENABLE_LEGACY_INPUT_MANAGER
        if (!string.IsNullOrEmpty(pauseButtonName) && Input.GetButtonDown(pauseButtonName))
        {
            if (debugInput) Debug.Log("[Menu_Manager] Legacy input detected: " + pauseButtonName);
            return true;
        }
#else
        // Si se usa el Input System de Unity, se evita el llamado a Input.GetButtonDown.
        // Dejar el campo pauseButtonName solo para proyectos con Legacy Input Manager activo.
#endif

        if (useXRInput)
        {
            InputDevice device = InputDevices.GetDeviceAtXRNode(pauseControllerNode);
            if (!device.isValid)
            {
                if (debugInput) Debug.Log("[Menu_Manager] XR device invalid: " + pauseControllerNode);
                lastPauseButtonPressed = false;
                return false;
            }

            bool buttonPressed = false;
            switch (pauseButtonType)
            {
                case XRButtonType.MenuButton:
                    device.TryGetFeatureValue(CommonUsages.menuButton, out buttonPressed);
                    break;
                case XRButtonType.PrimaryButton:
                    device.TryGetFeatureValue(CommonUsages.primaryButton, out buttonPressed);
                    break;
                case XRButtonType.SecondaryButton:
                    device.TryGetFeatureValue(CommonUsages.secondaryButton, out buttonPressed);
                    break;
                case XRButtonType.GripButton:
                    device.TryGetFeatureValue(CommonUsages.gripButton, out buttonPressed);
                    break;
                case XRButtonType.TriggerButton:
                    device.TryGetFeatureValue(CommonUsages.triggerButton, out buttonPressed);
                    break;
            }

            if (buttonPressed && !lastPauseButtonPressed)
            {
                if (debugInput) Debug.Log("[Menu_Manager] XR button pressed edge detected: " + pauseButtonType);
                lastPauseButtonPressed = true;
                return true;
            }

            if (!buttonPressed && lastPauseButtonPressed)
            {
                lastPauseButtonPressed = false;
            }

            if (debugInput && !buttonPressed)
            {
                Debug.Log("[Menu_Manager] XR button not pressed: " + pauseButtonType);
            }
        }

        return false;
    }

    public void TogglePause()
    {
        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;
        ShowMainPanel();
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
        HideAllPanels();
    }

    public void ShowMainPanel()
    {
        SetMenuState(MenuState.Main);
    }

    public void ShowVolumePanel()
    {
        SetMenuState(MenuState.Volume);
    }

    public void ShowCreditsPanel()
    {
        SetMenuState(MenuState.Credits);
    }

    private void SetMenuState(MenuState newState)
    {
        currentMenuState = newState;
        mainPanel?.SetActive(newState == MenuState.Main);
        volumePanel?.SetActive(newState == MenuState.Volume);
        creditsPanel?.SetActive(newState == MenuState.Credits);
    }

    public void HideAllPanels()
    {
        currentMenuState = MenuState.Closed;
        mainPanel?.SetActive(false);
        volumePanel?.SetActive(false);
        creditsPanel?.SetActive(false);
    }

    public bool IsPaused => isPaused;
    public MenuState CurrentMenuState => currentMenuState;
}

public enum XRButtonType
{
    MenuButton,
    PrimaryButton,
    SecondaryButton,
    GripButton,
    TriggerButton
}
