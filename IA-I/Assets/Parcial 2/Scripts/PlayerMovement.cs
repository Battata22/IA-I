using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float _speed;
    [SerializeField] int _hp;
    float xAxis, yAxis;

    Vector3 _direccion;

    void Start()
    {
    }


    void Update()
    {
        xAxis = Input.GetAxisRaw("Horizontal");
        yAxis = Input.GetAxisRaw("Vertical");

        Movement();
    }

    public void Movement()
    {
        Vector3 dir = (transform.right * xAxis + transform.forward * yAxis).normalized;

        _direccion.x = dir.x;
        _direccion.z = dir.z;

        //_cc.Move(_direccion * Time.fixedDeltaTime * _speed);
        transform.position += (_direccion * _speed * Time.deltaTime);
    }
}


