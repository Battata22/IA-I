using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM_Agent : MonoBehaviour
{
    FSM _fsm;
    [SerializeField] MeshRenderer _meshRenderer;
    [SerializeField] float _speed;

    private void Awake()
    {
        _fsm = new FSM();

        //add state idle
        _fsm.AddState(AgentStates.Idle, new IdleState(_fsm, _meshRenderer));

        //add state movement
        _fsm.AddState(AgentStates.Movement, new MovementState(_fsm, transform,_speed));


        //default
        _fsm.ChangeState(AgentStates.Idle);
    }

    void Start()
    {
        
    }
    
    void Update()
    {
        _fsm.FakeUpdate();
    }
}

public enum AgentStates
{
    Idle, Movement
}
