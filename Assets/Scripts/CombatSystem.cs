using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]

public class CombatSystem : MonoBehaviour
{
    [SerializeField] private GameObject model;
    [SerializeField] private GameObject arrowObject;
    [SerializeField] private Transform  spawnPoint;
    
    private Animator            _animator;
    private bool                _isAttacking;
    private CharacterController _controller;
    
    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
    }

    public void Weak()
    {
        _isAttacking = !_isAttacking;
        _animator.SetBool("isAttacking", _isAttacking);
    }

    public void Draw()
    {
        _controller.enabled = false;
        _animator.speed = 3;
    }
    public void Shoot()
    {
        Instantiate(arrowObject, spawnPoint.position, model.transform.rotation);
    }

    public void ShootEnd()
    {
        _controller.enabled = true;
        _animator.speed = 1;
    }

    private void Update()
    {
        
    }
}
