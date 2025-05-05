using System.Collections;
using System.Collections.Generic;
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

    public HuntingState(FSM fsm)
    {
        _fsm = fsm;
    }
    public void OnEnter()
    {
        //empiezo a cazar
    }

    public void OnExit()
    {
        //dejo de cazar
    }

    public void OnUpdate()
    {
        //chequear dentro de mi rango de vision si hay boid dentro -> seguir a quien tenga mas cerca de mi rango de vision
    }
}
