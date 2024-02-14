using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]

public class CombatSystem : MonoBehaviour
{
    [SerializeField] private GameObject model;
    [SerializeField] private GameObject arrowObject;
    [SerializeField] private LayerMask  aimLayer;
    
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
        _controller.enabled = false;
        _animator.SetBool("isAttacking", _isAttacking);
    }

    public void Shoot()
    {
        Debug.Log("Shoot");
        _controller.enabled = true;
        Instantiate(arrowObject, transform.position, model.transform.rotation);
    }

    private void Update()
    {
        if (_controller.enabled) return;
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hit, float.MaxValue, aimLayer))
        {
            var direction  = hit.point - transform.position;
            direction.Scale(new Vector3(1, 0, 1));
            var toRotation = Quaternion.LookRotation(direction, Vector3.up);
            model.transform.rotation = Quaternion.RotateTowards(model.transform.rotation, toRotation, 1000);
        }
    }
}
