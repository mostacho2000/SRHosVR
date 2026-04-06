using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class Computadora1 : MonoBehaviour
{
    public GameObject panelADesactivar;
    public Camera camaraJugador;
    public Camera camaraComputadora;
    private bool estaCercaDelJugador;
    private bool panelActivado;

    private FirstPersonController fpsController;

    private void Start()
    {
        fpsController = camaraJugador.GetComponentInParent<FirstPersonController>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && estaCercaDelJugador)
        {
            if (camaraJugador != null && camaraComputadora != null)
            {
                bool isUsingComputer = camaraComputadora.enabled;

                camaraJugador.enabled = isUsingComputer;
                camaraComputadora.enabled = !isUsingComputer;

                Cursor.visible = !isUsingComputer;
                Cursor.lockState = isUsingComputer ? CursorLockMode.Locked : CursorLockMode.None;

                if (fpsController != null)
                {
                    if (isUsingComputer)
                    {
                        fpsController.RestoreSpeeds();
                    }
                    else
                    {
                        fpsController.SetSpeedsToZero();
                    }
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("EnteredSpace");
            estaCercaDelJugador = true;
            panelADesactivar.SetActive(true);
            panelActivado = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("ExitSpace");
            estaCercaDelJugador = false;
            panelADesactivar.SetActive(false);
            panelActivado = false;
        }
    }
}