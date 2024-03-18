using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(CharacterController))]
public class MovementSystem : MonoBehaviour
{
    [SerializeField] private float      baseSpeed;
    [SerializeField] private float      rotationSpeed;
    [SerializeField] private Transform  playerModel; 
    [SerializeField] private Transform  cameraOffset;
    [SerializeField] private float      offsetStrength;
    [SerializeField] private GameObject model;
    [SerializeField] private LayerMask  aimLayer;
    [SerializeField] private LayerMask  dodgeLayer;
    [SerializeField] private Transform  shootPoint;
    [SerializeField] private float      dodgeCooldown;
    
    private CharacterController _controller;
    private DamageSystem        _damageSystem;
    private Animator            _animator;
    private EffectSystem        _effectSystem;
    private Vector3             _movementDir;
    private Camera              _camera;
    private bool                _canMove;
    private bool                _canRotate;
    private Vector3             _inputBuffer;
    private float               _dodgeCooldownTimer;
    private bool                _isDisplaced;
    
    public float curSpeed;
    public bool  isAttacking;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _damageSystem = GetComponent<DamageSystem>();
        _animator = GetComponent<Animator>();
        _effectSystem = GetComponent<EffectSystem>();
        Cursor.lockState = CursorLockMode.Confined;
        _camera = Camera.main;
        _canMove = true;
        _canRotate = true;
    }
    
    private void Move(Vector3 direction)
    {
        if (_canMove)
            _movementDir = _inputBuffer;
        if (!_controller.enabled || _isDisplaced) return;
        _controller.Move(direction * (curSpeed * Time.fixedDeltaTime));
    }

    private void FixedUpdate()
    {
        Move(_movementDir);
        if (!_canRotate) return;
        var ray = _camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hit, float.MaxValue, aimLayer))
        {
            var direction = hit.point - (isAttacking ? shootPoint.position : transform.position);
            if (direction.magnitude <= 0.6f)
                return;
            direction.Scale(new Vector3(1, 0, 1));
            var toRotation = Quaternion.LookRotation(direction, Vector3.up);
            model.transform.rotation = Quaternion.RotateTowards(model.transform.rotation, toRotation, 100);
        }
    }

    private void Update()
    {
        _isDisplaced = _effectSystem.CheckForDisplacementEffect();
        if (_isDisplaced)
        {
            var position  = transform.position;
            var direction = _effectSystem.GetDisplacementDirection();
            if (Physics.Raycast(position, direction, 1))
                _isDisplaced = false;
            else
                transform.position = Vector3.MoveTowards(position, position + direction, Time.deltaTime * 50 * _effectSystem.GetDisplacementSpeed());
        }
        CalculateSpeed();
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
        _controller.enabled = true;
        _animator.speed = 2;
        _effectSystem.AddEffect(new SlowEffect(0.5f, 0.5f));
        _canMove = false;
        _canRotate = false;
        if (_movementDir == Vector3.zero)
        {
            _movementDir = model.transform.forward;
        }
        DodgeRotate();
        _damageSystem.SetInvincible(true);
        _controller.excludeLayers = dodgeLayer;
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
        _damageSystem.SetInvincible(false);
        _effectSystem.AddEffect(new SlowEffect(1f, 0.8f));
        _controller.excludeLayers = aimLayer;
    }

    public float GetSpeed() => curSpeed;

    private void CalculateSpeed()
    {
        curSpeed = baseSpeed * _effectSystem.CalculateSpeedModifiers() * (_effectSystem.CheckIfDisabled() ? 0 : 1);
    }
}
