using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class PlacementTrigger : MonoBehaviour
{
    [Header("Configuración de la colocación")]
    [Tooltip("Objeto que debe colocarse dentro de este trigger para marcarlo como colocado correctamente.")]
    public Transform requiredPart;

    [Tooltip("Si está activado, la colocación se considera correcta solo cuando el objeto coincide con requiredPart.")]
    public bool requireExactObject = true;

    [Tooltip("Si se usa una etiqueta, cualquier objeto con esta etiqueta será aceptado en lugar de requiredPart.")]
    public string requiredTag;

    [Header("Feedback Visual")]
    [Tooltip("Renderer del mesh que mostrará el feedback visual (verde/rojo).")]
    public Renderer feedbackRenderer;

    [Tooltip("Material a mostrar cuando se detecta el objeto correcto.")]
    public Material correctMaterial;

    [Tooltip("Material a mostrar cuando se detecta un objeto incorrecto.")]
    public Material incorrectMaterial;

    [Tooltip("Material original del mesh (se restaura al estar vacío).")]
    public Material defaultMaterial;

    [Header("Eventos")]
    public UnityEvent onPartPlaced;
    public UnityEvent onPartRemoved;

    [HideInInspector]
    public bool isPartPlaced;

    private bool hasObjectInside = false;
    private bool lastObjectWasCorrect = false;

    private void Reset()
    {
        Collider collider = GetComponent<Collider>();
        collider.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (IsCorrectObject(other))
        {
            isPartPlaced = true;
            hasObjectInside = true;
            lastObjectWasCorrect = true;
            UpdateFeedbackMaterial(true);
            onPartPlaced?.Invoke();
        }
        else if (other.CompareTag("Draggable") || other.GetComponentInParent<Rigidbody>() != null)
        {
            // Objeto incorrecto detectado
            hasObjectInside = true;
            lastObjectWasCorrect = false;
            UpdateFeedbackMaterial(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (IsCorrectObject(other))
        {
            isPartPlaced = false;
            hasObjectInside = false;
            ResetFeedbackMaterial();
            onPartRemoved?.Invoke();
        }
        else if (other.CompareTag("Draggable") || other.GetComponentInParent<Rigidbody>() != null)
        {
            // Objeto incorrecto removido
            if (!isPartPlaced)
            {
                hasObjectInside = false;
                ResetFeedbackMaterial();
            }
        }
    }

    private bool IsCorrectObject(Collider other)
    {
        if (!string.IsNullOrEmpty(requiredTag) && other.CompareTag(requiredTag))
        {
            return true;
        }

        if (requireExactObject && requiredPart != null)
        {
            Transform otherTransform = other.transform;
            if (otherTransform == requiredPart || otherTransform.IsChildOf(requiredPart))
                return true;

            if (other.attachedRigidbody != null)
            {
                Transform rigidbodyTransform = other.attachedRigidbody.transform;
                if (rigidbodyTransform == requiredPart || rigidbodyTransform.IsChildOf(requiredPart))
                    return true;
            }

            return false;
        }

        return !requireExactObject;
    }

    /// <summary>
    /// Actualiza el material del renderer de feedback.
    /// </summary>
    private void UpdateFeedbackMaterial(bool isCorrect)
    {
        if (feedbackRenderer == null)
        {
            Debug.LogWarning($"[PlacementTrigger] El feedbackRenderer no está asignado en {gameObject.name}");
            return;
        }

        Material materialToUse = isCorrect ? correctMaterial : incorrectMaterial;

        if (materialToUse == null)
        {
            Debug.LogWarning($"[PlacementTrigger] El material {(isCorrect ? "correcto" : "incorrecto")} no está asignado en {gameObject.name}");
            return;
        }

        // Cambiar el material
        feedbackRenderer.material = materialToUse;
    }

    /// <summary>
    /// Restaura el material original del feedback renderer.
    /// </summary>
    private void ResetFeedbackMaterial()
    {
        if (feedbackRenderer == null)
            return;

        if (defaultMaterial != null)
        {
            feedbackRenderer.material = defaultMaterial;
        }
        else
        {
            Debug.LogWarning($"[PlacementTrigger] El defaultMaterial no está asignado en {gameObject.name}");
        }
    }

    /// <summary>
    /// Devuelve el estado actual del trigger.
    /// </summary>
    public void SetFeedbackMaterials(Material correct, Material incorrect, Material defaultMat)
    {
        correctMaterial = correct;
        incorrectMaterial = incorrect;
        defaultMaterial = defaultMat;
    }
}
