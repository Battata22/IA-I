using System;
using System.Collections.Generic;
using UnityEngine;

public class FSMMinions : MonoBehaviour
{
    IState _currentState;

    Dictionary<MinionState, IState> _allStates = new();

    public void AddState(MinionState newState, IState state)
    {

        if (_allStates.ContainsKey(newState)) return;

        _allStates.Add(newState, state);
    }

    public void ChangeState(MinionState newState)
    {
        if (_currentState != null) _currentState.OnExit();

        _currentState = _allStates[newState];
        _currentState?.OnEnter();
    }

    public void FakeUpdate()
    {
        if (_currentState != null) _currentState.OnUpdate();
    }
}

public class MinionIdleState : IState
{
    MinionBehaivour _minionScript;

    public MinionIdleState(MinionBehaivour minionScript)
    {
        _minionScript = minionScript;
    }

    public void OnEnter()
    {
        _minionScript._velocity = Vector3.zero;
        _minionScript._state = MinionState.Idle;
    }

    public void OnUpdate()
    {
        Debug.Log("Minion State Idle");
    }

    public void OnExit()
    {

    }

}

//------------------------------------------------------------------------------------------------------------------------------------------------------

public class MinionFlockingState : IState
{
    FSMMinions _fsm;
    MinionBehaivour _minionScript;

    public MinionFlockingState(FSMMinions fsm, MinionBehaivour minionScript)
    {
        _fsm = fsm;
        _minionScript = minionScript;
    }

    public void OnEnter()
    {
        _minionScript._state = MinionState.Flocking;
    }

    public void OnUpdate()
    {
        if (!_minionScript.CheckForBossLOS())
        {
            _fsm.ChangeState(MinionState.LookingForBoss);
        }
        else
        {
            _minionScript.Floking();
            //Debug.Log("minion en flocking");
        }
    }

    public void OnExit()
    {

    }

}

//------------------------------------------------------------------------------------------------------------------------------------------------------

public class MinionAttackingState : IState
{
    FSMMinions _fsm;
    MinionBehaivour _minionScript;
    List<FOV_Target> _otherAgents;
    GameObject _target;
    float _targetDistance = 10000;

    public MinionAttackingState(FSMMinions fsm, MinionBehaivour minionScript, List<FOV_Target> otherAgents)
    {
        _fsm = fsm;
        _minionScript = minionScript;
        _otherAgents = otherAgents;
    }


    public void OnEnter()
    {
        _minionScript._state = MinionState.Attacking;
        _minionScript._velocity = Vector3.zero;
        _target = null;
        _targetDistance = 10000;
    }

    public void OnUpdate()
    {
        if (_minionScript.CheckForEnemiesNearby())
        {
            _target = null;
            _targetDistance = 10000;

            foreach (var agent in _otherAgents)
            {
                if (Vector3.Distance(agent.transform.position, _minionScript.transform.position) <= _targetDistance)
                {
                    _targetDistance = Vector3.Distance(agent.transform.position, _minionScript.transform.position);
                    _target = agent.gameObject;
                }
            }

            _minionScript.Shoot(_target);

            Debug.Log("Minion State Attacking");
        }
        else
        {
            _fsm.ChangeState(MinionState.Flocking);
        }
    }

    public void OnExit()
    {

    }

}

//------------------------------------------------------------------------------------------------------------------------------------------------------

public class MinionRunningState : IState
{

    MinionBehaivour _minionScript;

    public MinionRunningState(MinionBehaivour minionScript)
    {
        _minionScript = minionScript;
    }


    public void OnEnter()
    {
        _minionScript.CalculateRunBase();
        _minionScript._state = MinionState.Running;
    }

    public void OnUpdate()
    {
        _minionScript.RunBase();
    }

    public void OnExit()
    {

    }

}

//------------------------------------------------------------------------------------------------------------------------------------------------------

public class MinionLookingForBossState : IState
{

    FSMBosses _fsm;
    MinionBehaivour _minionScript;

    public MinionLookingForBossState(MinionBehaivour minionScript)
    {
        _minionScript = minionScript;
    }

    public void OnEnter()
    {
        _minionScript.GoToBoss();
        _minionScript._state = MinionState.LookingForBoss;
    }

    public void OnUpdate()
    {
        _minionScript.GoToBossNode();
        //Debug.Log("looking");
    }

    public void OnExit()
    {

    }

}
