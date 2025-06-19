using UnityEngine;

public class Ghostly : FOV_Agent
{
    Pathfinding _path;
    [SerializeField] FOV_Target _player;

    [SerializeField] GhostState _state;

    [SerializeField] float _speed;
    [SerializeField] float _maxSpeed;
    [SerializeField] public Vector3 _velocity;
    [SerializeField] public Node[] _nodosBase;
    public Vector3 Velocity { get { return _velocity; } }

    [SerializeField] Node _ultimoNodo;

    [SerializeField] int _nextNode = 0;

    protected override void Start()
    {
        base.Start();
        _player = ManagerParcial2.Instance.Player;
        _path = new Pathfinding();
        _state = GhostState.Patrol;
    }


    protected override void Update()
    {

        #region Testing
        if (inFOV(_player.transform.position))
        {
            _player.ChangeColor(Color.black);
            print(gameObject.name + " ve al Player");
        }
        else
        {
            _player.ChangeColor(Color.white);
        }
        #endregion

        transform.position = (transform.position + _velocity * Time.deltaTime);


        if (_state == GhostState.Patrol)
        {
            //ir de nodo a nodo y chequear fov

            AddForce(Seek(_nodosBase[_nextNode].transform.position));

            if (Vector3.Distance(transform.position, _nodosBase[_nextNode].transform.position) < 0.1f)
            {
                _nextNode++;
                if (_nextNode > _nodosBase.Length - 1)
                {
                    _nextNode = 0;
                }
            }


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

        }
        else if (_state == GhostState.Following)
        {
            //seek al player mientras este en el fov
        }
        else if (_state == GhostState.GoingBack)
        {
            //ir al siguiente nodo del ultimo nodo que paso (si estaba en camino entre el nodo 1 al 2 entonces va al 2)
        }
        else if (_state == GhostState.GoingLastSeen)
        {
            //Hacer el camino de los nodos hasta el nodo mas cercano del punto ultima vez visto
        }
        else
        {
            print("estoy en la mierda");
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

    void AddForce(Vector3 dir)
    {
        _velocity = Vector3.ClampMagnitude(_velocity + dir, _maxSpeed);
    }
}

public enum GhostState
{
    Patrol, Following, GoingBack, GoingLastSeen
}
