using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    public BalaTeam team = BalaTeam.Error;
    [SerializeField] float _speed;
    [SerializeField] int _damage;

    private void Start()
    {
        Destroy(gameObject, 5);
    }

    void Update()
    {
        transform.position += transform.forward * _speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<IDamageable>() != null)
        {

            GameObject hit = other.gameObject;

            if (hit.GetComponent<JefesBehaviour>() != null)
            {
                if (team == BalaTeam.Naranja && hit.GetComponent<JefesBehaviour>()._team == BossTeam.celeste)
                {
                    hit.GetComponent<IDamageable>().TakeDamage(_damage);
                    Destroy(gameObject);
                }
                else if (team == BalaTeam.Celeste && hit.GetComponent<JefesBehaviour>()._team == BossTeam.naranja)
                {
                    hit.GetComponent<IDamageable>().TakeDamage(_damage);
                    Destroy(gameObject);
                }
            }
            else if (hit.GetComponent<MinionBehaivour>() != null)
            {
                if (team == BalaTeam.Naranja && hit.GetComponent<MinionBehaivour>()._team == BossTeam.celeste)
                {
                    hit.GetComponent<IDamageable>().TakeDamage(_damage);
                    Destroy(gameObject);
                }
                else if (team == BalaTeam.Celeste && hit.GetComponent<MinionBehaivour>()._team == BossTeam.naranja)
                {
                    hit.GetComponent<IDamageable>().TakeDamage(_damage);
                    Destroy(gameObject);
                }
            }
            else
            {
                print("Choque contra algo random");
            }

        }
        else if (other.gameObject.GetComponent<Node>() != null || other.gameObject.GetComponent<BulletBehaviour>() != null)
        {
            
        }
        else
        {
            Destroy(gameObject);
        }
    }
}

public enum BalaTeam
{
    Error, Naranja, Celeste
}
