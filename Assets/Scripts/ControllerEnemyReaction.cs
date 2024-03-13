using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerEnemyReaction : Reaction
{
    [SerializeField] private LayerMask  layerMask;

    private Collider[] _colliders;
    private Animator   _animator;

    protected override void OnAwake()
    {
        _animator = GetComponent<Animator>();
    }

    protected override void OnReact(GameObject stimuli)
    {
        var rotationDir = stimuli.transform.position - transform.position;
        rotationDir.y = 0;
        var rotation = Quaternion.LookRotation(rotationDir);
        transform.rotation = rotation;
        _colliders = Physics.OverlapBox(transform.position + transform.forward * 3,
            new Vector3(3, 3, 3), transform.rotation, layerMask);
        _animator.SetTrigger("React");
        _animator.speed = 4;
    }

    public void CastReact()
    {
        _animator.speed = 1;
        
        foreach (var collider in _colliders)
        {
            if (collider.CompareTag("Projectile"))
            {
                collider.transform.rotation = Quaternion.Inverse(collider.transform.rotation);
            }

            if (collider.CompareTag("Player"))
            {
                collider.GetComponent<EffectSystem>().AddEffect(new StunEffect(2f));
                collider.GetComponent<EffectSystem>().AddEffect(new DisplacementEffect(0.15f, transform.forward));
            }
        }
    }
}
