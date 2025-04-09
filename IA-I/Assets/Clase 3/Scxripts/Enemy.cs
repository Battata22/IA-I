using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Agent

{
    [SerializeField] Transform[] _waypoints;
    [SerializeField] int _currentWaypoint;

    protected override void Update()
    {
        AddForce(pursuit(_target));


        //AddForce(Seek(_waypoints[_currentWaypoint].position));

        //if (Vector3.Distance(_waypoints[_currentWaypoint].position, transform.position) < 0.5f)
        //{
        //    _currentWaypoint++;
        //    if (_currentWaypoint >= _waypoints.Length)
        //    {
        //        _currentWaypoint = 0;
        //    }
        //}

        base.Update();
    }


}
