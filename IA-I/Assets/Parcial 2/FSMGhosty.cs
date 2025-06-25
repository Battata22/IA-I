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
    //int _nextNode;
    //Node _ultimoNodo;
    GhostType _type;

    public GhostPatrolState(FSMGhosty fsm, Transform Transform, Action<Vector3> AddForce, Func<Vector3, Vector3> Arrive, Node[] nodosBase, /*int nextNode, Node ultimoNodo*/
        GhostType type)
    {
        _fsm = fsm;
        _transform = Transform;
        _AddForce = AddForce;
        _Arrive = Arrive;
        _nodosBase = nodosBase;
        //_nextNode = nextNode;
        //_ultimoNodo = ultimoNodo;
        _type = type;
    }

    public void OnEnter()
    {
        
    }

    public void OnUpdate()
    {
        _AddForce(_Arrive(_nodosBase[CheckMyType(_type)._nextNode].transform.position));

        if (Vector3.Distance(_transform.position, _nodosBase[CheckMyType(_type)._nextNode].transform.position) < 0.1f)
        {
            CheckMyType(_type)._ultimoNodo = _nodosBase[CheckMyType(_type)._nextNode];
            CheckMyType(_type)._nextNode++;
            if (CheckMyType(_type)._nextNode > _nodosBase.Length - 1)
            {
                CheckMyType(_type)._nextNode = 0;
            }
        }
    }

    public void OnExit()
    {

    }

    Ghostly CheckMyType(GhostType type)
    {
        if (type == GhostType.White)
        {
            return ManagerParcial2.Instance.WhiteGhost;
        }
        else if (type == GhostType.Blue)
        {
            return ManagerParcial2.Instance.BlueGhost;
        }
        else if (type == GhostType.Red)
        {
            return ManagerParcial2.Instance.RedGhost;
        }
        else if (type == GhostType.Yellow)
        {
            return ManagerParcial2.Instance.YellowGhost;
        }
        else
        {
            return null;
        }
    }
}
//------------------------------------------------------------------------------------------------------------------------------------------------------
public class GhostFollowingState : IState
{
    FSMGhosty _fsm;
    Transform _transform;
    Action<Vector3> _followPlayer;
    //float _waitTimer;
    float _timer;
    //Action<bool> _inFOV;
    [SerializeField, Range(5, 360)] float _viewAngle;
    [SerializeField, Range(0.5f, 15)] float _viewRange;
    LayerMask _obstacle;
    Func<Vector3, bool> _inFOV;
    GhostType _type;


    //bool _inFOV(Vector3 endPos)
    //{
    //    Vector3 dir = endPos - _transform.position;

    //    if (dir.magnitude > _viewRange) return false;

    //    //if (Vector3.Angle(-transform.up, dir) > _viewAngle / 2) return false;

    //    if (Vector3.Angle(_transform.forward, dir) > _viewAngle / 2) return false;

    //    if (!_InLOS(_transform.position, endPos)) return false;

    //    return true;
    //}
    //bool _InLOS(Vector3 start, Vector3 end)
    //{
    //    Vector3 dir = end - start;

    //    return !Physics.Raycast(start, dir.normalized, dir.magnitude, _obstacle);
    //}

    public GhostFollowingState(FSMGhosty fsm, Transform Transform, Action<Vector3> FollowPlayer/*, float WaitTimer*/, float timer, /*Action<bool> inFOV*/
        float ViewAngle, float ViewRange, LayerMask obstacle, Func<Vector3, bool> inFOV, GhostType type)
    {
        _fsm = fsm;
        _transform = Transform;
        _followPlayer = FollowPlayer;
        //_waitTimer = WaitTimer;
        _timer = timer;
        _viewAngle = ViewAngle;
        _viewRange = ViewRange;
        _obstacle = obstacle;
        _inFOV = inFOV;
        _type = type;
    }


    public void OnEnter()
    {

    }

    public void OnUpdate()
    {
        //seek al player mientras este en el fov
        _followPlayer(ManagerParcial2.Instance.Player.transform.position);

        //alertar a los demas de la pos del player
        if (CheckMyType(_type).waitTimer >= _timer)
        {
            ManagerParcial2.Instance.PlayerEvent.activarEventoFueraDeFOV = true;
            CheckMyType(_type).waitTimer = 0;
        }

        if (!_inFOV(ManagerParcial2.Instance.Player.transform.position) && /*_state*/ CheckMyType(_type)._state == GhostState.Following)
        {
            ManagerParcial2.Instance.PlayerEvent.activarEventoFueraDeFOV = true;

            _fsm.ChangeState(GhostState.GoingLastSeen);
            //_state = GhostState.GoingLastSeen;
        }
    }

    public void OnExit()
    {

    }

