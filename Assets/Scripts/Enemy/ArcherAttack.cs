using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ArcherAttack : MonoBehaviour
{
    [SerializeField] private GameObject arrowObject;
    [SerializeField] private Transform  spawnPoint;
    [SerializeField] private float      attackCooldown;

    private GameObject _target;
    private Animator   _animator;
    private bool       _isDrawing;
    private float      _attackCooldownTimer;

    public bool canAttack = true;
    public bool isAttacking;
    
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public bool StartAttack(GameObject target)
    {
        if (!canAttack) return false;
        _target = target;
        isAttacking = true;
        canAttack = false;
        _animator.SetBool("isAttacking", true);
        _isDrawing = true;
        return true;
    }

    public void Draw()
    {
        
    }
    
    public void Shoot()
    {
        _isDrawing = false;
        Instantiate(arrowObject, spawnPoint.position, transform.rotation);
        _animator.SetBool("isAttacking", false);
    }
    
    public void ShootEnd()
    {
        isAttacking = false;
    }

    private void Update()
    {
        if (!canAttack)
        {
            _attackCooldownTimer += Time.deltaTime;
            if (_attackCooldownTimer >= attackCooldown)
            {
                canAttack = true;
                _attackCooldownTimer = 0;
            }
        }
        
        
        if (!_isDrawing) return;
        var direction = _target.transform.position - spawnPoint.position;
        direction.Scale(new Vector3(1, 0, 1));
        transform.rotation = Quaternion.LookRotation(direction);
    }
}
