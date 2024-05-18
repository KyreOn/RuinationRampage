using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(CharacterController))]
public class MovementSystem : MonoBehaviour
{
    [SerializeField] private   float      baseSpeed;
    [SerializeField] private   Transform  cameraOffset;
    [SerializeField] private   float      offsetStrength;
    [SerializeField] protected GameObject model;
    [SerializeField] private   Transform  shootPoint;
    [SerializeField] protected LayerMask  aimLayer;
    
    protected CharacterController _controller;
    protected DamageSystem        _damageSystem;
    protected Animator            _animator;
    protected EffectSystem        _effectSystem;
    protected Vector3             _movementDir;
    protected Camera              _camera;
    protected Vector3             _inputBuffer;
    protected float               _dodgeCooldownTimer;
    protected bool                _isDisplaced;
    protected bool                _isPulled;

    private bool _cameraCentered;
    
    public float curSpeed;
    public bool  isAttacking;
    public bool  canMove;
    public bool  canRotate;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _damageSystem = GetComponent<DamageSystem>();
        _animator = GetComponent<Animator>();
        _effectSystem = GetComponent<EffectSystem>();
        Cursor.lockState = CursorLockMode.Confined;
        _camera = Camera.main;
        canMove = true;
        canRotate = true;

        if (!PlayerPrefs.HasKey("CameraCentered"))
            _cameraCentered = true;
        _cameraCentered = PlayerPrefs.GetInt("CameraCentered") == 0;
    }
    
    private void Move(Vector3 direction)
    {
        if (canMove)
            _movementDir = _inputBuffer;
        if (!_controller.enabled || _isDisplaced || _isPulled) return;
        _controller.Move(direction * (curSpeed * Time.fixedDeltaTime));
    }

    private void FixedUpdate()
    {
        Move(_movementDir);
        if (!canRotate) return;
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
        var pullEffect = _effectSystem.CheckForPulled();
        _isPulled = pullEffect is not null;
        if (_isPulled)
        {
            var position  = transform.position;
            var target    = ((PullingEffect)pullEffect).target;
            var direction = target - position;
            direction.y = 0;
            if (direction.magnitude < 0.5f)
                _effectSystem.RemoveEffectById(16);
            direction.Normalize();
            transform.position = Vector3.MoveTowards(position, position + direction, Time.deltaTime * 50 * ((PullingEffect)pullEffect).speed);
        }
        _isDisplaced = _effectSystem.CheckForDisplacementEffect();
        if (_isDisplaced)
        {
            var position  = transform.position;
            var direction = _effectSystem.GetDisplacementDirection();
            if (Physics.Raycast(position, direction, 1, 1 << 10))
                _isDisplaced = false;
            else
                transform.position = Vector3.MoveTowards(position, position + direction, Time.deltaTime * 50 * _effectSystem.GetDisplacementSpeed());
        }
        CalculateSpeed();
        
        if (!PlayerPrefs.HasKey("CameraCentered"))
            _cameraCentered = true;
        _cameraCentered = PlayerPrefs.GetInt("CameraCentered") == 0;
        
        if (!_cameraCentered && Time.timeScale != 0)
        {
            var mousePos = Input.mousePosition;
            var offset   = new Vector3(Screen.width / 2 - mousePos.x, 0, Screen.height / 2 - mousePos.y);
            offset.Scale(new Vector3(-0.01f * offsetStrength * 9,     0, -0.01f * offsetStrength * 16));
            cameraOffset.localPosition = offset;
        }
        else
        {
            cameraOffset.localPosition = Vector3.zero;
        }
        
        if (_movementDir == Vector3.zero || _effectSystem.CheckIfStunned() || isAttacking)
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

    private void CalculateSpeed()
    {
        curSpeed = baseSpeed * _effectSystem.CalculateSpeedModifiers() * (_effectSystem.CheckIfStunned() || _effectSystem.CheckIfRooted() ? 0 : 1) * (PlayerPrefs.GetString($"ChosenPerks0").Contains('8') ? 1.2f : 1) * (PlayerPrefs.GetString($"ChosenPerks1").Contains('9') ? 0.8f : 1);
    }
}
