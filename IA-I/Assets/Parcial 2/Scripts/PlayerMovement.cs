using UnityEngine;

    [RequireComponent(typeof(CharacterController))]

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float _speed;
    [SerializeField] int _hp;
    [SerializeField] CharacterController _cc;
    float xAxis, yAxis;

    Vector3 _direccion;

    void Start()
    {
        _cc = GetComponent<CharacterController>();
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

        _cc.Move(_direccion * Time.fixedDeltaTime * _speed);
    }
}


