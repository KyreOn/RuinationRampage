using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CasterEnemyReaction : Reaction
{
    [SerializeField] private LayerMask  layerMask;
    

    private Animator _animator;

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
        _animator.SetTrigger("React");
    }

    public void CastReact()
    {
        var colliders = Physics.OverlapBox(transform.position + transform.forward * 3,
            new Vector3(3, 3, 3), transform.rotation, layerMask);
        foreach (var collider in colliders)
        {
            if (collider.CompareTag("Projectile"))
            {
                collider.transform.rotation = Quaternion.Inverse(collider.transform.rotation);
            }

            if (collider.CompareTag("Player"))
            {
                collider.GetComponent<EffectSystem>().AddEffect(new StunEffect(0.15f));
                collider.GetComponent<EffectSystem>().AddEffect(new DisplacementEffect(0.15f, transform.forward));
            }
        }
    }
}
