using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerParcial2 : MonoBehaviour
{
    public static ManagerParcial2 Instance;

    public FOV_Target Player;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
}
