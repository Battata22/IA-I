using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HunterBehaivour : MonoBehaviour
{
    [SerializeField] protected Vector3 _vel;
    public Vector3 Vel
    {
        get
        {
            return _vel;
        }
    }

    FSM _fsm;
    [SerializeField] MeshRenderer _meshRenderer;
    [SerializeField] float _speed;

    private void Awake()
    {
        GameManager.instance.Hunter = this;

        //_fsm = new FSM();

        ////add state rest
        //_fsm.AddState(HunterStates.Idle, new IdleState(_fsm, _meshRenderer));

        ////add state movement
        //_fsm.AddState(HunterStates.Movement, new MovementState(_fsm, transform, _speed));


        ////default
        //_fsm.ChangeState(HunterStates.Idle);
    }

    void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    void Update()
    {
        _fsm.FakeUpdate();
    }

    public enum HunterStates
    {
        Rest, Hunting, Patrol
    }

}
