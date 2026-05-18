using UnityEngine;
using UnityEngine.Events;

public class Parts_Manager : MonoBehaviour
{
    [Header("Partes y puntos objetivo")]
    [Tooltip("Asigna aquí los 5 objetos que deben colocarse correctamente.")]
    public Transform[] partObjects = new Transform[5];

    [Tooltip("Asigna aquí los 5 puntos de destino correspondientes a cada objeto.")]
    public Transform[] targetPoints = new Transform[5];

    [Header("Verificación por trigger")]
    [Tooltip("Si quieres usar triggers, asigna aquí los PlacementTrigger de cada zona de colocación.")]
    public PlacementTrigger[] placementTriggers = new PlacementTrigger[5];

    [Tooltip("Si está activado, la verificación usará placementTriggers en lugar de comparaciones de posición.")]
    public bool useTriggers = false;

    [Header("Tolerancias de colocación")]
    [Tooltip("Distancia máxima permitida entre un objeto y su punto objetivo para considerarlo correcto.")]
    public float maxDistance = 0.1f;

    [Tooltip("Ángulo máximo permitido entre la rotación del objeto y su punto objetivo.")]
    public float maxAngle = 15f;

    [Tooltip("Si está activado, también se comprueba la rotación de cada pieza.")]
    public bool useRotationCheck = false;

    [Header("Eventos")]
    public UnityEvent onAllPartsCorrect;
    public UnityEvent onPartsIncorrect;
    
    [Header("Referencias")]
    [Tooltip("Referencia al Quest_Manager para actualizar el estado del quest.")]
    public Quest_Manager questManager;

    [HideInInspector]
    public bool allPartsCorrect;

    private enum PlacementState
    {
        Unknown,
        Correct,
        Incorrect
    }

    private PlacementState currentPlacementState = PlacementState.Unknown;

    void Start()
    {
        ValidateConfiguration();
        EvaluatePlacement();
    }

    void Update()
    {
        EvaluatePlacement();
    }

    private void ValidateConfiguration()
    {
        if (useTriggers)
        {
            if (placementTriggers == null || placementTriggers.Length == 0)
            {
                Debug.LogWarning("[Parts_Manager] useTriggers está activo pero no se han asignado placementTriggers.");
            }
        }
        else
        {
            if (partObjects == null || targetPoints == null)
            {
                Debug.LogWarning("[Parts_Manager] Arrays de partes o puntos objetivo no asignados.");
            }
            else if (partObjects.Length != targetPoints.Length)
            {
                Debug.LogWarning("[Parts_Manager] Los arrays de partes y puntos objetivo deben tener el mismo tamaño.");
            }
        }
    }

    public void EvaluatePlacement()
    {
        bool allCorrect = true;

        if (useTriggers)
        {
            if (placementTriggers == null || placementTriggers.Length == 0)
            {
                allCorrect = false;
            }
            else
            {
                foreach (PlacementTrigger trigger in placementTriggers)
                {
                    if (trigger == null || !trigger.isPartPlaced)
                    {
                        allCorrect = false;
                        break;
                    }
                }
            }
        }
        else
        {
            if (partObjects == null || targetPoints == null || partObjects.Length != targetPoints.Length)
            {
                allCorrect = false;
            }
            else
            {
                for (int i = 0; i < partObjects.Length; i++)
                {
                    if (partObjects[i] == null || targetPoints[i] == null)
                    {
                        allCorrect = false;
                        continue;
                    }

                    if (!IsPartInCorrectPosition(partObjects[i], targetPoints[i]))
                    {
                        allCorrect = false;
                    }
                }
            }
        }

        allPartsCorrect = allCorrect;
        UpdatePlacementState(allCorrect);
    }

    private void UpdatePlacementState(bool allCorrect)
    {
        if (allCorrect)
        {
            if (currentPlacementState != PlacementState.Correct)
            {
                currentPlacementState = PlacementState.Correct;
                onAllPartsCorrect?.Invoke();
                if (questManager != null)
                {
                    questManager.SetQuestState(3, true);
                }
            }
        }
        else
        {
            if (currentPlacementState != PlacementState.Incorrect)
            {
                currentPlacementState = PlacementState.Incorrect;
                onPartsIncorrect?.Invoke();
            }
        }
    }

    private bool IsPartInCorrectPosition(Transform part, Transform target)
    {
        if (Vector3.Distance(part.position, target.position) > maxDistance)
        {
            return false;
        }

        if (!useRotationCheck)
        {
            return true;
        }

        return Quaternion.Angle(part.rotation, target.rotation) <= maxAngle;
    }

    public bool AreAllPartsPlaced()
    {
        return allPartsCorrect;
    }
}
