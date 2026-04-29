using UnityEngine;

public class Pulse_Manager : MonoBehaviour
{
    [Tooltip("Mesh que define la silueta a seguir")]
    public GameObject meshShape;

    [Tooltip("Collider del mesh que se usará para comprobar contacto")]
    public Collider meshCollider;

    [Tooltip("Objeto sencillo que recorre la forma del mesh")]
    public Transform follower;

    [Tooltip("Puntos que el seguidor debe recorrer en orden sobre la forma")]
    public Transform[] pathPoints = new Transform[0];

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

    void Start()
    {
        ResetChallenge();
    }

    void Update()
    {
        if (!challengeActive || challengeCompleted) return;

        if (!IsFollowerOnMesh())
        {
            statusText = "Fuera de la silueta: reiniciando...";
            ResetChallenge();
            return;
        }

        CheckCurrentPathPoint();
    }

    public void StartChallenge()
    {
        if (meshShape == null || follower == null || meshCollider == null)
        {
            statusText = "Faltan referencias: asigna meshShape, meshCollider y follower";
            return;
        }

        challengeActive = true;
        challengeCompleted = false;
        currentPointIndex = 0;
        pointsCompleted = 0;
        statusText = "Challenge iniciado";
    }

    public void ResetChallenge()
    {
        challengeActive = false;
        challengeCompleted = false;
        currentPointIndex = 0;
        pointsCompleted = 0;
        statusText = "Challenge listo para iniciar";
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

            if (currentPointIndex >= pathPoints.Length)
            {
                CompleteChallenge();
            }
        }
    }

    private void CompleteChallenge()
    {
        challengeCompleted = true;
        challengeActive = false;
        statusText = "Reto completado";
    }
}

