using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reaction : MonoBehaviour
{
    [SerializeField] private Collider   reactCollider;
    [SerializeField] private float      reactionCooldown;
    [SerializeField] private GameObject player;
    [SerializeField] private float      reactionRadius;
    [SerializeField] private LayerMask  reactLayerMask;
    
    private float        _cooldownTimer;
    private bool         _canReact = true;
    
    protected EffectSystem effectSystem;
    
    public bool TryReact(GameObject stimuli)
    {
        if (!_canReact) return false;
        if (effectSystem.CheckForInvincibility()) return false;
        var ray = new Ray(transform.position, stimuli.transform.position - transform.position);
        if (Physics.Raycast(ray, out var hit, reactionRadius, reactLayerMask))
        {
            if (hit.transform.gameObject != stimuli)
                return false;
        }
            
        OnReact(stimuli);
        reactCollider.enabled = false;
        _canReact = false;
        return true;
    }

    protected virtual void OnReact(GameObject stimuli)
    {
    }

    protected virtual void OnAwake()
    {
        
    }
    
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        effectSystem = GetComponent<EffectSystem>();
        OnAwake();
    }

    protected virtual void OnReset()
    {
        
    }
    
    private void Update()
    {
        if (Vector3.Distance(transform.position, player.transform.position) <= reactionRadius)
            TryReact(player);
        if (_canReact) return;
        _cooldownTimer = Mathf.Clamp(_cooldownTimer + Time.deltaTime, 0, reactionCooldown);
        if (_cooldownTimer >= reactionCooldown)
        {
            reactCollider.enabled = true;
            _canReact = true;
            _cooldownTimer = 0;
            OnReset();
        }
    }
}
