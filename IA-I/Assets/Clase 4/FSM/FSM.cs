using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM
{
    IState _currentState, _currentHunterState;
    Dictionary<AgentStates, IState> _allStates = new();
    Dictionary<HunterStates, IState> _hunterAllStates = new();


    public void AddState(AgentStates newState, IState state)
    {
        if (_allStates.ContainsKey(newState)) return;
        _allStates.Add(newState, state);
    }
    public void AddState(HunterStates newState, IState state)
    {
        if (_hunterAllStates.ContainsKey(newState)) return;
        _hunterAllStates.Add(newState, state);
    }



    public void ChangeState(AgentStates newState)
    {
        if(_currentState != null)
        {
            _currentState.OnExit();
        }

        _currentState = _allStates[newState];
        _currentState.OnEnter();
    }

    public void ChangeState(HunterStates newState)
    {
        if (_currentHunterState != null)
        {
            _currentHunterState.OnExit();
        }

        _currentHunterState = _hunterAllStates[newState];
        _currentHunterState.OnEnter();
    }

    public void FakeUpdate()
    {
        if (_currentState != null)
        {
            _currentState.OnUpdate();
        }

        if (_currentHunterState != null)
        {
            _currentHunterState.OnUpdate();
        }
    }

}
