using UnityEngine;

public class Activate_COD_Quest : MonoBehaviour
{
    [Header("Panel Objects")]
    public GameObject[] typeAObjects; // Primer tipo de objetos del panel
    public GameObject[] typeBObjects; // Segundo tipo de objetos del panel

    [Header("Activation Settings")]
    public bool useTypeA = true; // Si es true, activa un objeto de Type A; si es false, activa de Type B
    public int activeObjectIndex = 0; // Índice del objeto a activar dentro del grupo seleccionado

    private GameObject currentActiveObject;

    void Start()
    {
        HideAllObjects();
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        ActivateSelectedObject();
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        HideAllObjects();
    }

    private void ActivateSelectedObject()
    {
        HideAllObjects();

        GameObject[] selectedList = useTypeA ? typeAObjects : typeBObjects;
        if (selectedList == null || selectedList.Length == 0)
            return;

        int index = Mathf.Clamp(activeObjectIndex, 0, selectedList.Length - 1);
        GameObject toActivate = selectedList[index];
        if (toActivate != null)
        {
            toActivate.SetActive(true);
            currentActiveObject = toActivate;
        }
    }

    private void HideAllObjects()
    {
        currentActiveObject = null;

        if (typeAObjects != null)
        {
            foreach (GameObject obj in typeAObjects)
            {
                if (obj != null)
                    obj.SetActive(false);
            }
        }

        if (typeBObjects != null)
        {
            foreach (GameObject obj in typeBObjects)
            {
                if (obj != null)
                    obj.SetActive(false);
            }
        }
    }
}
