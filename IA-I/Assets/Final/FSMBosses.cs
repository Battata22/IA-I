using System;
using System.Collections.Generic;
using UnityEngine;

public class FSMBosses : MonoBehaviour
{
    IState _currentState;

    Dictionary<BossState, IState> _allStates = new();

    public void AddState(BossState newState, IState state)
    {

        if (_allStates.ContainsKey(newState)) return;

        _allStates.Add(newState, state);
    }

    public void ChangeState(BossState newState)
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

public class BossIdleState : IState
{
    FSMBosses _fsm;
    JefesBehaviour _jefesBehaviourScript;

    public BossIdleState(JefesBehaviour jefesBehaviour)
    {
        _jefesBehaviourScript = jefesBehaviour;
    }

    public void OnEnter()
    {
        _jefesBehaviourScript._velocity = Vector3.zero;
    }

    public void OnUpdate()
    {
        
    }

    public void OnExit()
    {

    }

}
//------------------------------------------------------------------------------------------------------------------------------------------------------

#region Following
//public class BossFollowingState : IState
//{
//    FSMBosses _fsm;
//    Transform _transform;
//    //Action<Vector3> _followPlayer;
//    [SerializeField, Range(5, 360)] float _viewAngle;
//    [SerializeField, Range(0.5f, 15)] float _viewRange;
//    LayerMask _obstacle;
//    Func<Vector3, bool> _inFOV;

//    public BossFollowingState(FSMBosses fsm, Transform Transform, Action<Vector3> FollowPlayer, float ViewAngle, float ViewRange, 
//        LayerMask obstacle, Func<Vector3, bool> inFOV)
//    {
//        _fsm = fsm;
//        _transform = Transform;
//        //_followPlayer = FollowPlayer;
//        _viewAngle = ViewAngle;
//        _viewRange = ViewRange;
//        _obstacle = obstacle;
//        _inFOV = inFOV;
//    }


//    public void OnEnter()
//    {

//    }

//    public void OnUpdate()
//    {
//        ////seek al player mientras este en el fov
//        //_followPlayer(ManagerParcial2.Instance.Player.transform.position);

//        ////alertar a los demas de la pos del player
//        //if (CheckMyType(_type).waitTimer >= _timer)
//        //{
//        //    ManagerParcial2.Instance.PlayerEvent.activarEventoFueraDeFOV = true;
//        //    CheckMyType(_type).waitTimer = 0;
//        //}

//        //if (!_inFOV(ManagerParcial2.Instance.Player.transform.position) && /*_state*/ CheckMyType(_type)._state == GhostState.Following)
//        //{
//        //    ManagerParcial2.Instance.PlayerEvent.activarEventoFueraDeFOV = true;

//        //    _fsm.ChangeState(GhostState.GoingLastSeen);
//        //    //_state = GhostState.GoingLastSeen;
//        //}
//    }

//    public void OnExit()
//    {

//    }

//} 
#endregion

//------------------------------------------------------------------------------------------------------------------------------------------------------

public class BossAttackingState : IState
{
    FSMBosses _fsm;
    Transform _transform;
    //Action<Vector3> _followPlayer;
    [SerializeField, Range(5, 360)] float _viewAngle;
    [SerializeField, Range(0.5f, 15)] float _viewRange;
    LayerMask _obstacle;
    Func<Vector3, bool> _inFOV;

    public BossAttackingState(FSMBosses fsm, Transform Transform, /*Action<Vector3> FollowPlayer,*/ float ViewAngle, float ViewRange,
        LayerMask obstacle, Func<Vector3, bool> inFOV)
    {
        _fsm = fsm;
        _transform = Transform;
        //_followPlayer = FollowPlayer;
        _viewAngle = ViewAngle;
        _viewRange = ViewRange;
        _obstacle = obstacle;
        _inFOV = inFOV;
    }


    public void OnEnter()
    {

    }

