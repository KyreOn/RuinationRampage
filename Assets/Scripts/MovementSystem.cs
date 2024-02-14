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
        if (_movementDir == Vector3.zero) return;
        var toRotation = Quaternion.LookRotation(_movementDir, Vector3.up);
        playerModel.rotation = Quaternion.RotateTowards(playerModel.rotation, toRotation, rotationSpeed * Time.fixedDeltaTime);
    }

    private void Update()
    {
        var mousePos = Input.mousePosition;
        var offset   = new Vector3(Screen.width / 2 - mousePos.x, 0, Screen.height / 2 - mousePos.y);
        offset.Scale(new Vector3(-0.01f * offsetStrength * 9,     0, -0.01f * offsetStrength * 16));
        cameraOffset.localPosition = offset;
        var locomotion = Mathf.Lerp(_animator.GetFloat("Locomotion"), Mathf.Clamp(_movementDir.magnitude, 0, 1), Time.fixedDeltaTime*5);
        _animator.SetFloat("Locomotion", locomotion);
    }

    public void UpdateDirection(Vector2 direction)
    {
        _movementDir = new Vector3(direction.x, 0, direction.y);
    }
}
