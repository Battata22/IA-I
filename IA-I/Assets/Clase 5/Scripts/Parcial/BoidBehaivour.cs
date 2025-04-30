using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidBehaivour : MonoBehaviour
{

    [SerializeField] float _speed;
    [SerializeField] float _maxSpeed;
    [SerializeField] public Vector3 _velocity;
    public Vector3 Velocity { get { return _velocity; } }

    public PapaNodo _papaNode;

    public bool useFlocking = false, useEvade = false;

    private void Start()
    {
        GameManager.instance._myBoidsParcial.Add(this);

        AddForce(new Vector3(Random.Range(-1, 1), 0, Random.Range(-1, 1)) * _maxSpeed);
    }

    private void Update()
    {
        _papaNode.Execute(this);

        if (useFlocking == true)
        {
            Floking();
        }
        if (useEvade == true)
        {
            //Evade(/*el malo*/);
        }

        transform.position = GameManager.instance.GetPosition(transform.position + _velocity * Time.deltaTime);

        transform.forward = _velocity;
    }

    void Floking()
    {
        AddForce(Separation(GameManager.instance._myBoidsParcial, GameManager.instance._radioSeparation) * GameManager.instance._separationForce);
        AddForce(Allignment(GameManager.instance._myBoidsParcial, GameManager.instance._radioAllignment) * GameManager.instance._allignmentForce);
        AddForce(Cohesion(GameManager.instance._myBoidsParcial, GameManager.instance._radioAllignment) * GameManager.instance._cohesionForce);
    }

    Vector3 Separation(List<BoidBehaivour> myBoids, float radio)
    {
        Vector3 desired = Vector3.zero;

        foreach (BoidBehaivour boid in myBoids)
        {
            var dir = boid.transform.position - transform.position;

            if (boid == this || dir.magnitude > radio) continue;

            desired -= dir;
        }

        if (desired == Vector3.zero) return Vector3.zero;

        return Seek(desired);
    }

    Vector3 Cohesion(List<BoidBehaivour> myBoids, float radio)
    {
        Vector3 desired = transform.position;

        int count = 0;

        foreach (BoidBehaivour boid in myBoids)
        {
            if (boid == this || Vector3.Distance(transform.position, boid.transform.position) > radio) continue;

            count++;

            desired += boid.transform.position;
        }

        if (count == 0) return Vector3.zero;

        desired /= count;

        desired -= transform.position;

        return Seek(desired);
    }

    Vector3 Allignment(List<BoidBehaivour> myBoids, float radio)
    {
        Vector3 desired = Vector3.zero;

        int count = 0;

        foreach (BoidBehaivour boid in myBoids)
        {
            if (boid == this) continue;

            if (Vector3.Distance(transform.position, boid.transform.position) <= radio)
            {
                count++;

                desired += boid.Velocity;
            }
        }

        if (count == 0) return Vector3.zero;

        desired /= count;

        #region Otra Manera
        //desired = desired.normalized;

        //desired *= _maxSpeed;

        //Vector3 steering = desired - _velocity;

        //steering = Vector3.ClampMagnitude(steering, _maxSpeed);

        //return steering; 
        #endregion

        return Seek(desired);
    }

    public Vector3 Seek(Vector3 desired)
    {
        desired = desired.normalized;
        desired *= _maxSpeed;

        Vector3 steering = desired - _velocity;
        steering = Vector3.ClampMagnitude(steering, _maxSpeed);

        return new Vector3(steering.x, 0, steering.z);
    }

    public Vector3 Evade(Vector3 target) => -Seek(target);

    void AddForce(Vector3 dir)
    {
        _velocity = Vector3.ClampMagnitude(_velocity + dir, _maxSpeed);
    }

    private void OnDrawGizmos()
    {
        if (GameManager.instance != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, GameManager.instance._radioSeparation);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, GameManager.instance._radioAllignment);


        }

    }

}
