using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class HuntingState : IState
{

    //Si el cazador detecta un boid en su rango de visión, pasa a perseguirlo usando Pursuit o a dispararle.

    //Opción Pursuit: Se mueve de forma inteligente prediciendo la posición futura del boid.

    //Opción Shooting: Dispara con predicción de trayectoria para acertarle a un boid en movimiento
    //(Seria aplicar Pursuit también, pero en vez de movernos a esa dirección, apuntamos y disparamos).

    //Perseguir o disparar gasta energía, por lo que si la energía se agota, vuelve a descansar (Cambia al estado Idle/Rest).

    //Si pierde de vista al boid, vuelve al estado Patrol.

    FSM _fsm;
    float _energy;
    float _energyDrain;
    Transform _transform;
    float _radiusBoidDetection;
    LayerMask _layerBoid;
    Action<Vector3> AddForce;
    Func<BoidBehaivour, Vector3> Pursuit;
    Func<Vector3, Vector3> Seek;
    Slider _energySlider;
    float _maxEnergy;
    Vector3 _vel;
    TextMeshProUGUI _textEstado;
    HunterBehaivour _hunterScript;

    public HuntingState(FSM fsm, float restTime, Transform transform, float radiusBoidDetection, 
        LayerMask layerBoid, Action<Vector3> addForce, Func<BoidBehaivour, Vector3> pursuit, Func<Vector3, 
            Vector3> seek, float energyDrain, Slider energySlider, float maxEnergy, Vector3 vel, TextMeshProUGUI _TextEstado, HunterBehaivour HunterScript)
    {
        _fsm = fsm;
        _energy = restTime;
        _transform = transform;
        _radiusBoidDetection = radiusBoidDetection;
        _layerBoid = layerBoid;
        AddForce = addForce;
        Pursuit = pursuit;
        Seek = seek;
        _energyDrain = energyDrain;
        _energySlider = energySlider;
        _maxEnergy = maxEnergy;
        _vel = vel;
        _textEstado = _TextEstado;
        _hunterScript = HunterScript;
    }
    public void OnEnter()
    {
        _textEstado.text = ("Estado Hunter: Hunting");
    }

    public void OnExit()
    {

    }

    public void OnUpdate()
    {
        //Debug.Log("toy hunteando loco");

        if (_hunterScript._energy <= 0)
        {
            
            _fsm.ChangeState(HunterStates.Rest);
        }

        if (CheckNearbyBoids() != null)
        {
            AddForce(Seek(CalculateNearbyBoid()));
            _hunterScript._energy -= _energyDrain * Time.deltaTime;
        }
        else
        {
            _fsm.ChangeState(HunterStates.Patrol);
        }

        _energySlider.value = _hunterScript._energy;

        //_transform.position = GameManager.instance.GetPosition(_transform.position + _vel * Time.deltaTime);
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

    public Vector3 CalculateNearbyBoid()
    {
        var _boids = Physics.OverlapSphere(_transform.position, _radiusBoidDetection, _layerBoid);
        foreach (var boid in _boids)
        {
            if (_lastClosestBoid > Vector3.Distance(boid.transform.position, _transform.position))
            {
                _lastClosestBoid = Vector3.Distance(boid.transform.position, _transform.position);
                _closestBoid = boid.transform;
            }
        }
        _lastClosestBoid = 10000;
        return _closestBoid.position;
    }
}
