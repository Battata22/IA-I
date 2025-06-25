using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ghostly : FOV_Agent
{
    Pathfinding _path;
    [SerializeField] FOV_Target _player;

    public GhostState _state;

    [SerializeField] GhostType _type;

    [SerializeField] float _speed;
    [SerializeField] float _maxSpeed;
    [SerializeField] public Vector3 _velocity;
    [SerializeField] public Node[] _nodosBase;
    [SerializeField, Range(0, 1)] public float _visionRadius;
    [SerializeField, Range(0, 1)] float _maxForce;
    public Vector3 Velocity { get { return _velocity; } }

    public Node _ultimoNodo;

    public int _nextNode = 0;

    public List<Node> tempNodesFollow;
    public int _indexTemp;

    [SerializeField] bool canMove = true;

    FSMGhosty _fsm;

    private void Awake()
    {
        if (ManagerParcial2.Instance.FSM)
        {
            _fsm = new FSMGhosty();

            //add state Following
            _fsm.AddState(GhostState.Following, new GhostFollowingState(_fsm, transform, FollowPlayer/*, waitTimer*/, timer, _viewAngle, _viewRange, _obstacle, inFOV, _type));

            //add state Patrol
            _fsm.AddState(GhostState.Patrol, new GhostPatrolState(_fsm, transform, AddForce, Arrive, _nodosBase, /*_nextNode, _ultimoNodo*/ _type));

            //add state GoingBack
            _fsm.AddState(GhostState.GoingBack, new GhostGoingBackState(_fsm, transform, AddForce, Arrive, /*tempNodesFollow, _indexTemp, _nextNode*/ _type));

            //add state GoingLastSeen
            _fsm.AddState(GhostState.GoingLastSeen, new GhostGoingLastSeenState(_fsm, transform, AddForce, Arrive, /*tempNodesFollow, _indexTemp, _nextNode,*/ VolverABase, _type));



            //default
            _fsm.ChangeState(GhostState.Patrol);
        }
    }

    protected override void Start()
    {
        base.Start();
        _player = ManagerParcial2.Instance.Player;
        _path = new Pathfinding();        
        _state = GhostState.Patrol;
        //ManagerParcial2.Instance.PlayerEvent.outFOV += CallingAvengers;

        #region Manager Type
        if (_type == GhostType.White)
        {
            ManagerParcial2.Instance.WhiteGhost = this;
        }
        else if (_type == GhostType.Red)
        {
            ManagerParcial2.Instance.RedGhost = this;
        }
        else if (_type == GhostType.Blue)
        {
            ManagerParcial2.Instance.BlueGhost = this;
        }
        else if (_type == GhostType.Yellow)
        {
            ManagerParcial2.Instance.YellowGhost = this;
        }
        else
        {
            print("La cagaste en algun lado mamahuevaso");
        } 
        #endregion

    }

    public float waitTimer, timer;
    protected override void Update()
    {
        //Timer
        waitTimer += Time.deltaTime;

        //Movimiento Siempre
        transform.position = (transform.position + _velocity * Time.deltaTime);
        transform.forward = _velocity;

        //Deteccion constante del player
        if (inFOV(_player.transform.position) && _state != GhostState.Following)
        {
            if (ManagerParcial2.Instance.FSM)
            {
                _fsm.ChangeState(GhostState.Following);
            }
            else
            {
                _state = GhostState.Following;
            }
        }
        
        #region TestingFov
        if (inFOV(_player.transform.position))
        {
            _player.ChangeColor(Color.black);
            print(gameObject.name + " ve al Player");
            //ManagerParcial2.Instance.PlayerEvent.inFOV = true;
            //if (!ManagerParcial2.Instance.PlayerEvent.meVen.Contains(this))
            //{
            //    ManagerParcial2.Instance.PlayerEvent.meVen.Add(this);
            //}
        }
        else
        {
            _player.ChangeColor(Color.white);
            //ManagerParcial2.Instance.PlayerEvent.inFOV = false;
            //if (ManagerParcial2.Instance.PlayerEvent.meVen.Contains(this))
            //{
            //    ManagerParcial2.Instance.PlayerEvent.meVen.Remove(this);
            //}
        }
        #endregion

        if (ManagerParcial2.Instance.FSM)
        {
            //FSM
            _fsm.FakeUpdate();

        }
        else
        {
            if (_state == GhostState.Patrol)//------------------------------------------------------------------------------------------------------------
            {
                //ir de nodo a nodo y chequear fov

                //chequearFOV

                AddForce(Arrive(_nodosBase[_nextNode].transform.position));

                if (Vector3.Distance(transform.position, _nodosBase[_nextNode].transform.position) < 0.1f)
                {
                    _ultimoNodo = _nodosBase[_nextNode];
                    _nextNode++;
                    if (_nextNode > _nodosBase.Length - 1)
                    {
                        _nextNode = 0;
                    }
                }


                #region Comment
                //for (int i = 0; i < _nodosBase.Length; i++)
                //{
                //    if (Vector3.Distance(transform.position, _nodosBase[i].transform.position) <= 0.1f && i < _nodosBase.Length)
                //    {
                //        _nextNode = _nodosBase[i + 1];
                //        _ultimoNodo = _nodosBase[i];
                //        print(i);
                //    }
                //    else if (Vector3.Distance(transform.position, _nodosBase[i].transform.position) <= 0.2f && i >= _nodosBase.Length)
                //    {
                //        _nextNode = _nodosBase[0];
                //        _ultimoNodo = _nodosBase[i];
                //    }
                //}

                //print("haciendo seek a " + _nextNode.name);
                //AddForce(Seek(_nextNode.transform.position)); 
                #endregion

            }
            else if (_state == GhostState.Following)//------------------------------------------------------------------------------------------------------------
            {
                //seek al player mientras este en el fov
                FollowPlayer(_player.transform.position);

                //alertar a los demas de la pos del player
                if (waitTimer >= timer)
                {
                    ManagerParcial2.Instance.PlayerEvent.activarEventoFueraDeFOV = true;
                    waitTimer = 0;
                }

                if (!inFOV(_player.transform.position) && _state == GhostState.Following)
                {
                    ManagerParcial2.Instance.PlayerEvent.activarEventoFueraDeFOV = true;
                    _state = GhostState.GoingLastSeen;
                }
            }
            else if (_state == GhostState.GoingBack)//-------------------------------------------------------------------------------------------------------------
            {
                //ir al siguiente nodo del ultimo nodo que paso (si estaba en camino entre el nodo 1 al 2 entonces va al 2)

                AddForce(Arrive(tempNodesFollow[_indexTemp].transform.position));

                if (Vector3.Distance(transform.position, tempNodesFollow[_indexTemp].transform.position) < 0.1f)
                {
                    _indexTemp++;

                    if (_indexTemp >= tempNodesFollow.Count)
                    {
                        _state = GhostState.Patrol;
                        _nextNode = 1;
                    }
                }
            }
            else if (_state == GhostState.GoingLastSeen)//------------------------------------------------------------------------------------------------------------
            {
                //Hacer el camino de los nodos hasta el nodo mas cercano del punto ultima vez visto

                //print("tamaño de la lista " + tempNodesFollow.Count + ". index " + _indexTemp);

                AddForce(Arrive(tempNodesFollow[_indexTemp].transform.position));

                if (Vector3.Distance(transform.position, tempNodesFollow[_indexTemp].transform.position) < 0.1f)
                {
                    _indexTemp++;

                    if (_indexTemp > tempNodesFollow.Count - 1)
                    {
                        #region Comment
                        //canMove = false;
                        //print("llegue al nodo temp");
                        //hacer GoinBack 
                        #endregion
                        _indexTemp--;
                        _state = GhostState.GoingBack;
                        VolverABase();
                    }
                }

                #region Comment
                //if (canMove)
                //{
                //    AddForce(Arrive(tempNodesFollow[_indexTemp].transform.position));
                //} 
                #endregion
            }
            else
            {
                print("estoy en la mierda");
            }
        }


        
    }

    public void CallingAvengers()
    {
        if (_state != GhostState.Following)
        {
            //if (_state != GhostState.GoingBack)
            //{
            //    _indexTemp = 0;
            //}
            tempNodesFollow = _path.CalculateAStar(_nodosBase[_nextNode], ManagerParcial2.Instance.tempNode);
            print(tempNodesFollow.Count);

            if (ManagerParcial2.Instance.FSM)
            {
                _fsm.ChangeState(GhostState.GoingLastSeen);
            }
            else
            {
                _state = GhostState.GoingLastSeen;
            }

            //print("avengers");
        }
    }

    bool calculoPathVolver = false;
    public void VolverABase()
    {
        calculoPathVolver = false;
        if (!calculoPathVolver)
        {
            _indexTemp = 0;
            tempNodesFollow = _path.CalculateAStar(ManagerParcial2.Instance.tempNode, _nodosBase[0]);
            calculoPathVolver = true;
            print(tempNodesFollow.Count);
        }
    }

    public void FollowPlayer(Vector3 playerPos)
    {
        AddForce(Arrive(playerPos));
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Hijo")
        {
            SceneManager.LoadScene(0);
        } 
    }
}

public enum GhostState
{
    Patrol, Following, GoingBack, GoingLastSeen, testing
}

public enum GhostType
{
    White, Red, Blue, Yellow
}

