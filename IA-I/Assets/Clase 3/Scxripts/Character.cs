using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : Agent
{
    public Transform _objTarget;
    public float _visionRadius;

    protected override void Update()
    {
        AddForce(Evade(_target));


        AddForce(Arrive(_objTarget.position));

        //transform.position += _vel * Time.deltaTime;
        //transform.forward = _vel;

        base.Update();
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
        dir *= _maxSpd * (dist / _visionRadius);

        Vector3 steering = dir - _vel;
        steering = Vector3.ClampMagnitude(steering, _maxForce);

        return steering;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        if (_objTarget != null )
        {
            Gizmos.DrawWireSphere(_objTarget.position, _visionRadius);
        }

    }
}
