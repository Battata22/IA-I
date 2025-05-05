using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class RestState : IState
{

    //El cazador tiene un nivel de energía que se reduce mientras patrulla o persigue boids.

    //Cuando la energía llega a 0, el cazador deja de moverse y entra en Idle para descansar.

    //Luego de X segundos de descanso, su energía se recupera y vuelve a patrullar (Cambia al estado Patrol).

    FSM _fsm;

    public RestState(FSM fsm)
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
