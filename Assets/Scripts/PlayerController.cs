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

    [SerializeField] private float _lerpSpeed = 20f;

    private const float RotationSpeed = 100f;
    private const float Speed = 700f;
    private const float RunningSpeed = 200f;
    private const float JumpPower = 4f;

    private const float MinVelMagForOppositeMovement = 0.01f ;
    private const float OppositeMovementMultiplier = 0.175f;
    private const float MaxSpeed = 300.0f;

    private float xInput = 0.0f;
    private float yInput = 0.0f;

    private float _pitch;
    private float _yaw;
    private float _roll;

    private float _translationZ;
    private float _translationX;
    private float _translationY;
    private float _runSpeed;

    private bool _noclip;
    
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
        //Jump();
        Movement();
    }
    
    public void Update()
    {
        CheckInput();
        
    }

    private void LateUpdate()
    {
        Look();
    }

    private void CheckInput()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            _rigidbody.isKinematic = !_rigidbody.isKinematic;
        }

        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");
    }

    private void Move()
    {
        _translationZ = Input.GetAxis("Vertical") * Time.deltaTime * Speed;
        _translationX = Input.GetAxis("Horizontal") * Time.deltaTime * Speed;
        _translationY = _rigidbody.velocity.y;
        _runSpeed = Input.GetAxis("Run") * Time.deltaTime * RunningSpeed;

        if (_rigidbody.isKinematic)
            Noclip();
        else
            Walk();
    }

    private void Noclip()
    {
        transform.position = Vector3.Lerp(transform.position, transform.position + ((_translationZ * _camera.transform.forward) + 
                (_translationX *_camera.transform.right)),  _lerpSpeed);
    }

    private void Walk()
    {
        if (IsRunning())
            _rigidbody.velocity = (_orientation.forward * _translationZ) + (_orientation.right * _translationX) +
                                  (_orientation.forward * _runSpeed) + (Vector3.up * _translationY);
        else
            _rigidbody.velocity = (_orientation.forward * _translationZ) + (_orientation.right * _translationX) +
                                  (Vector3.up * _translationY);
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

    private void Movement()
    {
        //Find actual velocity relative to where player is looking
        Vector2 mag = RelativeVelocityToCamera();
        float xMag = mag.x;
        float yMag = mag.y;


        //Counteract sliding and sloppy movement
        OppositeMovement(xInput, yInput, mag);
        
        if (xInput > 0 && xMag > MaxSpeed) xInput= 0;
        if (xInput < 0 && xMag < -MaxSpeed) xInput = 0;
        if (yInput > 0 && yMag > MaxSpeed) yInput = 0;
        if (yInput < 0 && yMag < -MaxSpeed) yInput = 0;
        
        //Apply forces to move player
        _rigidbody.AddForce(_orientation.transform.forward * yInput * Speed * Time.deltaTime);
        _rigidbody.AddForce(_orientation.transform.right * xInput * Speed * Time.deltaTime);
    }
    
    private Vector2 RelativeVelocityToCamera() {
        float lookAngle = _orientation.transform.eulerAngles.y;
        float moveAngle = Mathf.Atan2(_rigidbody.velocity.x, _rigidbody.velocity.z) * Mathf.Rad2Deg;

        float u = Mathf.DeltaAngle(lookAngle, moveAngle);
        float v = 90 - u;

        float magnitue = _rigidbody.velocity.magnitude;
        float yMag = magnitue * Mathf.Cos(u * Mathf.Deg2Rad);
        float xMag = magnitue * Mathf.Cos(v * Mathf.Deg2Rad);
        
        return new Vector2(xMag, yMag);
    }
    
    private void OppositeMovement(float inputX, float inputY, Vector2 mag) {
        
        //Counter movement
        if (Math.Abs(mag.x) > MinVelMagForOppositeMovement && Math.Abs(inputX) < 0.05f || (mag.x < -MinVelMagForOppositeMovement && inputX > 0) || (mag.x > MinVelMagForOppositeMovement && inputX < 0)) {
            _rigidbody.AddForce(Speed * _orientation.transform.right * Time.deltaTime * -mag.x * OppositeMovementMultiplier);
        }
        if (Math.Abs(mag.y) > MinVelMagForOppositeMovement && Math.Abs(inputY) < 0.05f || (mag.y < -MinVelMagForOppositeMovement && inputY > 0) || (mag.y > MinVelMagForOppositeMovement && inputY < 0)) {
            _rigidbody.AddForce(Speed * _orientation.transform.forward * Time.deltaTime * -mag.y * OppositeMovementMultiplier);
        }
        
        //Limit diagonal running. This will also cause a full stop if sliding fast and un-crouching, so not optimal.
        /*if (Mathf.Sqrt((Mathf.Pow(rb.velocity.x, 2) + Mathf.Pow(rb.velocity.z, 2))) > maxSpeed) {
            float fallingSpeed = rb.velocity.y;
            Vector3 n = rb.velocity.normalized * maxSpeed;
            rb.velocity = new Vector3(n.x, fallingSpeed, n.z);
        }*/
    }
}