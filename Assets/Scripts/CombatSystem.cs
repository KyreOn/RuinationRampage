using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]

public class CombatSystem : MonoBehaviour
{
    [SerializeField] private GameObject model;
    [SerializeField] private GameObject arrowObject;
    
    
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
    }
    public void Shoot()
    {
        Debug.Log("Shoot");
        _controller.enabled = true;
        Instantiate(arrowObject, transform.position + model.transform.forward, model.transform.rotation);
    }

    private void Update()
    {
        
    }
}
