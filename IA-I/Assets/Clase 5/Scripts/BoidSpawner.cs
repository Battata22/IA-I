using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidSpawner : MonoBehaviour
{
    public GameObject boidPrefab;
    public LayerMask _pisoMask;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {

            Vector3 mousePosition = Input.mousePosition;

            Ray ray = Camera.main.ScreenPointToRay(mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100000, _pisoMask))
            {
                Vector3 spawnPosition = hit.point;

                Instantiate(boidPrefab, new Vector3(spawnPosition.x, spawnPosition.y + 0.95f, spawnPosition.z), Quaternion.identity);
            }
            //else
            //{
            //    Vector3 spawnPosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, 10f));
            //    spawnPosition.z = 0f;

            //    Instantiate(boidPrefab, new Vector3(spawnPosition.x, spawnPosition.y + 1, spawnPosition.z), Quaternion.identity);
            //}
        }
    }
}
