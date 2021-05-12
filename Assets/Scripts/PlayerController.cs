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
    private const float Speed = 500f;
    private const float RunningSpeed = 1000f;
    private const float JumpPower = 200f;
    private const float NoClipSpeed = 100.0f;

    private const float MinVelMagForOppositeMovement = 0.01f ;
    private const float OppositeMovementMultiplier = 0.2f;
    private const float MaxSpeed = 300.0f;

    private float xInput = 0.0f;
    private float yInput = 0.0f;
    private bool isJumping = false;
    private bool isRunning = false;

    private bool hasJumped = false;
    private const float jumpCooldown = 0.25f;

    private float _pitch;
    private float _yaw;
    private float _roll;

    private float _translationZ;
    private float _translationX;
    private float _translationY;
    private float _runSpeed;

    private bool _noclip;

    // Need to keep track of position and normals because unity keeps updating collision objects
    private Collision _lastCollision;
    private Vector3 _lastCollisionPoint;
    private Vector3 _lastCollisionNormal;
    private Collision _currentCollision;
    private Vector3 _currentCollisionPoint;
    private Vector3 _currentCollisionNormal;

    private void Start()
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
        if(_rigidbody.isKinematic)
            Noclip();
        else
            Movement();
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

        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");
        
        isJumping = Input.GetButton("Jump");
        isRunning = Input.GetKey(KeyCode.LeftShift);
    }

    private void Noclip()
    {
        _translationZ = yInput * Time.deltaTime * NoClipSpeed;
        _translationX = xInput * Time.deltaTime * NoClipSpeed;

        var position = transform.position;
        var transform1 = _camera.transform;
        position = Vector3.Lerp(position, position + (_translationZ * transform1.forward + _translationX *transform1.right),  _lerpSpeed);
        transform.position = position;
    }

    private void Jump()
    {
        if (isJumping && !hasJumped && (IsOnGround() || CanWallJump()))
        {
            if (CanWallJump())
            {
                _lastCollision = _currentCollision;
                _lastCollisionNormal = _currentCollisionNormal;
                _lastCollisionPoint = _currentCollisionPoint;
            }
            
            _rigidbody.AddForce(Vector2.up * (JumpPower * 1.5f));
            hasJumped = true;
            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private void ResetJump()
    {
        hasJumped = false;
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
        _yaw = _playerCamera.transform.localRotation.eulerAngles.y + _yaw;
        
        _roll = 0f;
        
        _playerCamera.transform.localRotation = Quaternion.Euler(_pitch, _yaw, _roll);
        _orientation.transform.localRotation = Quaternion.Euler(0.0f, _yaw, 0.0f);

    }

    private void Movement()
    {
        if (_rigidbody.velocity.magnitude >= 500) return;
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

        float totalSpeed = Speed;
        totalSpeed += (isRunning) ? RunningSpeed : 0f;
        
        //Apply forces to move player
        _rigidbody.AddForce(_orientation.transform.forward * (yInput * totalSpeed * Time.deltaTime));
        _rigidbody.AddForce(_orientation.transform.right * (xInput * totalSpeed * Time.deltaTime));
    }
    
    private Vector2 RelativeVelocityToCamera() {
        float lookAngle = _orientation.transform.eulerAngles.y;
        var velocity = _rigidbody.velocity;
        float moveAngle = Mathf.Atan2(velocity.x, velocity.z) * Mathf.Rad2Deg;

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
            Debug.Log(other.gameObject.name);
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