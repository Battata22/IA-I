using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HunterBehaivour : MonoBehaviour
{
    [SerializeField] protected Vector3 _vel;
    public Vector3 Vel
    {
        get
        {
            return _vel;
        }
    }

    FSM _fsm;
    [SerializeField] float _speed;

    [SerializeField] Transform[] _waypoints;
    [SerializeField] float _radiusBoidDetection;
    [SerializeField] LayerMask _layerBoid;

    [SerializeField] float _maxSpeed;
    [SerializeField] float _visionRadius;
    [SerializeField] float _maxForce;

    [SerializeField] float _maxEnergy;
    [SerializeField] float _energia;
    [SerializeField] float _energyDrain;

    [SerializeField] Slider _energySlider;

    [SerializeField] TextMeshProUGUI _textEstado;

    private void Awake()
    {
        GameManager.instance.Hunter = this;

        _fsm = new FSM();

        //add state rest
        _fsm.AddState(HunterStates.Rest, new RestState(_fsm, _maxEnergy, _energia, transform, _radiusBoidDetection, 
            _layerBoid, _energyDrain, _vel, _energySlider, AddForce, Seek, _textEstado));

        //add state patrol
        _fsm.AddState(HunterStates.Patrol, new PatrolState(_fsm, transform, _waypoints, _radiusBoidDetection,
             _layerBoid, AddForce, Seek, _vel, _maxSpeed, _textEstado));

        //add state hunting
        _fsm.AddState(HunterStates.Hunting, new HuntingState(_fsm, _energia, transform, 
            _radiusBoidDetection, _layerBoid, AddForce, Pursuit, Seek, _energyDrain, _energySlider, _maxEnergy, _vel, _textEstado));



        //default
        _fsm.ChangeState(HunterStates.Patrol);
    }

    void Start()
    {
        _energia = _maxEnergy;
        _energySlider.maxValue = _maxEnergy;
        _energySlider.value = _energia;
    }

    void Update()
    {
        _fsm.FakeUpdate();

        if (_vel != Vector3.zero)
        {
            transform.forward = _vel;
        }

        transform.position = GameManager.instance.GetPosition(transform.position + _vel * Time.deltaTime);
    }

    #region Movement_Y_Cosas
    public Vector3 Seek(Vector3 desired)
    {
        //desired = desired.normalized;

        desired = (desired - transform.position).normalized;

        desired *= _maxSpeed;

        Vector3 steering = desired - _vel;
        steering = Vector3.ClampMagnitude(steering, _maxForce * Time.deltaTime);

        return new Vector3(steering.x, 0, steering.z);
    }

    public Vector3 Evade(HunterBehaivour target)
    {
        var desired = target.transform.position + target.Vel;

        return Flee(-desired);
    }

    public Vector3 Flee(Vector3 target) => -Seek(target);

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

        Vector3 steering = dir - _vel;
        steering = Vector3.ClampMagnitude(steering, _maxForce);

        return steering;
    }

    void AddForce(Vector3 dir)
    {
        _vel = Vector3.ClampMagnitude(_vel + dir, _maxSpeed);
    }

    public Vector3 Pursuit(BoidBehaivour target)
    {
        var desired = target.transform.position + target.Velocity;

        return Seek(desired);
    }
    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _radiusBoidDetection);
    }

}

public enum HunterStates
{
    Rest, Hunting, Patrol
}
