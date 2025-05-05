using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;
using static UnityEngine.ParticleSystem;

public class PatrolState : IState
{

    //El cazador se mueve a través de un conjunto de waypoints.

    //Al llegar al último waypoint, puede volver al primero o hacer el recorrido en sentido inverso.

    //Si un boid entra en su rango de visión, cambia al estado Hunting.

    FSM _fsm;
    public PatrolState(FSM fsm)
    {
        _fsm = fsm;
    }

    public void OnEnter()
    {

    }

    public void OnExit()
    {

    }

    public void OnUpdate()
    {

    }

}
