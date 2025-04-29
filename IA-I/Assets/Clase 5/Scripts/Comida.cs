using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Comida : MonoBehaviour
{
    public GameObject cuadradoPrefab;

    void Update()
    {
        if (Input.GetMouseButtonDown(2))
        {
            Vector3 mousePosition = Input.mousePosition;

            Ray ray = Camera.main.ScreenPointToRay(mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Vector3 spawnPosition = hit.point;

                Instantiate(cuadradoPrefab, spawnPosition, Quaternion.identity);
            }
            else
            {
                // Si el raycast no pega con nada lo instancia a una distancia de la camara
                Vector3 spawnPosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, 10f));
                spawnPosition.z = 0f; 

                Instantiate(cuadradoPrefab, spawnPosition, Quaternion.identity);
            }
        }
    }
}
