using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class MovementSystem : MonoBehaviour
{
    [SerializeField] private float      speed;
    [SerializeField] private float      rotationSpeed;
    [SerializeField] private Transform  playerModel; 
    [SerializeField] private Transform  cameraOffset;
    [SerializeField] private float      offsetStrength;
    [SerializeField] private GameObject model;
    [SerializeField] private LayerMask  aimLayer;
    [SerializeField] private Transform  shootPoint;
    
    private CharacterController _controller;
    private HitEffect           _hitEffect;
    private Animator            _animator;
    private Vector3             _movementDir;
    private Camera              _camera;
    private bool                _canMove;
    private bool                _canRotate;
    private Vector3             _inputBuffer;
    private float               _curSpeed;
    

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _hitEffect = GetComponent<HitEffect>();
        _animator = GetComponent<Animator>();
        Cursor.lockState = CursorLockMode.Confined;
        _camera = Camera.main;
        _canMove = true;
        _canRotate = true;
        _curSpeed = speed;
    }
    
    private void Move(Vector3 direction)
    {
        if (_canMove)
            _movementDir = _inputBuffer;
        _controller.Move(direction * (_curSpeed * Time.fixedDeltaTime));
    }

    private void FixedUpdate()
    {
        Move(_movementDir);
        var ray = _camera.ScreenPointToRay(Input.mousePosition);
        if (!_canRotate) return;
        if (Physics.Raycast(ray, out var hit, float.MaxValue, aimLayer))
        {
            var direction = hit.point - shootPoint.position;
            if (direction.magnitude <= 0.6f)
                return;
            direction.Scale(new Vector3(1, 0, 1));
            var toRotation = Quaternion.LookRotation(direction, Vector3.up);
            model.transform.rotation = Quaternion.RotateTowards(model.transform.rotation, toRotation, 100);
        }
    }

    private void Update()
    {
        _curSpeed = Mathf.Lerp(_curSpeed, speed, Time.deltaTime);
        var mousePos = Input.mousePosition;
        var offset   = new Vector3(Screen.width / 2 - mousePos.x, 0, Screen.height / 2 - mousePos.y);
        offset.Scale(new Vector3(-0.01f * offsetStrength * 9,     0, -0.01f * offsetStrength * 16));
        cameraOffset.localPosition = offset;
        if (_movementDir == Vector3.zero)
        {
            _animator.SetFloat("Direction", 0);
            _animator.SetFloat("Strafe",    0);
            return;
        }
        var rotation       = model.transform.rotation.eulerAngles.y;
        var rotationVector = new Vector3(Mathf.Sin(Mathf.Deg2Rad * rotation), 0, Mathf.Cos(Mathf.Deg2Rad * rotation));
        var angle          = Vector3.SignedAngle(_movementDir, rotationVector, Vector3.up);
        
        _animator.SetFloat("Direction", Mathf.Cos(Mathf.Deg2Rad * angle));
        _animator.SetFloat("Strafe",    Mathf.Sin(Mathf.Deg2Rad * angle));
    }

    public void UpdateDirection(Vector2 direction)
    {
        _inputBuffer = new Vector3(direction.x, 0, direction.y);
    }
    
    public void Dodge()
    {
        _animator.SetTrigger("Dodge");
    }

    public void DodgeStart()
    {
        _animator.speed = 2;
        _curSpeed = 10;
        _canMove = false;
        _canRotate = false;
        if (_movementDir == Vector3.zero)
        {
            _movementDir = model.transform.forward;
        }
        DodgeRotate();
        _hitEffect.SetInvincible(true);
    }
    private void DodgeRotate()
    {
        var toRotation = Quaternion.LookRotation(_movementDir, Vector3.up);
        model.transform.rotation = Quaternion.RotateTowards(model.transform.rotation, toRotation, 1000);
    }
    public void DodgeEnd()
    {
        _animator.speed = 1;
        _canMove = true;
        _canRotate = true;
        _hitEffect.SetInvincible(false);
        _curSpeed = 10;
    }

    public float GetSpeed() => _curSpeed;
}
