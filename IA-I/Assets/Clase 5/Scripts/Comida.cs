using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Comida : MonoBehaviour
{
    public GameObject cuadradoPrefab;
    public float _spawnTime, _limiteX, _limiteZ;
    float waitSpawnRandom;

    void Update()
    {
        waitSpawnRandom += Time.deltaTime;

        SpawnMouse();

        RandomSpawn();
    }

    void RandomSpawn()
    {
        if (waitSpawnRandom >= _spawnTime)
        {
            var x = Random.Range(-_limiteX, _limiteX + 1);
            var z = Random.Range(-_limiteZ, _limiteZ + 1);

            Instantiate(cuadradoPrefab, new Vector3(x, 1, z), Quaternion.identity);

            waitSpawnRandom = 0;
        }
    }

    void SpawnMouse()
    {
        if (Input.GetMouseButtonDown(2))
        {
            Vector3 mousePosition = Input.mousePosition;

            Ray ray = Camera.main.ScreenPointToRay(mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Vector3 spawnPosition = hit.point;

                Instantiate(cuadradoPrefab, new Vector3(spawnPosition.x, spawnPosition.y + 1, spawnPosition.z), Quaternion.identity);
            }
            else
            {
                // Si el raycast no pega con nada lo instancia a una distancia de la camara
                Vector3 spawnPosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, 10f));
                spawnPosition.z = 0f;

                Instantiate(cuadradoPrefab, new Vector3(spawnPosition.x, spawnPosition.y + 1, spawnPosition.z), Quaternion.identity);
            }
        }
    }
}
