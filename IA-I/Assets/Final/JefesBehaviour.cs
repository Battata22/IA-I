using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class JefesBehaviour : FOV_Agent
{
    [SerializeField] BossTeam _team;
    [SerializeField] Node _tempNodeNaranja;
    [SerializeField] Node _tempNodeCeleste;

    Pathfinding _path;
    FSMBosses _fsm;
    public BossState _state;

    [SerializeField] float _speed;
    [SerializeField] float _maxSpeed;
    [SerializeField] public Vector3 _velocity;
    [SerializeField, Range(0, 1)] public float _visionRadius;
    [SerializeField, Range(0, 1)] float _maxForce;
    public Vector3 Velocity { get { return _velocity; } }

    public Node _ultimoNodo;

    public int _nextNode = 0;

    public List<Node> tempNodesFollow;
    public int _indexTemp;

    private void Awake()
    {
        _fsm = new FSMBosses();

        //add state Following
        _fsm.AddState(BossState.Idle, new BossIdleState(this));

        //add state Patrol
        _fsm.AddState(BossState.GoingToClick, new BossGoingToClickState(_indexTemp, GoToTempNode));

        //add state GoingBack
        _fsm.AddState(BossState.Attacking, new BossAttackingState(_fsm, transform, _viewAngle, _viewRange, _obstacle ,inFOV));

        //add state GoingLastSeen
        _fsm.AddState(BossState.Running, new BossRunningState(_fsm, transform, _viewAngle, _viewRange, _obstacle, inFOV));


        //default
        _fsm.ChangeState(BossState.Idle);
    }

    protected override void Start()
    {
        _path = new Pathfinding();
        _state = BossState.Idle;
    }


    protected override void Update()
    {
        //Movimiento Siempre
        transform.position = (transform.position + _velocity * Time.deltaTime);
        transform.forward = _velocity;

        _fsm.FakeUpdate();
    }

    public void GoToClick()
    {
        //sacar el nodo mas cercano
        //hacer fov y agarrar el nodo

        if (_team == BossTeam.naranja)
        {
            tempNodesFollow = _path.CalculateThetaStar(GetClosestNode(), _tempNodeNaranja);
            _fsm.ChangeState(BossState.GoingToClick);
        }
        else if (_team == BossTeam.celeste)
        {
            print("test celeste");
            tempNodesFollow = _path.CalculateThetaStar(GetClosestNode(), _tempNodeCeleste);
        }
    }

    LayerMask _nodosLayer = 11;

    Node GetClosestNode()
    {
        var nodosCercanos = Physics.OverlapSphere(transform.position, 250);

        Node nodoMasCercano = null;
        float distNodoMasCercano = 10000;

        foreach (var nodo in nodosCercanos)
        {
            if (Vector3.Distance(nodo.transform.position, transform.position) < distNodoMasCercano && nodo.GetComponent<Node>())
            {
                print(nodo.name);
                distNodoMasCercano = Vector3.Distance(nodo.transform.position, transform.position);
                nodoMasCercano = nodo.GetComponent<Node>();
            }
        }

        return nodoMasCercano;
    }

    void GoToTempNode()
    {
        AddForce(Arrive(tempNodesFollow[_indexTemp].transform.position));

        if (Vector3.Distance(transform.position, tempNodesFollow[_indexTemp].transform.position) < 0.1f)
        {
            _indexTemp++;

            if (_indexTemp >= tempNodesFollow.Count)
            {
                _fsm.ChangeState(BossState.Idle);
            }
        }
    }


    public Vector3 Seek(Vector3 desired)
    {
        //desired = desired.normalized;
        desired = (desired - transform.position).normalized;

        desired *= _maxSpeed;

        Vector3 steering = desired - _velocity;
        steering = Vector3.ClampMagnitude(steering, _maxSpeed * Time.deltaTime);

        return new Vector3(steering.x, 0, steering.z);
    }

    Vector3 Arrive(Vector3 target)
    {
        float dist = Vector3.Distance(transform.position, target);

        if (dist > _visionRadius)
        {
            return Seek(target);
        }

        Vector3 dir = target - transform.position;
        dir = dir.normalized;
        dir *= _maxSpeed * (dist / _visionRadius);

        Vector3 steering = dir - _velocity;
        steering = Vector3.ClampMagnitude(steering, _maxForce);

        return new Vector3(steering.x, 0, steering.z);
    }

    void AddForce(Vector3 dir)
    {
        _velocity = Vector3.ClampMagnitude(_velocity + dir, _maxSpeed);
    }

}

public enum BossTeam
{
    naranja, celeste
}

public enum BossState
{
    GoingToClick, Idle, Running, Attacking
}
