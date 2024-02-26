using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(MovementSystem))]
[RequireComponent(typeof(CombatSystem))]
public class CharacterInput : MonoBehaviour
{
    [SerializeField] private InputActionReference movement;
    [SerializeField] private InputActionReference weakAttack;
    [SerializeField] private InputActionReference strongAttack;
    [SerializeField] private InputActionReference dodge;
    
    private MovementSystem _movementSystem;
    private CombatSystem   _combatSystem;

    private void Awake()
    {
        _movementSystem = GetComponent<MovementSystem>();
        _combatSystem = GetComponent<CombatSystem>();
    }
    
    private void OnEnable()
    {
        weakAttack.action.started += WeakAttackStart;
        weakAttack.action.canceled += WeakAttackStop;
        strongAttack.action.started += StrongAttackStart;
        strongAttack.action.canceled += StrongAttackStop;
        dodge.action.performed += OnDodge;
    }

    private void OnDisable()
    {
        weakAttack.action.started -= WeakAttackStart;
        weakAttack.action.canceled -= WeakAttackStop;
        strongAttack.action.started -= StrongAttackStart;
        strongAttack.action.canceled -= StrongAttackStop;
        dodge.action.performed -= OnDodge;
    }
    
    private void WeakAttackStart(InputAction.CallbackContext obj)
    {
        _combatSystem.StartWeak();
    }

    private void WeakAttackStop(InputAction.CallbackContext obj)
    {
        _combatSystem.StopWeak();
    }
    
    private void StrongAttackStart(InputAction.CallbackContext obj)
    {
        _combatSystem.StartStrong();
    }
    
    private void StrongAttackStop(InputAction.CallbackContext obj)
    {
        _combatSystem.StopStrong();
    }
    
    private void OnDodge(InputAction.CallbackContext obj)
    {
        _movementSystem.Dodge();
    }

    private void Update()
    {
        _movementSystem.UpdateDirection(movement.action.ReadValue<Vector2>());
    }
}
