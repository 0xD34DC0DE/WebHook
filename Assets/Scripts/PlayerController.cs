using UnityEngine;

public class PlayerController : Singleton<PlayerController>
{
    private Transform _transform;
    private Camera _camera;
    private Rigidbody _rigidbody;

    private const float RotationSpeed = 150f;
    private const float Speed = 5f;
    private const float RunningSpeed = 5f;
    private const float JumpPower = 10f;

    private float _pitch;
    private float _yaw;
    private float _roll;

    private float _translationZ;
    private float _translationX;
    private float _runSpeed;

    void Start()
    {
        GetComponents();
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void GetComponents()
    {
        _camera = Camera.main;
        _transform = GetComponent<Transform>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Move();
        Turn();
        Jump();
    }

    private void Move()
    {
        _translationZ = Input.GetAxis("Vertical") * Time.deltaTime * Speed;
        _translationX = Input.GetAxis("Horizontal") * Time.deltaTime * Speed;
        _runSpeed = Input.GetAxis("Run") * Time.deltaTime * RunningSpeed;

        // TODO: MAKE THIS NOT UGLY
        if (IsRunning())
        {
            _transform.position += (_transform.forward * _translationZ) + (_transform.right * _translationX) +
                                   (_transform.forward * _runSpeed);
        }
        else
            _transform.position += (_transform.forward * _translationZ) + (_transform.right * _translationX);
    }

    // TODO: 
    private void Jump()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            _rigidbody.AddForce(_transform.up * JumpPower);
        }
    }

    private void Turn()
    {
        _yaw = Input.GetAxis("Mouse X") * Time.deltaTime * RotationSpeed;
        _transform.eulerAngles += new Vector3(0, _yaw, 0);
    }

    private bool IsRunning()
    {
        return Input.GetKey(KeyCode.LeftShift) && IsWalking();
    }

    private bool IsWalking()
    {
        return Input.GetKey(KeyCode.W);
    }

    private void Look()
    {
        _pitch = Input.GetAxis("Mouse Y") * Time.deltaTime * RotationSpeed;
        _roll = 0f;

        _pitch = Mathf.Clamp(_pitch, -90f, 90f);

        _camera.transform.localEulerAngles += new Vector3(-_pitch, 0, _roll);
    }

    private void LateUpdate()
    {
        Look();
    }
}