using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherMovementSystem : MovementSystem
{
    [SerializeField] private LayerMask dodgeLayer;

    public void OnDodgeStart()
    {
        _controller.enabled = true;
        _animator.speed = 1;
        _effectSystem.AddEffect(new SlowEffect(0.5f, 0.5f), false);
        canMove = false;
        canRotate = false;
        if (_movementDir == Vector3.zero)
        {
            _movementDir = model.transform.forward;
        }

        //model.transform.rotation = Quaternion.LookRotation(_movementDir);
        _damageSystem.SetInvincible(true);
        _controller.excludeLayers = dodgeLayer;
    }

    public void OnDodgeEnd(float boostStrength)
    {
        _animator.speed = 1;
        canMove = true;
        canRotate = true;
        _damageSystem.SetInvincible(false);
        _effectSystem.AddEffect(
            new SlowEffect(PlayerPrefs.GetString($"ChosenPerks0").Contains('2') ? 1.5f : 1, boostStrength), false);
        _controller.excludeLayers = aimLayer;
    }
}
