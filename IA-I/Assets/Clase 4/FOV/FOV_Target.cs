using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FOV_Target : MonoBehaviour
{
    [SerializeField] MeshRenderer _meshRenderer;   
    

    protected virtual void Start()
    {
        if(_meshRenderer == null)
        {
            _meshRenderer = GetComponent<MeshRenderer>();
        }
    }


    public void ChangeColor(Color color)
    {
        _meshRenderer.material.color = color;
    }
}
