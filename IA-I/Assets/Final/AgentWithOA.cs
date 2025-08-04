using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentWithOA : MonoBehaviour
{
    [SerializeField] Transform _target;
    [SerializeField] LayerMask _obstacle;
    Vector3 _velocity;
    [SerializeField, Range(0f, 10f)] float _maxVelocity;
    [SerializeField, Range(0f, 2f)] float _sphereCastRadius = 1;
    [SerializeField, Range(0.01f, 0.1f)] float _maxForce;
    [SerializeField, Range(1f, 5f)] float _maxOAForce;



    void Update()
    {
        AddForce(Seek(_target.position));
        AddForce(ObstacleAvoidance());

        transform.position += _velocity * Time.deltaTime;
        transform.forward = _velocity;
    }

    Vector3 ObstacleAvoidance()
    {
        Vector3 pos = transform.position;
        Vector3 dir = transform.forward;
        //Vector3 dir = _velocity;
        float dist = _velocity.magnitude;

        if(Physics.SphereCast(pos, _sphereCastRadius,dir,out RaycastHit hit,dist, _obstacle))
        {
            var obstacle = hit.transform;
            Vector3 dirToObj = obstacle.position - pos;
            float angle = Vector3.SignedAngle(dir, dirToObj, Vector3.up);
            Vector3 desired = angle > 0 ? -transform.right : transform.right;
            desired = desired.normalized;
            desired *= _maxVelocity;

            //Steering
            Vector3 steering = desired - _velocity;
            return Vector3.ClampMagnitude(steering, _maxOAForce);
        }
        return Vector3.zero;
    }

    protected Vector3 Seek(Vector3 target)
    {
        //Desired Velocity
        Vector3 desired = target - transform.position;
        desired = desired.normalized;
        desired *= _maxVelocity;

        //Steering
        Vector3 steering = desired - _velocity;
        steering = Vector3.ClampMagnitude(steering, _maxForce);
        return steering;
    }

    protected void AddForce(Vector3 dir)         //Current Velocity
    {
        _velocity = Vector3.ClampMagnitude(_velocity + dir, _maxVelocity);
    }
}
