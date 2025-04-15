using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class MovementState : IState
{
    FSM _fsm;
    Transform _target;
    float _speed;

    public MovementState(FSM fsm, Transform target, float speed)
    {
        _fsm = fsm;
        _target = target;
        _speed = speed;
    }
    public void OnEnter()
    {
        Debug.Log("entro a moverme");
    }

    public void OnExit()
    {
        Debug.Log("salgo a moverme");
    }

    public void OnUpdate()
    {
        _target.position += (_target.forward * Input.GetAxisRaw("Vertical") + _target.right * Input.GetAxisRaw("Horizontal")).normalized * (_speed * Time.deltaTime);

        if (Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0)
        {
            _fsm.ChangeState(AgentStates.Idle);
        }
    }

    public void OnTest() => Debug.Log("a");
}
