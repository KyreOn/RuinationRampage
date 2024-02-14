using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class CombatSystem : MonoBehaviour
{
    private Animator            _animator;
    private bool                isAttacking;
    private CharacterController _controller;
    
    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
    }
    
    public void Weak()
    {
        isAttacking = !isAttacking;
        Debug.Log(isAttacking);
        _animator.SetBool("isAttacking", isAttacking);
    }
}
