using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Pulse_Manager : MonoBehaviour
{
    [Tooltip("Mesh que define la silueta a seguir")]
    public GameObject meshShape;

    [Tooltip("Collider del mesh que se usará para comprobar contacto")]
    public Collider meshCollider;

    [Tooltip("Tag del objeto follower que recorre la forma del mesh")]
    public string followerTag = "spherINT";

    private Transform follower;

    [Tooltip("Puntos que el seguidor debe recorrer en orden sobre la forma")]
    public Transform[] pathPoints = new Transform[0];

    [Tooltip("Renderers de cada punto que mostrarán el feedback de color")]
    public Renderer[] pointRenderers = new Renderer[0];

    [Tooltip("Material que se usará cuando el punto sea correcto en orden")]
    public Material correctMaterial;

    [Tooltip("Material que se usará cuando el punto no sea correcto o aún no esté alcanzado")]
    public Material incorrectMaterial;

    [Tooltip("Material original que se restaura cuando se reinicia el reto")]
    public Material defaultMaterial;

    [Tooltip("Radio alrededor del punto para considerar que está recorrido")]
    public float pointRadius = 0.2f;

    [Tooltip("Distancia máxima al mesh para considerar que el objeto aún está en contacto")]
    public float maxDistanceFromMesh = 0.05f;

    [Tooltip("True si la mecánica está activa")]
    public bool challengeActive = false;

    [Tooltip("True si el reto se completó")]
    public bool challengeCompleted = false;

    [Tooltip("Índice del punto actual que debe alcanzar el seguidor")]
    public int currentPointIndex = 0;

    [Tooltip("Cantidad de puntos completados")]
    public int pointsCompleted = 0;

    [Tooltip("Texto de estado para ver en el Inspector")]
    public string statusText;

    [Tooltip("Objeto de texto para mostrar feedback en pantalla")]
    public TMP_Text feedbackTextTMP;

    [Tooltip("Texto UI alternativo si no se usa TextMeshPro")]
    public Text feedbackTextUI;

    [Tooltip("Referencia al Quest_Manager para actualizar el estado de la misión al completar el reto")]
    public Quest_Manager questManager;

    private string eventLog;
    private Material[] originalMaterials;

    void Start()
    {
        FindFollowerByTag();
        InitializePointFeedback();
        ResetChallenge();
    }

    void Update()
    {
        if (!challengeActive || challengeCompleted)
        {
            UpdateFeedbackText();
            return;
        }

        if (!IsFollowerOnMesh())
        {
            statusText = "Fuera de la silueta: reiniciando...";
            eventLog = "Fuera de la silueta. Se reinicia el reto.";
            ResetChallenge();
            return;
        }

        CheckCurrentPathPoint();
        UpdateFeedbackText();
    }

    public void StartChallenge()
    {
        // Buscar el follower si no se encontró en Start()
        if (follower == null)
        {
            FindFollowerByTag();
        }

        if (meshShape == null || follower == null || meshCollider == null)
        {
            statusText = "Faltan referencias: asigna meshShape y meshCollider. Verifica que exista un objeto con tag " + followerTag;
            eventLog = "No se pudo iniciar el challenge. Falta el follower con tag: " + followerTag;
            UpdateFeedbackText();
            return;
        }

        challengeActive = true;
        challengeCompleted = false;
        currentPointIndex = 0;
        pointsCompleted = 0;
        statusText = "Challenge iniciado";
        eventLog = "El reto está activo. Sigue los puntos en orden.";
        UpdateFeedbackText();
    }

    public void ResetChallenge()
    {
        challengeActive = false;
        challengeCompleted = false;
        currentPointIndex = 0;
        pointsCompleted = 0;
        statusText = "Challenge listo para iniciar";
        eventLog = "Reto reiniciado. Presiona iniciar para comenzar.";
        UpdatePointStates();
        UpdateFeedbackText();
    }

    private void InitializePointFeedback()
    {
        if (pointRenderers == null || pointRenderers.Length == 0)
            return;

        originalMaterials = new Material[pointRenderers.Length];

        for (int i = 0; i < pointRenderers.Length; i++)
        {
            if (pointRenderers[i] != null)
            {
                originalMaterials[i] = pointRenderers[i].sharedMaterial;
            }
        }

        UpdatePointStates();
    }

    private void UpdatePointStates()
    {
        if (pathPoints == null || pointRenderers == null)
            return;

        int count = Mathf.Min(pathPoints.Length, pointRenderers.Length);

        for (int i = 0; i < count; i++)
        {
            if (pointRenderers[i] == null)
                continue;

            if (i < currentPointIndex)
            {
                SetPointMaterial(i, true);
            }
            else
            {
                SetPointMaterial(i, false);
            }
        }
    }

    private void SetPointMaterial(int index, bool isCorrect)
    {
        if (pointRenderers == null || index < 0 || index >= pointRenderers.Length)
            return;

        Renderer renderer = pointRenderers[index];
        if (renderer == null)
            return;

        if (isCorrect)
        {
            if (correctMaterial != null)
            {
                renderer.material = correctMaterial;
            }
            else
            {
                renderer.material.color = Color.yellow;
            }
        }
        else
        {
            if (incorrectMaterial != null)
            {
                renderer.material = incorrectMaterial;
            }
            else
            {
                renderer.material.color = Color.red;
            }
        }
    }

    private void ResetPointMaterial(int index)
    {
        if (pointRenderers == null || index < 0 || index >= pointRenderers.Length)
            return;

        Renderer renderer = pointRenderers[index];
        if (renderer == null)
            return;

        if (defaultMaterial != null)
        {
            renderer.material = defaultMaterial;
        }
        else if (originalMaterials != null && index < originalMaterials.Length && originalMaterials[index] != null)
        {
            renderer.material = originalMaterials[index];
        }
    }

    private void FindFollowerByTag()
    {
        GameObject followerObject = GameObject.FindWithTag(followerTag);
        if (followerObject != null)
        {
            follower = followerObject.transform;
            statusText = $"Follower encontrado: {followerObject.name}";
        }
        else
        {
            follower = null;
            statusText = $"Follower no encontrado. Verifica que exista un objeto con tag '{followerTag}'";
        }
    }

    private void UpdateFeedbackText()
    {
        string stateLabel = challengeCompleted ? "Completado" : (challengeActive ? "Iniciado" : "No iniciado");
        string pointsLabel = pathPoints != null && pathPoints.Length > 0 ? $"Puntos: {pointsCompleted}/{pathPoints.Length}" : "Sin puntos definidos";
        string currentTarget = !challengeCompleted && pathPoints != null && pathPoints.Length > 0 && currentPointIndex < pathPoints.Length ? $"Siguiente punto: {currentPointIndex + 1}" : string.Empty;

        string fullText = $"Estado: {stateLabel}\n{pointsLabel}\n{currentTarget}\n{statusText}\n{eventLog}".Trim();

        if (feedbackTextTMP != null)
        {
            feedbackTextTMP.text = fullText;
        }

        if (feedbackTextUI != null)
        {
            feedbackTextUI.text = fullText;
        }
    }

    private bool IsFollowerOnMesh()
    {
        if (meshCollider == null || follower == null) return false;

        Vector3 closest = meshCollider.ClosestPoint(follower.position);
        float distance = Vector3.Distance(closest, follower.position);
        return distance <= maxDistanceFromMesh;
    }

    private void CheckCurrentPathPoint()
    {
        if (pathPoints == null || pathPoints.Length == 0) return;
        if (currentPointIndex >= pathPoints.Length) return;

        Transform targetPoint = pathPoints[currentPointIndex];
        if (targetPoint == null) return;

        float dist = Vector3.Distance(follower.position, targetPoint.position);
        if (dist <= pointRadius)
        {
            pointsCompleted++;
            currentPointIndex++;
            statusText = $"Punto {pointsCompleted}/{pathPoints.Length} completado";
            eventLog = $"Punto {pointsCompleted} alcanzado en orden.";
            UpdatePointStates();
            UpdateFeedbackText();

            if (currentPointIndex >= pathPoints.Length)
            {
                CompleteChallenge();
            }
        }
        else
        {
            eventLog = $"Avanza hacia el punto {currentPointIndex + 1} en orden.";
            UpdateFeedbackText();
        }
    }

    private void CompleteChallenge()
    {
        challengeCompleted = true;
        challengeActive = false;
        statusText = "Reto completado";
        eventLog = "Todos los puntos fueron alcanzados en orden.";

        if (questManager != null)
        {
            questManager.SetQuestState(4, true);
        }

        UpdateFeedbackText();
    }
}