    Ghostly CheckMyType(GhostType type)
    {
        if (type == GhostType.White)
        {
            return ManagerParcial2.Instance.WhiteGhost;
        }
        else if (type == GhostType.Blue)
        {
            return ManagerParcial2.Instance.BlueGhost;
        }
        else if (type == GhostType.Red)
        {
            return ManagerParcial2.Instance.RedGhost;
        }
        else if (type == GhostType.Yellow)
        {
            return ManagerParcial2.Instance.YellowGhost;
        }
        else
        {
            return null;
        }
    }
}
//------------------------------------------------------------------------------------------------------------------------------------------------------
public class GhostGoingBackState : IState
{
    FSMGhosty _fsm;
    Transform _transform;
    Action<Vector3> _AddForce;
    Func<Vector3, Vector3> _Arrive;
    //List<Node> _tempNodesFollow;
    //int _indexTemp;
    //int _nextNode;
    GhostType _type;
    public GhostGoingBackState(FSMGhosty fsm, Transform Transform, Action<Vector3> AddForce, Func<Vector3, Vector3> Arrive, /*List<Node> tempNodesFollow,
        int indexTemp, int nextNode*/ GhostType type)
    {
        _fsm = fsm;
        _transform = Transform;
        _AddForce = AddForce;
        _Arrive = Arrive;
        //_tempNodesFollow = tempNodesFollow;
        //_indexTemp = indexTemp;
        //_nextNode = nextNode;
        _type = type;
    }

    public void OnEnter()
    {

    }

    public void OnUpdate()
    {
        //ir al siguiente nodo del ultimo nodo que paso (si estaba en camino entre el nodo 1 al 2 entonces va al 2)

        _AddForce(_Arrive(CheckMyType(_type).tempNodesFollow[CheckMyType(_type)._indexTemp].transform.position));

        if (Vector3.Distance(_transform.position, CheckMyType(_type).tempNodesFollow[CheckMyType(_type)._indexTemp].transform.position) < 0.1f)
        {
            CheckMyType(_type)._indexTemp++;

            if (CheckMyType(_type)._indexTemp >= CheckMyType(_type).tempNodesFollow.Count)
            {
                _fsm.ChangeState(GhostState.Patrol);

                //_state = GhostState.Patrol;
                CheckMyType(_type)._nextNode = 1;
            }
        }
    }

    public void OnExit()
    {

    }

    Ghostly CheckMyType(GhostType type)
    {
        if (type == GhostType.White)
        {
            return ManagerParcial2.Instance.WhiteGhost;
        }
        else if (type == GhostType.Blue)
        {
            return ManagerParcial2.Instance.BlueGhost;
        }
        else if (type == GhostType.Red)
        {
            return ManagerParcial2.Instance.RedGhost;
        }
        else if (type == GhostType.Yellow)
        {
            return ManagerParcial2.Instance.YellowGhost;
        }
        else
        {
            return null;
        }
    }
}
//------------------------------------------------------------------------------------------------------------------------------------------------------
public class GhostGoingLastSeenState : IState
{
    FSMGhosty _fsm;
    Transform _transform;
    Action<Vector3> _AddForce;
    Func<Vector3, Vector3> _Arrive;
    //List<Node> _tempNodesFollow;
    //int _indexTemp;
    //int _nextNode;
    Action _VolverABase;
    GhostType _type;
    public GhostGoingLastSeenState(FSMGhosty fsm, Transform Transform, Action<Vector3> AddForce, Func<Vector3, Vector3> Arrive, /* List<Node> tempNodesFollow, 
        int indexTemp, int nextNode,*/ Action volverABase, GhostType ghostType)
    {
        _fsm = fsm;
        _transform = Transform;
        _AddForce = AddForce;
        _Arrive = Arrive;
        //_nextNode = nextNode;
        //_indexTemp = indexTemp;
        //_tempNodesFollow = tempNodesFollow;
        _VolverABase = volverABase;
        _type = ghostType;
    }

    public void OnEnter()
    {
        //if (CheckMyType(_type)._state != GhostState.GoingLastSeen)
        //CheckMyType(_type)._indexTemp = 0;
    }

    public void OnUpdate()
    {
        //Hacer el camino de los nodos hasta el nodo mas cercano del punto ultima vez visto

        //print("tamaño de la lista " + tempNodesFollow.Count + ". index " + _indexTemp);

        _AddForce(_Arrive(CheckMyType(_type).tempNodesFollow[CheckMyType(_type)._indexTemp].transform.position));

        if (Vector3.Distance(_transform.position, CheckMyType(_type).tempNodesFollow[CheckMyType(_type)._indexTemp].transform.position) < 0.1f)
        {
            CheckMyType(_type)._indexTemp++;

            if (CheckMyType(_type)._indexTemp > CheckMyType(_type).tempNodesFollow.Count - 1)
            {
                #region Comment
                //canMove = false;
                //print("llegue al nodo temp");
                //hacer GoinBack 
                #endregion
                CheckMyType(_type)._indexTemp--;
                _fsm.ChangeState(GhostState.GoingBack);

                //_state = GhostState.GoingBack;
                _VolverABase();
            }
        }
    }

    public void OnExit()
    {

    }

    Ghostly CheckMyType(GhostType type)
    {
        if (type == GhostType.White)
        {
            return ManagerParcial2.Instance.WhiteGhost;
        }
        else if (type == GhostType.Blue)
        {
            return ManagerParcial2.Instance.BlueGhost;
        }
        else if (type == GhostType.Red)
        {
            return ManagerParcial2.Instance.RedGhost;
        }
        else if (type == GhostType.Yellow)
        {
            return ManagerParcial2.Instance.YellowGhost;
        }
        else
        {
            return null;
        }
    }
}
