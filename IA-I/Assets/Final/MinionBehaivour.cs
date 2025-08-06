using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionBehaivour : FOV_Agent, IDamageable, IBoidFinal
{
    public BossTeam _team;
    [SerializeField] JefesBehaviour _myBoss;
    [SerializeField] float _rangoDeCercaniaArrive;

    [SerializeField] Node _bossNode;
    [SerializeField] Node _nodoBase;

    [SerializeField] int _life;
    [SerializeField] int _maxLife = 100;

    Pathfinding _path;
    FSMMinions _fsm;
    public MinionState _state;

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

    //-----------------------Daño--------------------------------

    public Renderer _myRenderer;
    public Material _matBase, _matDano;
    public float _damageAnimationTime;

    //-----------------------Feedback--------------------------------

    public Material _matAttacking;
    public Material _matFloking;
    public Material _matLooking;
    public Material _matRunning;

    //-----------------------Balas--------------------------------

    [SerializeField] BulletBehaviour _bullet;
    [SerializeField] float _shootCooldown;
    [SerializeField] float _waitShoot;

    //-----------------------ObstacleAvoidance--------------------------------

    [SerializeField, Range(0f, 2f)] float _sphereCastRadius = 1;
    [SerializeField, Range(1f, 5f)] float _maxOAForce;
    public bool ObstacleActivo = true;

    private void Awake()
    {
        _life = _maxLife;

        _myRenderer = GetComponent<Renderer>();

        //-----------------------------------------------------------------------------------------

        _fsm = new FSMMinions();

        _fsm.AddState(MinionState.Idle, new MinionIdleState(this));

        _fsm.AddState(MinionState.Flocking, new MinionFlockingState(_fsm, this));

        _fsm.AddState(MinionState.Attacking, new MinionAttackingState(_fsm, this, _otherAgents));

        _fsm.AddState(MinionState.LookingForBoss, new MinionLookingForBossState(this));

        _fsm.AddState(MinionState.Running, new MinionRunningState(this));

        _fsm.AddState(MinionState.Lost, new MinionRunningState(this));


        //default
        _fsm.ChangeState(MinionState.Flocking);

        //-----------------------------------------------------------------------------------------

        if (_team == BossTeam.naranja)
        {
            FlockingManager.instance._myNaranjaTeammates.Add(this);
            MouseManager.RefreshSearchNaranja += GoToBoss;
        }
        else if (_team == BossTeam.celeste)
        {
            FlockingManager.instance._myCelesteTeammates.Add(this);
            MouseManager.RefreshSearchCeleste += GoToBoss;
        }
    }

    protected override void Start()
    {
        _path = new Pathfinding();
        _state = MinionState.Idle;
    }


    protected override void Update()
    {
        if (ObstacleActivo)
        {
            AddForce(ObstacleAvoidance());
        }
        else
        {
            Lost();
        }

            //Movimiento Siempre
            transform.position = (transform.position + _velocity * Time.deltaTime);
        transform.forward = _velocity;

        _fsm.FakeUpdate();

        if (_life > 40)
        {
            //Cambiar a FSM attack si entra alguien en FOV
            foreach (var agent in _otherAgents)
            {
                if (inFOV(agent.transform.position))
                {
                    //print(gameObject.name + " esta viendo a " + agent);
                    _fsm.ChangeState(MinionState.Attacking);
                }
            }
        }
        else if (_life <= 40 && _state != MinionState.Running)
        {
            _fsm.ChangeState(MinionState.Running);
        }
    }

    //-----------------------------------------------

    Vector3 ObstacleAvoidance()
    {
        Vector3 pos = transform.position;
        Vector3 dir = transform.forward;
        float dist = _velocity.magnitude;

        if (Physics.SphereCast(pos, _sphereCastRadius, dir, out RaycastHit hit, dist, _obstacle))
        {
            var obstacle = hit.transform;
            Vector3 dirToObj = obstacle.position - pos;
            float angle = Vector3.SignedAngle(dir, dirToObj, Vector3.up);
            Vector3 desired = angle > 0 ? -transform.right : transform.right;
            desired = desired.normalized;
            desired *= _maxSpeed;

            //Steering
            Vector3 steering = desired - _velocity;
            return Vector3.ClampMagnitude(steering, _maxOAForce);
        }
        return Vector3.zero;
    }

    //-----------------------------------------------

    public void Floking()
    {
        if (Vector3.Distance(_myBoss.transform.position, transform.position) > _rangoDeCercaniaArrive)
        {
            AddForce(Arrive(_myBoss.transform.position));
        }
        else
        {
            AddForce(-Seek(_myBoss.transform.position));
            //_velocity = Vector3.zero;
        }

        if (_team == BossTeam.naranja)
        {
            AddForce(Separation(FlockingManager.instance._myNaranjaTeammates, FlockingManager.instance._radioSeparation) * FlockingManager.instance._separationForce);
            //AddForce(SeparationRay(FlockingManager.instance._radioSeparation) * FlockingManager.instance._separationForce);
            //AddForce(SeparationBoss(FlockingManager.instance._radioSeparation) * FlockingManager.instance._separationForce);
            //AddForce(Cohesion(FlockingManager.instance._myNaranjaTeammates, FlockingManager.instance._radioAllignment) * FlockingManager.instance._cohesionForce);
        }
        else if (_team == BossTeam.celeste)
        {
            AddForce(Separation(FlockingManager.instance._myCelesteTeammates, FlockingManager.instance._radioSeparation) * FlockingManager.instance._separationForce);
            //AddForce(Cohesion(FlockingManager.instance._myCelesteTeammates, FlockingManager.instance._radioAllignment) * FlockingManager.instance._cohesionForce);
        }


    }

    //-----------------------------------------------

    Vector3 Separation(List<MinionBehaivour> myTeammates, float radio)
    {
        Vector3 desired = Vector3.zero;

        foreach (MinionBehaivour teammate in myTeammates)
        {
            var dir = teammate.transform.position - transform.position;

            if (teammate == this || dir.magnitude > radio) continue;

            desired -= dir;
        }

        if (desired == Vector3.zero) return Vector3.zero;

        return Seek(desired);
    }

    //-----------------------------------------------

    Vector3 SeparationRay(float radio)
    {
        Vector3 desired = Vector3.zero;

        var cosas = Physics.OverlapSphere(transform.position, 10000);

        foreach (var cosa in cosas)
        {
            if (_team == BossTeam.naranja)
            {
                if (cosa.GetComponent<MinionBehaivour>() != null)
                {
                    if (cosa.GetComponent<MinionBehaivour>()._team == BossTeam.naranja)
                    {
                        var dir = cosa.transform.position - transform.position;

                        if (cosa == this || dir.magnitude > radio) continue;

                        desired -= dir;
                    }
                }
                if (cosa.GetComponent<JefesBehaviour>() != null)
                {
                    if (cosa.GetComponent<JefesBehaviour>()._team == BossTeam.naranja)
                    {
                        var dir = cosa.transform.position - transform.position;

                        if (cosa == this || dir.magnitude > radio) continue;

                        desired -= dir;
                    }
                }
            }
            else if (_team == BossTeam.celeste)
            {
                if (cosa.GetComponent<MinionBehaivour>() != null)
                {
                    if (cosa.GetComponent<MinionBehaivour>()._team == BossTeam.celeste)
                    {
                        var dir = cosa.transform.position - transform.position;

                        if (cosa == this || dir.magnitude > radio) continue;

                        desired -= dir;
                    }
                }
                if (cosa.GetComponent<JefesBehaviour>() != null)
                {
                    if (cosa.GetComponent<JefesBehaviour>()._team == BossTeam.celeste)
                    {
                        var dir = cosa.transform.position - transform.position;

                        if (cosa == this || dir.magnitude > radio) continue;

                        desired -= dir;
                    }
                }
            }
        }

        if (desired == Vector3.zero) return Vector3.zero;

        return Seek(desired);
    }

    //-----------------------------------------------

    Vector3 SeparationBoss(float radio)
    {
        Vector3 desired = Vector3.zero;

        var dir = _myBoss.transform.position - transform.position;

        if (dir.magnitude > radio)
        {
            desired -= dir;
        }

        if (desired == Vector3.zero) return Vector3.zero;

        return Seek(desired);
    }

    //-----------------------------------------------

    Vector3 Cohesion(List<MinionBehaivour> myTeammates, float radio)
    {
        Vector3 desired = transform.position;

        int count = 0;

        foreach (MinionBehaivour teammate in myTeammates)
        {
            if (teammate == this || Vector3.Distance(transform.position, teammate.transform.position) > radio) continue;

            count++;

            desired += teammate.transform.position;
        }

        if (count == 0) return Vector3.zero;

        desired /= count;

        desired -= transform.position;

        return Seek(desired);
    }

    //-----------------------------------------------

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

    public void Lost()
    {
        if (!InLOS(transform.position, _myBoss.transform.position))
        {
            AddForce(Arrive(_bossNode.transform.position));
        }
        else
        {
            //_fsm.ChangeState(MinionState.Flocking);
            ObstacleActivo = true;
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

    public bool CheckForBossLOS()
    {
        return InLOS(transform.position, _myBoss.transform.position);
    }

    //-----------------------------------------------

    public void CalculateRunBase()
    {
        _indexTemp = 0;
        tempNodesFollow = _path.CalculateThetaStar(GetClosestNode(), _nodoBase);
    }

    //-----------------------------------------------

    public void RunBase()
    {
        AddForce(Arrive(tempNodesFollow[_indexTemp].transform.position));

        if (Vector3.Distance(transform.position, tempNodesFollow[_indexTemp].transform.position) < 0.1f)
        {
            _indexTemp++;

            if (_indexTemp >= tempNodesFollow.Count)
            {
                _life = _maxLife;
                _fsm.ChangeState(MinionState.Flocking);
            }
        }
    }

    //-----------------------------------------------

    public void GoToBoss()
    {
        _indexTemp = 0;
        _bossNode.EjecutarTempNode();
        tempNodesFollow = _path.CalculateThetaStar(GetClosestNode(), _bossNode);
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

    public void GoToBossNode()
    {
        //si esta en LOS del boss cambiar a flocking
        if (InLOS(transform.position, _myBoss.transform.position))
        {
            _fsm.ChangeState(MinionState.Flocking);
        }

        AddForce(Arrive(tempNodesFollow[_indexTemp].transform.position));

        if (Vector3.Distance(transform.position, tempNodesFollow[_indexTemp].transform.position) < 0.1f)
        {
            _indexTemp++;

            if (_indexTemp >= tempNodesFollow.Count - 1)
            {
                if (InLOS(transform.position, _myBoss.transform.position))
                {
                    _fsm.ChangeState(MinionState.Flocking);
                }
                else
                {
                    GoToBoss();
                }
            }
        }
    }

    //-----------------------------------------------

    public void ResetIndex()
    {
        _indexTemp = 0;
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
            StopCoroutine(DamageAnimation());
            //print("Murio " + name);
            gameObject.SetActive(false);
            transform.position = new Vector3(100, 100, 100);
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

    //-----------------------------------------------

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

    //-----------------------------------------------
    void AddForce(Vector3 dir)
    {
        _velocity = Vector3.ClampMagnitude(_velocity + dir, _maxSpeed);
    }

    //-----------------------------------------------

    #region Descartado
    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.layer == 19 && other != null && _state != MinionState.Lost)
    //    {
    //        _fsm.ChangeState(MinionState.Lost);
    //        print("test");

    //    }
    //}

    //private void OnTriggerStay(Collider other)
    //{
    //    if (other.gameObject.layer == 19 && other != null && _state != MinionState.Lost)
    //    {
    //        _fsm.ChangeState(MinionState.Lost);
    //        print("test");
    //    }
    //} 
    #endregion

    //-----------------------------------------------

    private void OnDestroy()
    {
        if (_team == BossTeam.naranja)
        {
            FlockingManager.instance._myNaranjaTeammates.Add(this);
            MouseManager.RefreshSearchNaranja -= GoToBoss;
        }
        else if (_team == BossTeam.celeste)
        {
            FlockingManager.instance._myCelesteTeammates.Add(this);
            MouseManager.RefreshSearchCeleste -= GoToBoss;
        }
    }
}

public enum MinionState
{
    Idle, Flocking, Attacking, Running, LookingForBoss, Lost
}
