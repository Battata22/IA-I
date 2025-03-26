using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Conejo : MonoBehaviour
{

    [SerializeField] Vector3 _vel;

    #region Floats
    [Space]
    [Header("<color=red>Stats</color>")]
    [SerializeField] float _maxSpd;
    [SerializeField,Range(0,1)] float _maxForce; 
    #endregion

    [SerializeField] Transform _target;


    void Start()
    {
        
    }


    void Update()
    {
        AddForce(CorreGato(_target.position));

        transform.position += _vel * Time.deltaTime;
        transform.forward = _vel;
    }

    public Vector3 Seek(Vector3 target)
    {
        //vel deseada, vel actual, steering
        Vector3 dir = target - transform.position;
        dir = dir.normalized;
        dir *=_maxSpd;

        Vector3 steering = dir - _vel;
        steering = Vector3.ClampMagnitude(steering, _maxForce);

        return steering;

    }

    //public Vector3 CorreGato(Vector3 target)
    //{
    //    return -Seek(target);
    //}

    //lo de arriba es lo mismo que abajo
    public Vector3 CorreGato(Vector3 target) => -Seek(target);


    void AddForce(Vector3 dir)
    {
        _vel = Vector3.ClampMagnitude(_vel + dir, _maxSpd);
    }
}
