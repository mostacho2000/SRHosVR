using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuertaMuerta1 : MonoBehaviour
{
    
    public void DestruirPuerta()
    {
        Debug.Log("Puerta destruida");
        Destroy(gameObject);
    }
}

