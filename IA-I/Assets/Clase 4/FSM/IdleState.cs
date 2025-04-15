using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IState
{
    FSM _fsm;
    MeshRenderer _meshRenderer;

    public IdleState(FSM fsm, MeshRenderer meshRenderer)
    {
        _fsm = fsm;
        _meshRenderer = meshRenderer;
    }
    public void OnEnter()
    {
        _meshRenderer.material.color = Color.white;
        Debug.Log("entro a idle");
    }

    public void OnExit()
    {
        _meshRenderer.material.color = Color.red;
        Debug.Log("salgo a idle");
    }

    public void OnUpdate()
    {
        if(Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {
            _fsm.ChangeState(AgentStates.Movement);
        }
    }
}
