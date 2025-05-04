using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Agent

{
    [SerializeField] Transform[] _waypoints;
    [SerializeField] int _currentWaypoint;

    private void Start()
    {
        
    }

    protected override void Update()
    {
        AddForce(pursuit(_target));

        base.Update();
    }


}
