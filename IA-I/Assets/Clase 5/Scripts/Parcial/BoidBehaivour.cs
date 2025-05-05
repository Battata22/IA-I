using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidBehaivour : MonoBehaviour
{

    [SerializeField] float _speed;
    [SerializeField] float _maxSpeed;
    public float _visionRadius;
    [SerializeField, Range(0, 1)] protected float _maxForce;
    [SerializeField] public Vector3 _velocity;
    [SerializeField] LayerMask _maskComida, _maskBoids;
    public Vector3 Velocity { get { return _velocity; } }

    public PapaNodo _papaNode;

    public bool useFlocking = false, useEvade = false, useFood = false, useRandom = false;

    private void Start()
    {
        GameManager.instance._myBoidsParcial.Add(this);

        AddForce(new Vector3(Random.Range(-1, 1), 0, Random.Range(-1, 1)) * _maxSpeed);

        if (_papaNode == null)
        {
            _papaNode = GameManager.instance.qNodoStart;
        }
    }

    private void Update()
    {
        _papaNode.Execute(this);

        waitRandom += Time.deltaTime;

        if (useFlocking == true)
        {
            Floking();
            print("Flocking");
        }
        else if (useEvade == true)
        {
            AddForce(Evade(GameManager.instance.Hunter));
            print("Evade");
        }
        else if (useFood == true)
        {
            AddForce(Arrive(CalculateNearbyFood()));
            print("Food");
        }
        else if (useRandom == true)
        {
            //RandomMovement
            AddForce(RandomDir());
            print("random");
        }

        transform.position = GameManager.instance.GetPosition(transform.position + _velocity * Time.deltaTime);
        //profe 1 transform.position += _velocity * Time.deltaTime;

        if (_velocity != Vector3.zero)
        {
            transform.forward = _velocity;
        }

        // profe 2 transform.position = GameManager.instance.GetPosition(transform.position);

        transform.localRotation = new Quaternion(0, transform.localRotation.y, 0, 0);
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

        Vector3 steering = dir - _velocity;
        steering = Vector3.ClampMagnitude(steering, _maxForce);

        return steering;
    }

    //public Vector3 Evade(Agent target)
    //{
    //    var desired = target.transform.position + target.Vel;

    //    return Flee(desired);
    //}

    void AddForce(Vector3 dir)
    {
        _velocity = Vector3.ClampMagnitude(_velocity + dir, _maxSpeed);
    }

    public bool IsFoodNearby()
    {
        var _food = Physics.OverlapSphere(transform.position, _visionRadius, _maskComida);
        if (_food.Length > 0)
        {
            return true;
        }
        else return false;
    }

    [SerializeField] float _lastClosestFood = 10000;
    [SerializeField] Vector3 _closestFood;

    public Vector3 CalculateNearbyFood()
    {
        var _food = Physics.OverlapSphere(transform.position, _visionRadius, _maskComida);
        foreach (var f in _food)
        {
            if (_lastClosestFood > Vector3.Distance(f.transform.position, transform.position))
            {
                _lastClosestFood = Vector3.Distance(f.transform.position, transform.position);
                _closestFood = f.transform.position;
            }
        }
        _lastClosestFood = 10000;
        return _closestFood;
    }

    public bool IsBoidNearby()
    {
        var _boid = Physics.OverlapSphere(transform.position, _visionRadius, _maskBoids);
        if (_boid != null)
        {
            return true;
        }
        else return false;
    }

    [SerializeField] float _lastClosestBoid = 10000;
    [SerializeField] Vector3 _closestBoid;
    public Vector3 CalculateNearbyBoid()
    {
        var _boids = Physics.OverlapSphere(transform.position, _visionRadius, _maskBoids);
        foreach (var boid in _boids)
        {
            if (_lastClosestBoid > Vector3.Distance(boid.transform.position, transform.position))
            {
                _lastClosestBoid = Vector3.Distance(boid.transform.position, transform.position);
                _closestBoid = boid.transform.position;
            }
        }
        _lastClosestBoid = 10000;
        return _closestBoid;
    }

    float waitRandom = 1000;
    [SerializeField] float randomSearchTime;
    [SerializeField] Vector3 randomDir;

    public Vector3 RandomDir()
    {
        if (waitRandom >= randomSearchTime)
        {
            waitRandom = 0;

             float random1 = Random.Range(-GameManager.instance._width, GameManager.instance._width);
             float random2 = Random.Range(-GameManager.instance._height, GameManager.instance._height);

            randomDir = new Vector3(random1, 1, random2);
        }

        return randomDir;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 7)
        {
            Destroy(collision.gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        if (GameManager.instance != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, GameManager.instance._radioSeparation);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, GameManager.instance._radioAllignment);

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, _visionRadius);
        }

    }

}
