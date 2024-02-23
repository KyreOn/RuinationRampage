using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(MovementSystem))]
[RequireComponent(typeof(CombatSystem))]
public class CharacterInput : MonoBehaviour
{
    private MovementSystem _movementSystem;
    private CombatSystem   _combatSystem;

    private void Awake()
    {
        _movementSystem = GetComponent<MovementSystem>();
        _combatSystem = GetComponent<CombatSystem>();
    }
    
    public void OnMove(InputValue value)
    {
        _movementSystem.UpdateDirection(value.Get<Vector2>());
    }

    public void OnWeak()
    {
        _combatSystem.Weak();
    }

    public void OnStrong()
    {
        _combatSystem.Strong();
    }
    
    public void OnDodge()
    {
        _movementSystem.Dodge();
    }
}
