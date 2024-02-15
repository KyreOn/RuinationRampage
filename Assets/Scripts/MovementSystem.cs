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
    
    private CharacterController _controller;
    private Animator            _animator;
    private Vector3             _movementDir;
    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        Cursor.lockState = CursorLockMode.Confined;
    }
    
    private void Move(Vector3 direction)
    {
        _controller.Move(direction * (speed * Time.fixedDeltaTime));
    }

    private void FixedUpdate()
    {
        Move(_movementDir);
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hit, float.MaxValue, aimLayer))
        {
            var direction = hit.point - transform.position;
            direction.Scale(new Vector3(1, 0, 1));
            var toRotation = Quaternion.LookRotation(direction, Vector3.up);
            model.transform.rotation = Quaternion.RotateTowards(model.transform.rotation, toRotation, 1000);
        }
    }

    private void Update()
    {
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
        
        Debug.Log(angle);
        //var locomotion = Mathf.Lerp(_animator.GetFloat("Locomotion"), Mathf.Clamp(_movementDir.magnitude, 0, 1), Time.fixedDeltaTime*5);
        _animator.SetFloat("Direction", Mathf.Cos(Mathf.Deg2Rad * angle));
        _animator.SetFloat("Strafe",    Mathf.Sin(Mathf.Deg2Rad * angle));
    }

    public void UpdateDirection(Vector2 direction)
    {
        _movementDir = new Vector3(direction.x, 0, direction.y);
    }
}
