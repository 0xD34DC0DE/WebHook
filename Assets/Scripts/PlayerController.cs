using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : Singleton<PlayerController>
{
    private Camera _camera;
    private Rigidbody _rigidbody;
    private CapsuleCollider _capsuleCollider;

    [SerializeField] private Transform playerCamera;
    [SerializeField] private Transform orientation;
    [SerializeField] private float lerpSpeed = 20f;

    private const float RotationSpeed = 100f;
    private const float Speed = 500f;
    private const float RunningSpeed = 1000f;
    private const float JumpPower = 200f;
    private const float NoClipSpeed = 100.0f;

    private const float MinVelMagForOppositeMovement = 0.01f ;
    private const float OppositeMovementMultiplier = 0.2f;
    private const float MaxSpeed = 300.0f;

    private float _xInput = 0.0f;
    private float _yInput = 0.0f;
    private bool _isJumping = false;
    private bool _isRunning = false;
    private bool _isCrouching = false;
    
    private bool _hasJumped = false;
    private const float JumpCooldown = 0.25f;

    private float _pitch;
    private float _yaw;
    private float _roll;

    private float _translationZ;
    private float _translationX;
    private float _translationY;
    private float _runSpeed;

    private bool _noclip;
    
    private Collision _lastCollision;
    private Collision _currentCollision;
    private Vector3 _lastCollisionPoint;
    private Vector3 _lastCollisionNormal;
    private Vector3 _currentCollisionPoint;
    private Vector3 _currentCollisionNormal;
    
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
        _capsuleCollider = GetComponent<CapsuleCollider>();
    }

    private void FixedUpdate()
    {
        Jump();
        Crouch();
        if(_rigidbody.isKinematic)
            Noclip();
        else
            Movement();
    }

    private void Crouch()
    {
        _capsuleCollider.height = _isCrouching ? 1.0f : 2.0f;
    }

    public void Update()
    {
        CheckInput();
        if(_lastCollision != null)
            Debug.DrawRay(_lastCollisionPoint, _lastCollisionNormal, Color.magenta);
        if(_currentCollision != null)
            Debug.DrawRay(_currentCollisionPoint, _currentCollisionNormal, Color.yellow);
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
        
        _isCrouching = Input.GetKey(KeyCode.LeftControl) && !_isCrouching ? true : false;
        
        if (Input.GetKey(KeyCode.LeftControl))
        {
            _isCrouching = true;
        }
        else
        {
            _isCrouching = false;
        }

        _xInput = Input.GetAxisRaw("Horizontal");
        _yInput = Input.GetAxisRaw("Vertical");
        
        _isJumping = Input.GetButton("Jump");
        _isRunning = Input.GetKey(KeyCode.LeftShift);
    }

    private void Noclip()
    {
        _translationZ = _yInput * Time.deltaTime * NoClipSpeed;
        _translationX = _xInput * Time.deltaTime * NoClipSpeed;
        
        transform.position = Vector3.Lerp(transform.position, transform.position + ((_translationZ * _camera.transform.forward) + 
                (_translationX *_camera.transform.right)),  lerpSpeed);
    }

    private void Jump()
    {
        if (_isJumping && !_hasJumped && (IsOnGround() || CanWallJump()))
        {
            if (CanWallJump())
            {
                _lastCollision = _currentCollision;
                _lastCollisionNormal = _currentCollisionNormal;
                _lastCollisionPoint = _currentCollisionPoint;
            }
            
            _rigidbody.AddForce(Vector2.up * (JumpPower * 1.5f));
            _hasJumped = true;
            Invoke(nameof(ResetJump), JumpCooldown);
        }
    }

    private void ResetJump()
    {
        _hasJumped = false;
    }

    private bool IsOnGround()
    {
        return Physics.Raycast(transform.position, Vector3.down, 1.5f);
    }

    // TODO: Stop camera from going upside down
    private void Look()
    {
        _pitch -= Input.GetAxis("Mouse Y") * Time.deltaTime * RotationSpeed;
        _pitch = Mathf.Clamp(_pitch, -90f, 90f);
        
        _yaw = Input.GetAxis("Mouse X") * Time.deltaTime * RotationSpeed;
        _yaw = playerCamera.transform.localRotation.eulerAngles.y + _yaw;
        
        _roll = 0f;
        
        playerCamera.transform.localRotation = Quaternion.Euler(_pitch, _yaw, _roll);
        orientation.transform.localRotation = Quaternion.Euler(0.0f, _yaw, 0.0f);

    }

    private void Movement()
    {
        if (_rigidbody.velocity.magnitude >= 500) return;
        //Find actual velocity relative to where player is looking
        Vector2 mag = RelativeVelocityToCamera();
        float xMag = mag.x;
        float yMag = mag.y;


        //Counteract sliding and sloppy movement
        OppositeMovement(_xInput, _yInput, mag);
        
        if (_xInput > 0 && xMag > MaxSpeed) _xInput= 0;
        if (_xInput < 0 && xMag < -MaxSpeed) _xInput = 0;
        if (_yInput > 0 && yMag > MaxSpeed) _yInput = 0;
        if (_yInput < 0 && yMag < -MaxSpeed) _yInput = 0;

        float totalSpeed = Speed;
        totalSpeed += (_isRunning) ? RunningSpeed : 0f;
        
        //Apply forces to move player
        _rigidbody.AddForce(orientation.transform.forward * _yInput * totalSpeed * Time.deltaTime);
        _rigidbody.AddForce(orientation.transform.right * _xInput * totalSpeed * Time.deltaTime);
    }
    
    private Vector2 RelativeVelocityToCamera() {
        float lookAngle = orientation.transform.eulerAngles.y;
        float moveAngle = Mathf.Atan2(_rigidbody.velocity.x, _rigidbody.velocity.z) * Mathf.Rad2Deg;

        float u = Mathf.DeltaAngle(lookAngle, moveAngle);
        float v = 90 - u;

        float magnitude = _rigidbody.velocity.magnitude;
        float yMag = magnitude * Mathf.Cos(u * Mathf.Deg2Rad);
        float xMag = magnitude * Mathf.Cos(v * Mathf.Deg2Rad);
        
        return new Vector2(xMag, yMag);
    }
    
    private void OppositeMovement(float inputX, float inputY, Vector2 mag) {
        
        //Counter movement
        if (Math.Abs(mag.x) > MinVelMagForOppositeMovement && Math.Abs(inputX) < 0.05f || (mag.x < -MinVelMagForOppositeMovement && inputX > 0) || (mag.x > MinVelMagForOppositeMovement && inputX < 0)) {
            _rigidbody.AddForce(Speed * orientation.transform.right * Time.deltaTime * -mag.x * OppositeMovementMultiplier);
        }
        if (Math.Abs(mag.y) > MinVelMagForOppositeMovement && Math.Abs(inputY) < 0.05f || (mag.y < -MinVelMagForOppositeMovement && inputY > 0) || (mag.y > MinVelMagForOppositeMovement && inputY < 0)) {
            _rigidbody.AddForce(Speed * orientation.transform.forward * Time.deltaTime * -mag.y * OppositeMovementMultiplier);
        }
        
        //Limit diagonal running. This will also cause a full stop if sliding fast and un-crouching, so not optimal.
        /*if (Mathf.Sqrt((Mathf.Pow(rb.velocity.x, 2) + Mathf.Pow(rb.velocity.z, 2))) > maxSpeed) {
            float fallingSpeed = rb.velocity.y;
            Vector3 n = rb.velocity.normalized * maxSpeed;
            rb.velocity = new Vector3(n.x, fallingSpeed, n.z);
        }*/
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer.Equals(LayerMask.NameToLayer("World")))
        {
            _currentCollision = other;
            // This is dumb...
            _currentCollisionPoint = other.GetContact(0).point;
            _currentCollisionNormal = other.GetContact(0).normal;
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.layer.Equals(LayerMask.NameToLayer("World")))
        {
            _lastCollision = _currentCollision;
            // Dumb stuff again
            _lastCollisionPoint = _currentCollisionPoint;
            _lastCollisionNormal = _currentCollisionNormal;
            _currentCollision = null;
        }
    }

    private bool CanWallJump()
    {
        switch (_lastCollision)
        {
            case null when _currentCollision == null:
                return false;
            case null when _currentCollision != null:
                return true;
        }
        
        return _lastCollisionNormal != _currentCollisionNormal;
    }
}