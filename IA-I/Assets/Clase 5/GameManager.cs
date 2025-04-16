using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public float _width;
    public float _height;

    [SerializeField, Range(0f, 1f)] public float _separationForce;
    [SerializeField, Range(0f, 1f)] public float _cohesionForce;
    [SerializeField, Range(0f, 1f)] public float _allignmentForce;

    [SerializeField] public float _radioSeparation;
    [SerializeField] public float _radioAllignment;

    public List<Boid> _myBoids = new();



    public Vector3 GetPosition(Vector3 position)
    {
        if (position.x < -_width)
        {
            position.x = _width;
        }
        if (position.x > _width)
        {
            position.x = -_width;
        }
        if (position.z < -_height)
        {
            position.z = _height;
        }
        if (position.z > _height)
        {
            position.z = -_height;
        }
    
        return position;
    }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }



    private void OnDrawGizmos()
    {
        Vector3 leftDown = (Vector3.right * -_width) + (Vector3.forward * -_height);
        Vector3 leftUp = (Vector3.right * -_width) + (Vector3.forward * _height);
        Vector3 rightDown = (Vector3.right * _width) + (Vector3.forward * -_height);
        Vector3 rightUp = (Vector3.right * _width) + (Vector3.forward * _height);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(leftDown, leftUp);
        Gizmos.DrawLine(leftUp, rightUp);
        Gizmos.DrawLine(rightUp, rightDown);
        Gizmos.DrawLine(rightDown, leftDown);
    }
}