    public void OnUpdate()
    {
        ////seek al player mientras este en el fov
        //_followPlayer(ManagerParcial2.Instance.Player.transform.position);

        ////alertar a los demas de la pos del player
        //if (CheckMyType(_type).waitTimer >= _timer)
        //{
        //    ManagerParcial2.Instance.PlayerEvent.activarEventoFueraDeFOV = true;
        //    CheckMyType(_type).waitTimer = 0;
        //}

        //if (!_inFOV(ManagerParcial2.Instance.Player.transform.position) && /*_state*/ CheckMyType(_type)._state == GhostState.Following)
        //{
        //    ManagerParcial2.Instance.PlayerEvent.activarEventoFueraDeFOV = true;

        //    _fsm.ChangeState(GhostState.GoingLastSeen);
        //    //_state = GhostState.GoingLastSeen;
        //}
    }

    public void OnExit()
    {

    }

}

//------------------------------------------------------------------------------------------------------------------------------------------------------

public class BossRunningState : IState
{
    FSMBosses _fsm;
    Transform _transform;
    //Action<Vector3> _followPlayer;
    [SerializeField, Range(5, 360)] float _viewAngle;
    [SerializeField, Range(0.5f, 15)] float _viewRange;
    LayerMask _obstacle;
    Func<Vector3, bool> _inFOV;

    public BossRunningState(FSMBosses fsm, Transform Transform, /*Action<Vector3> FollowPlayer,*/ float ViewAngle, float ViewRange,
        LayerMask obstacle, Func<Vector3, bool> inFOV)
    {
        _fsm = fsm;
        _transform = Transform;
        //_followPlayer = FollowPlayer;
        _viewAngle = ViewAngle;
        _viewRange = ViewRange;
        _obstacle = obstacle;
        _inFOV = inFOV;
    }


    public void OnEnter()
    {

    }

    public void OnUpdate()
    {
        ////seek al player mientras este en el fov
        //_followPlayer(ManagerParcial2.Instance.Player.transform.position);

        ////alertar a los demas de la pos del player
        //if (CheckMyType(_type).waitTimer >= _timer)
        //{
        //    ManagerParcial2.Instance.PlayerEvent.activarEventoFueraDeFOV = true;
        //    CheckMyType(_type).waitTimer = 0;
        //}

        //if (!_inFOV(ManagerParcial2.Instance.Player.transform.position) && /*_state*/ CheckMyType(_type)._state == GhostState.Following)
        //{
        //    ManagerParcial2.Instance.PlayerEvent.activarEventoFueraDeFOV = true;

        //    _fsm.ChangeState(GhostState.GoingLastSeen);
        //    //_state = GhostState.GoingLastSeen;
        //}
    }

    public void OnExit()
    {

    }

}

//------------------------------------------------------------------------------------------------------------------------------------------------------

public class BossGoingToClickState : IState
{
    FSMBosses _fsm;

    int _indexTemp;
    Action _goToTempNode;
    public BossGoingToClickState(int indexTemp, Action goToTempNode)
    {
        _indexTemp = indexTemp;
        _goToTempNode = goToTempNode;
    }

    public void OnEnter()
    {
        _indexTemp = 0;
    }

    public void OnUpdate()
    {
        _goToTempNode(); 

        #region Old
        ////Hacer el camino de los nodos hasta el nodo mas cercano del punto ultima vez visto

        ////print("tamaño de la lista " + tempNodesFollow.Count + ". index " + _indexTemp);

        //_AddForce(_Arrive(CheckMyType(_type).tempNodesFollow[CheckMyType(_type)._indexTemp].transform.position));

        //if (Vector3.Distance(_transform.position, CheckMyType(_type).tempNodesFollow[CheckMyType(_type)._indexTemp].transform.position) < 0.1f)
        //{
        //    CheckMyType(_type)._indexTemp++;

        //    if (CheckMyType(_type)._indexTemp > CheckMyType(_type).tempNodesFollow.Count - 1)
        //    {
        //        #region Comment
        //        //canMove = false;
        //        //print("llegue al nodo temp");
        //        //hacer GoinBack 
        //        #endregion
        //        CheckMyType(_type)._indexTemp--;
        //        _fsm.ChangeState(GhostState.GoingBack);

        //        //_state = GhostState.GoingBack;
        //        _VolverABase();
        //    }
        //} 
        #endregion
    }

    public void OnExit()
    {

    }

}
