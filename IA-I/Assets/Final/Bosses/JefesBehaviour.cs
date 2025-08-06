using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class JefesBehaviour : FOV_Agent, IDamageable, IBoidFinal
{
    public BossTeam _team;
    [SerializeField] Node _tempNodeNaranja;
    [SerializeField] Node _tempNodeCeleste;

    [SerializeField] int _life;
    [SerializeField] int _maxLife = 100;

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

    [SerializeField] BulletBehaviour _bullet;
    [SerializeField] float _shootCooldown;

    [SerializeField] TextMeshProUGUI _textGameOver;

    //-----------------------Daño--------------------------------

    public Renderer _myRenderer;
    public Material _matBase, _matDano;
    public float _damageAnimationTime;

    //-----------------------Feedback--------------------------------

    public Material _matAttacking;
    public Material _matGoing;

    private void Awake()
    {
        _life = _maxLife;

        _myRenderer = GetComponent<Renderer>();

        //-----------------------------------------------------------------------------------------

        _fsm = new FSMBosses();

        //add state Idle
        _fsm.AddState(BossState.Idle, new BossIdleState(this));

        //add state GoToClick
        _fsm.AddState(BossState.GoingToClick, new BossGoingToClickState(this, _indexTemp, GoToTempNode));

        //add state Attacking
        _fsm.AddState(BossState.Attacking, new BossAttackingState(_fsm, this, _otherAgents));

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

        foreach (var agent in _otherAgents)
        {
            if (inFOV(agent.transform.position))
            {
                //print(gameObject.name + " esta viendo a " + agent);
                _fsm.ChangeState(BossState.Attacking);
            }
        }
    }

    //-----------------------------------------------

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
            tempNodesFollow = _path.CalculateThetaStar(GetClosestNode(), _tempNodeCeleste);
            _fsm.ChangeState(BossState.GoingToClick);
        }
    }

    //-----------------------------------------------

    Node GetClosestNode()
    {
        var nodosCercanos = Physics.OverlapSphere(transform.position, 250);

        Node nodoMasCercano = null;
        float distNodoMasCercano = 10000;

        foreach (var nodo in nodosCercanos)
        {
            if (Vector3.Distance(nodo.transform.position, transform.position) < distNodoMasCercano && nodo.GetComponent<Node>() && nodo.GetComponent<Node>().tempNode == false)
            {
                distNodoMasCercano = Vector3.Distance(nodo.transform.position, transform.position);
                nodoMasCercano = nodo.GetComponent<Node>();
            }
        }

        return nodoMasCercano;
    }

    //-----------------------------------------------

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

    //-----------------------------------------------

    public void ResetIndex()
    {
        _indexTemp = 0;
    }

    //-----------------------------------------------

    [SerializeField] float _waitShoot;
    public void Shoot(GameObject target)
    {
        _waitShoot += Time.deltaTime;

        if (target != null && _waitShoot >= _shootCooldown)
        {
            var bala = Instantiate(_bullet, transform.position, Quaternion.LookRotation(target.transform.position - transform.position));

            if (_team == BossTeam.naranja)
            {
                bala.team = BalaTeam.Naranja;
            }
            else if (_team == BossTeam.celeste)
            {
                bala.team = BalaTeam.Celeste;
            }

            _waitShoot = 0;
        }
    }

    //-----------------------------------------------

    public bool CheckForEnemiesNearby()
    {
        bool alguien = false;

        foreach (var agent in _otherAgents)
        {
            if (inFOV(agent.transform.position))
            {
                alguien = true;
            }
        }

        return alguien;
    }

    //-----------------------------------------------
    
    public Transform GetTransform()
    {
        return transform;
    }

    //-----------------------------------------------

    public GameObject GetGameObject()
    {
        return gameObject;
    }

    //-----------------------------------------------

    public void TakeDamage(int damage)
    {
        StartCoroutine(DamageAnimation());

        _life -= damage;

        if (_life <= 0)
        {
            //Game over para el team x
            print(gameObject.name + " murio");
            Time.timeScale = 0;

            if (_team == BossTeam.naranja)
            {
                StopCoroutine(DamageAnimation());
                _textGameOver.text = "Gana el equipo Celeste";
            }
            else if (_team == BossTeam.celeste)
            {
                StopCoroutine(DamageAnimation());
                _textGameOver.text = "Gana el equipo Naranja";
            }
            _textGameOver.enabled = true;
        }
    }

    //-----------------------------------------------

    IEnumerator DamageAnimation()
    {
        _myRenderer.material = _matDano;
        yield return new WaitForSeconds(_damageAnimationTime);
        _myRenderer.material = _matBase;
    }

    //-----------------------------------------------

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

public interface IDamageable
{
    public void TakeDamage(int damage);
}
