using UnityEngine;

public class Ghostly : FOV_Agent
{
    Pathfinding _path;
    [SerializeField] FOV_Target _player;

    [SerializeField] GhostState _state;

    [SerializeField] float _speed;
    [SerializeField] float _maxSpeed;
    [SerializeField] public Vector3 _velocity;
    public Vector3 Velocity { get { return _velocity; } }

    protected override void Start()
    {
        base.Start();
        _player = ManagerParcial2.Instance.Player;
        _path = new Pathfinding();
        _state = GhostState.Patrol;
    }


    protected override void Update()
    {
        //base.Update();
        //print(inFOV(_player.transform.position) ? "true" : "false");

        if (inFOV(_player.transform.position))
        {
            print(gameObject.name + " ve al Player");
        }

        if (inFOV(_player.transform.position))
        {
            _player.ChangeColor(Color.black);
        }
        else
        {
            _player.ChangeColor(Color.white);
        }


        if (_state == GhostState.Patrol)
        {
            //ir de nodo a nodo y chequear fov
        }
        else if (_state == GhostState.Following)
        {
            //seek al player mientras este en el fov
        }
        else if (_state == GhostState.GoingBack)
        {
            //ir al siguiente nodo del ultimo nodo que paso (si estaba en camino entre el nodo 1 al 2 entonces va al 2)
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
    Patrol, Following, GoingBack
}
