using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;
using static UnityEngine.ParticleSystem;

public class PatrolState : IState
{

    //El cazador se mueve a través de un conjunto de waypoints.

    //Al llegar al último waypoint, puede volver al primero o hacer el recorrido en sentido inverso.

    //Si un boid entra en su rango de visión, cambia al estado Hunting.

    FSM _fsm;

    Action<Vector3> AddForce;
    Func<Vector3, Vector3> Seek;


    Transform _transform;
    Transform[] _waypoints;
    float _radiusBoidDetection;
    LayerMask _layerBoid;
    int _indexWaypoint;
    Vector3 _vel;
    float _maxSpeed;
    [SerializeField] TextMeshProUGUI _textEstado;

    public PatrolState(FSM fsm, Transform transform, Transform[] waypoints, float radiusBoid,
        LayerMask layerBoids, Action<Vector3> addForce, Func<Vector3, Vector3> seek, 
        Vector3 velocity, float maxSpeed, TextMeshProUGUI _TextEstado)
    {
        _fsm = fsm;
        _transform = transform;
        _waypoints = waypoints;
        _radiusBoidDetection = radiusBoid;
        _layerBoid = layerBoids;
        AddForce = addForce;
        Seek = seek;
        _vel = velocity;
        _maxSpeed = maxSpeed;
        _textEstado = _TextEstado;
    }

    public void OnEnter()
    {
        _textEstado.text = ("Estado Hunter: Patrol");
    }

    public void OnExit()
    {
        _vel = Vector3.zero;
        AddForce(Seek(_vel));
    }

    public void OnUpdate()
    {
        //move waypoint
        MoveBetweenWaypoints();

        if (CheckNearbyBoids() != null)
        {
            //Debug.Log("estoy re cazando wacho");
            _fsm.ChangeState(HunterStates.Hunting);
        }

        //_transform.position = GameManager.instance.GetPosition(_transform.position + _vel * Time.deltaTime);

    }

    void MoveBetweenWaypoints()
    {        
        AddForce(Seek(_waypoints[_indexWaypoint].position));

        if (Vector3.Distance(_transform.position, _waypoints[_indexWaypoint].position) <= 2f)
        {
            _indexWaypoint++;
            if (_indexWaypoint >= _waypoints.Length)
            {
                _indexWaypoint = 0;
            } 
        }
    }

    float _lastClosestBoid = 10000;
    Transform _closestBoid;
    Transform CheckNearbyBoids()
    {
        Collider[] boids = Physics.OverlapSphere(_transform.position, _radiusBoidDetection, _layerBoid);
        
        if (boids.Length == 0)
        {
            _closestBoid = null;
            return _closestBoid;
        } 

        foreach (var boid in boids)
        {
            if (_lastClosestBoid > Vector3.Distance(boid.transform.position, _transform.position))
            {
                _lastClosestBoid = Vector3.Distance(boid.transform.position, _transform.position);
                _closestBoid = boid.transform;
            }
        }
        _lastClosestBoid = 10000;
        return _closestBoid;
    }

}
