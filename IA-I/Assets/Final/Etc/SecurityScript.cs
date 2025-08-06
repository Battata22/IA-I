using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecurityScript : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<MinionBehaivour>() != null)
        {
            other.GetComponent<MinionBehaivour>().ObstacleActivo = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<MinionBehaivour>() != null)
        {
            other.GetComponent<MinionBehaivour>().ObstacleActivo = false;
        }
    }
}
