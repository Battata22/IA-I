using System;
using System.Collections.Generic;
using UnityEngine;

public class FSMGhosty : MonoBehaviour
{
    IState _currentState;

    Dictionary<GhostState, IState> _allStates = new();

    public void AddState(GhostState newState, IState state)
    {

        if (_allStates.ContainsKey(newState)) return;

        _allStates.Add(newState, state);
    }

    public void ChangeState(GhostState newState)
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

public class GhostPatrolState : IState
{
    FSMGhosty _fsm;
    Transform _transform;
    Action<Vector3> _AddForce;
    Func<Vector3, Vector3> _Arrive;
    Node[] _nodosBase;
    int _nextNode;
    Node _ultimoNodo;

    public GhostPatrolState(FSMGhosty fsm, Transform Transform, Action<Vector3> AddForce, Func<Vector3, Vector3> Arrive, Node[] nodosBase, int nextNode, Node ultimoNodo
        )
    {
        _fsm = fsm;
        _AddForce = AddForce;
        _Arrive = Arrive;
        _nodosBase = nodosBase;
        _nextNode = nextNode;
        _ultimoNodo = ultimoNodo;
    }

    public void OnEnter()
    {

    }

    public void OnUpdate()
    {
        _AddForce(_Arrive(_nodosBase[_nextNode].transform.position));

        if (Vector3.Distance(_transform.position, _nodosBase[_nextNode].transform.position) < 0.1f)
        {
            _ultimoNodo = _nodosBase[_nextNode];
            _nextNode++;
            if (_nextNode > _nodosBase.Length - 1)
            {
                _nextNode = 0;
            }
        }
    }

    public void OnExit()
    {

    }
}
//------------------------------------------------------------------------------------------------------------------------------------------------------
public class GhostFollowingState : IState
{
    FSMGhosty _fsm;
    Transform _transform;
    Action<Vector3> _followPlayer;
    FOV_Target _player;
    float _waitTimer;
    float _timer;
    GhostState _state;
    //Action<bool> _inFOV;
    [SerializeField, Range(5, 360)] float _viewAngle;
    [SerializeField, Range(0.5f, 15)] float _viewRange;
    LayerMask _obstacle;

    bool _inFOV(Vector3 endPos)
    {
        Vector3 dir = endPos - _transform.position;

        if (dir.magnitude > _viewRange) return false;

        //if (Vector3.Angle(-transform.up, dir) > _viewAngle / 2) return false;

        if (Vector3.Angle(_transform.forward, dir) > _viewAngle / 2) return false;

        if (!_InLOS(_transform.position, endPos)) return false;

        return true;
    }
    bool _InLOS(Vector3 start, Vector3 end)
    {
        Vector3 dir = end - start;

        return !Physics.Raycast(start, dir.normalized, dir.magnitude, _obstacle);
    }

    public GhostFollowingState(FSMGhosty fsm, Transform Transform, Action<Vector3> FollowPlayer, FOV_Target player, float WaitTimer, float timer, GhostState state, /*Action<bool> inFOV*/
        float ViewAngle, float ViewRange, LayerMask obstacle, bool inFOV, bool InLOS)
    {
        _fsm = fsm;
        _transform = Transform;
        _followPlayer = FollowPlayer;
        _player = player;
        _waitTimer = WaitTimer;
        _timer = timer;
        _state = state;
        //_inFOV = inFOV;
        _viewAngle = ViewAngle;
        _viewRange = ViewRange;
        _obstacle = obstacle;
        //_inFOV = inFOV;


    }


    public void OnEnter()
    {

    }

    public void OnUpdate()
    {
        //seek al player mientras este en el fov
        _followPlayer(_player.transform.position);

        //alertar a los demas de la pos del player
        if (_waitTimer >= _timer)
        {
            ManagerParcial2.Instance.PlayerEvent.activarEventoFueraDeFOV = true;
            _waitTimer = 0;
        }

        //if (!_inFOV(_player.transform.position) && _state == GhostState.Following)
        //{
        //    ManagerParcial2.Instance.PlayerEvent.activarEventoFueraDeFOV = true;
        //    _state = GhostState.GoingLastSeen;
        //}
    }

    public void OnExit()
    {

    }
}
//------------------------------------------------------------------------------------------------------------------------------------------------------
public class GhostGoingBackState : IState
{
    FSMGhosty _fsm;
    Transform _transform;
    Action<Vector3> _AddForce;
    Func<Vector3, Vector3> _Arrive;
    Node[] _nodosBase;
    int _nextNode;
    Node _ultimoNodo;
    public GhostGoingBackState(FSMGhosty fsm, Transform Transform, Action<Vector3> AddForce, Func<Vector3, Vector3> Arrive, Node[] nodosBase, int nextNode, Node ultimoNodo
    )
    {
        _fsm = fsm;
        _AddForce = AddForce;
        _Arrive = Arrive;
        _nodosBase = nodosBase;
        _nextNode = nextNode;
        _ultimoNodo = ultimoNodo;
    }

    public void OnEnter()
    {

    }

    public void OnUpdate()
    {

    }

    public void OnExit()
    {

    }
}
//------------------------------------------------------------------------------------------------------------------------------------------------------
public class GhostGoingLastSeenState : IState
{
    FSMGhosty _fsm;
    Transform _transform;
    Action<Vector3> _AddForce;
    Func<Vector3, Vector3> _Arrive;
    Node[] _nodosBase;
    int _nextNode;
    Node _ultimoNodo;
    public GhostGoingLastSeenState(FSMGhosty fsm, Transform Transform, Action<Vector3> AddForce, Func<Vector3, Vector3> Arrive, Node[] nodosBase, int nextNode, Node ultimoNodo
    )
    {
        _fsm = fsm;
        _AddForce = AddForce;
        _Arrive = Arrive;
        _nodosBase = nodosBase;
        _nextNode = nextNode;
        _ultimoNodo = ultimoNodo;
    }

    public void OnEnter()
    {

    }

    public void OnUpdate()
    {

    }

    public void OnExit()
    {

    }
}
