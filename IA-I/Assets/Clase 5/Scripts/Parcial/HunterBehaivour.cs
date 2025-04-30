using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HunterBehaivour : MonoBehaviour
{


    [SerializeField] protected Vector3 _vel;

    public Vector3 Vel
    {
        get
        {
            return _vel;
        }
    }

    private void Awake()
    {
        GameManager.instance.Hunter = this;
    }

    void Start()
    {
        
    }

    
    void Update()
    {
        
    }
}
