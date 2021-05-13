using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebHook : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody playerRb;
    [SerializeField] private Transform hookStartVisualTransform;
    [SerializeField] private Transform playerCamera;
    [SerializeField] private Transform playerRigidBodyGameObject;
    [SerializeField] private GameObject targetingPrefab;
    [SerializeField] private GameObject hookLineObject;

    [Header("Gameplay settings")]
    [SerializeField] private float maxDistance = 500.0f;
    [SerializeField] private float minDistance = 10.0f;
    [SerializeField] private float initialWinding = 20.0f;
    
    [Header("Hook joint settings")]
    [SerializeField] private float spring = 4.5f;
    [SerializeField] private float damper = 7.0f;
    [SerializeField] private float massScale = 4.5f;

    private GameObject _targetingPrefabInstance;
    private GameObject _hookLineObjectInstance;
    
    private RaycastHit _raycastHit;
    private SpringJoint _hookJoint;
    private Vector3 _hookPoint;
    private float _targetHookDistance;
    
    // States
    private bool _canLaunch = false;
    private bool _isAiming = false;
    private bool _isFiring = false;
    private bool _isHooked = false;
    private bool _isWinding = false;

    // Quick hack
    private bool _targetingPlayer = false;

    private void Update()
    {
        UpdateStates();

        if (_isAiming)
        {
            RayCast();
            if(!_targetingPlayer)
                DrawAimTarget();
        }

        if (!_isAiming && _targetingPrefabInstance != null)
        {
            Destroy(_targetingPrefabInstance);
            _targetingPrefabInstance = null;
        }
        
        if (_isFiring && _canLaunch)
        {
            Attach();
        }

        if (_isHooked)
        {
            DrawHook();
        } else if (_hookLineObjectInstance != null)
        {
            Destroy(_hookLineObjectInstance);
            _hookLineObjectInstance = null;
        }
        
        if (!_isHooked && _hookJoint != null)
        {
            Detach();
        }
    }

    private void UpdateStates()
    {
        if (!_isHooked && Input.GetMouseButton(1))
        {
            _isAiming = true;
            _isFiring = true;

        } else
        {
            _isAiming = false;
            _isFiring = false;
        }
        
        if (_isHooked && Input.GetMouseButtonUp(1))
        {
            _isHooked = false;
        }

        _isWinding = _isHooked && Input.GetMouseButton(0);
    }

    private void Attach()
    {
        _hookPoint = _raycastHit.point;
        
        _hookJoint = playerRigidBodyGameObject.gameObject.AddComponent<SpringJoint>();
        _hookJoint.autoConfigureConnectedAnchor = false;
        _hookJoint.connectedAnchor = _hookPoint;

        _targetHookDistance = Vector3.Distance(playerRb.position, _hookPoint);

        _hookJoint.minDistance = minDistance;
        _hookJoint.maxDistance = _targetHookDistance - initialWinding;

        _hookJoint.spring = spring;
        _hookJoint.damper = damper;
        _hookJoint.massScale = massScale;

        _isHooked = true;
    }

    private void Detach()
    {
        Destroy(_hookJoint);
        _hookJoint = null;
    }

    private void RayCast()
    {
        _targetingPlayer = false;
        if (Physics.Raycast(playerCamera.position, playerCamera.transform.forward, out _raycastHit, Mathf.Infinity))
        {
            _targetingPlayer = _raycastHit.collider.gameObject.tag.Equals("Player");
            _canLaunch = _raycastHit.distance <= maxDistance;

            if (_targetingPlayer)
                _canLaunch = false;
        }
        else
        {
            _canLaunch = false;
        }
        
        Debug.DrawLine(hookStartVisualTransform.position, _raycastHit.point, Color.red);
    }

    private void DrawAimTarget()
    {
        if (_isHooked && _targetingPrefabInstance == null)
        {
            Destroy(_targetingPrefabInstance);
            _targetingPrefabInstance = null;
            return;
        }
        
        if (_raycastHit.point != Vector3.zero)
        {
            if (_targetingPrefabInstance == null)
            {
                _targetingPrefabInstance = Instantiate(targetingPrefab, _raycastHit.point,
                    Quaternion.LookRotation(_raycastHit.normal));
            }
            else
            {
                _targetingPrefabInstance.transform.position = _raycastHit.point;
                _targetingPrefabInstance.transform.rotation = Quaternion.FromToRotation(Vector3.up, _raycastHit.normal);
            }
        }
        else
        {
            if (_targetingPrefabInstance != null)
            {
                Destroy(_targetingPrefabInstance);
                _targetingPrefabInstance = null;
            }
        }
    }

    private void DrawHook()
    {
        if (_hookLineObjectInstance == null)
        {
            _hookLineObjectInstance = Instantiate(hookLineObject);
        }
        
        UpdateHookTransform();
        Debug.DrawLine(hookStartVisualTransform.position, _hookPoint, Color.magenta);
    }
    
    private void UpdateHookTransform()
    {
        Vector3 position = Vector3.Lerp(hookStartVisualTransform.position, _hookPoint, 0.5f);
        _hookLineObjectInstance.transform.position = position;

        float distance = Vector3.Distance(hookStartVisualTransform.position, _hookPoint);
        _hookLineObjectInstance.transform.localScale = new Vector3(0.2f, distance * 0.5f, 0.2f);
        
        Vector3 direction = Vector3.Normalize(hookStartVisualTransform.position - _hookPoint);
        _hookLineObjectInstance.transform.rotation = Quaternion.LookRotation(direction) * Quaternion.FromToRotation(Vector3.down, Vector3.forward);
    }
    
}
