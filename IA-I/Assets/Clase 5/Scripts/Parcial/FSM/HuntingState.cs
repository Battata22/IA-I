using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class HuntingState : IState
{

    //Si el cazador detecta un boid en su rango de visi�n, pasa a perseguirlo usando Pursuit o a dispararle.

    //Opci�n Pursuit: Se mueve de forma inteligente prediciendo la posici�n futura del boid.

    //Opci�n Shooting: Dispara con predicci�n de trayectoria para acertarle a un boid en movimiento
    //(Seria aplicar Pursuit tambi�n, pero en vez de movernos a esa direcci�n, apuntamos y disparamos).

    //Perseguir o disparar gasta energ�a, por lo que si la energ�a se agota, vuelve a descansar (Cambia al estado Idle/Rest).

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
