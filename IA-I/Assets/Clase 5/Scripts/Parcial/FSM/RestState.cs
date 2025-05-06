using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;

public class RestState : IState
{

    //El cazador tiene un nivel de energía que se reduce mientras patrulla o persigue boids.

    //Cuando la energía llega a 0, el cazador deja de moverse y entra en Idle para descansar.

    //Luego de X segundos de descanso, su energía se recupera y vuelve a patrullar (Cambia al estado Patrol).

    FSM _fsm;
    float _maxEnergy;
    float _energy;
    float _energyDrain;
    Transform _transform;
    float _radiusBoidDetection;
    LayerMask _layerBoid;
    Vector3 _vel;
    Slider _energySlider;
    Action<Vector3> AddForce;
    Func<Vector3, Vector3> Seek;
    TextMeshProUGUI _textEstado;
    HunterBehaivour _hunterScript;

    public RestState(FSM fsm, float maxEnergy, float Energy, Transform transform, float radiusBoidDetection, 
        LayerMask layerBoid, float energyDrain, Vector3 vel, Slider energySlider, Action<Vector3> addForce, 
        Func<Vector3, Vector3> seek, TextMeshProUGUI _TextEstado, HunterBehaivour HunterScript)
    {
        _fsm = fsm;
        _maxEnergy = maxEnergy;
        _energy = Energy;
        _transform = transform;
        _radiusBoidDetection = radiusBoidDetection;
        _layerBoid = layerBoid;
        _energyDrain = energyDrain;
        _vel = vel;
        _energySlider = energySlider;
        AddForce = addForce;
        Seek = seek;
        _textEstado = _TextEstado;
        _hunterScript = HunterScript;
    }
    public void OnEnter()
    {
        //_energy = 0;
        _hunterScript._energy = 0;
        //_vel = Vector3.zero;
        AddForce(Seek(_transform.position));

        _textEstado.text = ("Estado Hunter: Rest");
    }

    public void OnExit()
    {

    }

    public void OnUpdate()
    {
        _hunterScript._energy += _energyDrain * Time.deltaTime * 2.5f;

        if(_hunterScript._energy >= _maxEnergy)
        {
            if (CheckNearbyBoids() != null)
            {
                //Debug.Log("estoy re cazando wacho");
                _fsm.ChangeState(HunterStates.Hunting);
            }
            else
            {
                _fsm.ChangeState(HunterStates.Patrol);
            }
        }

        _energySlider.value = _hunterScript._energy;

        AddForce(Seek(_transform.position));
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

}
