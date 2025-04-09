using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Agent : MonoBehaviour
{

    [SerializeField] protected Vector3 _vel;

    public Vector3 Vel
    {
        get
        {
            return _vel;
        }
    }

    #region Floats
    [Space]
    [Header("<color=red>Stats</color>")]
    [SerializeField] protected float _maxSpd;
    [SerializeField,Range(0,1)] protected float _maxForce; 
    #endregion

    

    [SerializeField] protected Agent _target;
    //[SerializeField] protected SteringElection _myElection;


    protected virtual void Start()
    {
        
    }


    protected virtual void Update()
    {

        //switch (_myElection)
        //{
        //    case SteringElection.flee:
        //        AddForce(Flee(_target.transform.position));
        //        break;
        //    case SteringElection.evade:
        //        AddForce(Evade(_target));
        //        break;
        //    case SteringElection.persuit:
        //        AddForce(pursuit(_target));
        //        break;
        //    case SteringElection.seek:
        //        AddForce(Seek(_target.transform.position));
        //        break;
        //    default:
        //        break;
        //}
        //AddForce(Flee(_target.transform.position));

        transform.position += _vel * Time.deltaTime;
        transform.forward = _vel;
    }

    public Vector3 Seek(Vector3 target)
    {
        //vel deseada, vel actual, steering
        Vector3 dir = target - transform.position;
        dir = dir.normalized;
        dir *=_maxSpd;

        Vector3 steering = dir - _vel;
        steering = Vector3.ClampMagnitude(steering, _maxForce);

        return steering;

    }

    public Vector3 Flee(Vector3 target) => -Seek(target);


    public void AddForce(Vector3 dir)
    {
        _vel = Vector3.ClampMagnitude(_vel + dir, _maxSpd);
    }

    public Vector3 pursuit(Agent target)
    {
        var desired = target.transform.position + target.Vel;

        return Seek(desired);
    }

    public Vector3 Evade(Agent target)
    {
        var desired = target.transform.position + target.Vel;

        return Flee(desired);
    }

}
