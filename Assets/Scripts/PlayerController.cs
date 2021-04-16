using UnityEngine;

public class PlayerController : Singleton<PlayerController>
{
    private Transform _transform;
    private Camera _camera;
    private Rigidbody _rigidbody;

    private const float RotationSpeed = 150f;
    private const float Speed = 5f;
    private const float RunningSpeed = 5f;
    private const float JumpPower = 4f;

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
        Turn();
    }

    private void FixedUpdate()
    {
        Move();
        Jump();
    }

    private void Move()
    {
        _translationZ = Input.GetAxis("Vertical") * Time.deltaTime * Speed;
        _translationX = Input.GetAxis("Horizontal") * Time.deltaTime * Speed;
        _runSpeed = Input.GetAxis("Run") * Time.deltaTime * RunningSpeed;

        // TODO: Make this not ugly / smoother
        if (IsRunning())
        {
            _transform.position += (_transform.forward * _translationZ) + (_transform.right * _translationX) +
                                   (_transform.forward * _runSpeed);
        }
        else
            _transform.position += (_transform.forward * _translationZ) + (_transform.right * _translationX);
    }

    private void Jump()
    {
        if (Input.GetKey(KeyCode.Space) && IsOnGround())
        {
            _rigidbody.velocity = Vector3.up * JumpPower;
        }
    }

    private bool IsOnGround()
    {
        return Physics.Raycast(_transform.position, Vector3.down, 1.5f);
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

    // TODO: Stop camera from going glitching
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