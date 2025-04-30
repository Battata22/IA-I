using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {

            Vector3 mousePosition = Input.mousePosition;

            Ray ray = Camera.main.ScreenPointToRay(mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Vector3 spawnPosition = hit.point;


                Instantiate(enemyPrefab, new Vector3(spawnPosition.x, spawnPosition.y + 1, spawnPosition.z), Quaternion.identity);
            }
            else
            {
                Vector3 spawnPosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, 10f));
                spawnPosition.z = 0f;

                Instantiate(enemyPrefab, new Vector3(spawnPosition.x, spawnPosition.y + 1, spawnPosition.z), Quaternion.identity);
            }
        }
    }
}
