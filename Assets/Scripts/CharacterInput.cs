using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(MovementSystem))]
[RequireComponent(typeof(CombatSystem))]
public class CharacterInput : MonoBehaviour
{
    [SerializeField] private InputActionReference movement;
    [SerializeField] private InputActionReference weakAttack;
    [SerializeField] private InputActionReference strongAttack;
    [SerializeField] private InputActionReference dodge;
    [SerializeField] private InputActionReference spellQ;
    [SerializeField] private InputActionReference spellE;
    [SerializeField] private InputActionReference spellR;
    [SerializeField] private InputActionReference pause;
    
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
        dodge.action.started += OnPrepareDodge;
        dodge.action.canceled += OnCastDodge;
        spellQ.action.started += OnPrepareSpellQ;
        spellQ.action.canceled += OnCastSpellQ;
        spellE.action.started += OnPrepareSpellE;
        spellE.action.canceled += OnCastSpellE;
        spellR.action.started += OnPrepareSpellR;
        spellR.action.canceled += OnCastSpellR;
        pause.action.performed += OnPause;
    }

    private void OnDisable()
    {
        weakAttack.action.started -= WeakAttackStart;
        weakAttack.action.canceled -= WeakAttackStop;
        strongAttack.action.started -= StrongAttackStart;
        strongAttack.action.canceled -= StrongAttackStop;
        dodge.action.started -= OnPrepareDodge;
        dodge.action.canceled -= OnCastDodge;
        spellQ.action.started -= OnPrepareSpellQ;
        spellQ.action.canceled -= OnCastSpellQ;
        spellE.action.started -= OnPrepareSpellE;
        spellE.action.canceled -= OnCastSpellE;
        spellR.action.started -= OnPrepareSpellR;
        spellR.action.canceled -= OnCastSpellR;
        pause.action.performed -= OnPause;
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
    
    private void OnPrepareDodge(InputAction.CallbackContext obj)
    {
        _combatSystem.PrepareDodge();
    }
    
    private void OnCastDodge(InputAction.CallbackContext obj)
    {
        _combatSystem.CastDodge();
    }

    private void OnPrepareSpellQ(InputAction.CallbackContext obj)
    {
        _combatSystem.PrepareSpellQ();
    }
    
    private void OnCastSpellQ(InputAction.CallbackContext obj)
    {
        _combatSystem.CastSpellQ();
    }
    
    private void OnPrepareSpellE(InputAction.CallbackContext obj)
    {
        _combatSystem.PrepareSpellE();
    }
    
    private void OnCastSpellE(InputAction.CallbackContext obj)
    {
        _combatSystem.CastSpellE();
    }
    
    private void OnPrepareSpellR(InputAction.CallbackContext obj)
    {
        _combatSystem.PrepareSpellR();
    }
    
    private void OnCastSpellR(InputAction.CallbackContext obj)
    {
        _combatSystem.CastSpellR();
    }

    private void OnPause(InputAction.CallbackContext obj)
    {
        SceneManager.LoadScene(0);
    }
    
    private void Update()
    {
        _movementSystem.UpdateDirection(movement.action.ReadValue<Vector2>());
    }
}
