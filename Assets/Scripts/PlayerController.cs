using System;
using UnityEngine;

public class PlayerController : Singleton<PlayerController>
{
    private Camera _camera;
    private Rigidbody _rigidbody;

    [SerializeField]
    private Transform _playerCamera;
    [SerializeField]
    private Transform _orientation;

    private const float RotationSpeed = 100f;
    private const float Speed = 200f;
    private const float RunningSpeed = 200f;
    private const float JumpPower = 4f;

    private float _pitch;
    private float _yaw;
    private float _roll;

    private float _translationZ;
    private float _translationX;
    private float _translationY;
    private float _runSpeed;

    void Start()
    {
        GetComponents();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void GetComponents()
    {
        _camera = Camera.main;
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Jump();
        Move();
    }
    
    public void Update()
    {
        Look();
    }

    private void Move()
    {
        _translationZ = Input.GetAxis("Vertical") * Time.deltaTime * Speed;
        _translationX = Input.GetAxis("Horizontal") * Time.deltaTime * Speed;
        _translationY = _rigidbody.velocity.y;
        _runSpeed = Input.GetAxis("Run") * Time.deltaTime * RunningSpeed;

        // TODO: Make this not ugly / smoother
        if (IsRunning())
            _rigidbody.velocity = (_orientation.forward * _translationZ) + (_orientation.right * _translationX) +
                                  (_orientation.forward * _runSpeed);
        else
            _rigidbody.velocity = (_orientation.forward * _translationZ) + (_orientation.right * _translationX) + Vector3.up * _translationY;
        
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
        return Physics.Raycast(transform.position, Vector3.down, 1.5f);
    }

    private bool IsRunning()
    {
        return Input.GetKey(KeyCode.LeftShift) && IsWalking();
    }

    private bool IsWalking()
    {
        return Input.GetKey(KeyCode.W);
    }

    // TODO: Stop camera from going upside down
    private void Look()
    {
        _pitch -= Input.GetAxis("Mouse Y") * Time.deltaTime * RotationSpeed;
        _pitch = Mathf.Clamp(_pitch, -90f, 90f);
        
        _yaw = Input.GetAxis("Mouse X") * Time.deltaTime * RotationSpeed;
        _yaw = _playerCamera.transform.localRotation.eulerAngles.y + _yaw;
        
        _roll = 0f;
        
        _playerCamera.transform.localRotation = Quaternion.Euler(_pitch, _yaw, _roll);
        _orientation.transform.localRotation = Quaternion.Euler(0.0f, _yaw, 0.0f);
    }
}