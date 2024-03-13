using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reaction : MonoBehaviour
{
    [SerializeField] private float      reactionCooldown;
    [SerializeField] private GameObject player;
    [SerializeField] private float      reactionRadius;
    [SerializeField] private LayerMask  reactLayerMask;
    
    private float _cooldownTimer;
    private bool  _canReact = true;
    
    public bool TryReact(GameObject stimuli)
    {
        if (!_canReact) return false;
        var ray = new Ray(transform.position, stimuli.transform.position - transform.position);
        if (Physics.Raycast(ray, out var hit, reactionRadius, reactLayerMask))
        {
            if (hit.transform.gameObject != stimuli)
                return false;
        }
            
        OnReact(stimuli);
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
        OnAwake();
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, player.transform.position) <= reactionRadius)
            TryReact(player);
        if (_canReact) return;
        _cooldownTimer = Mathf.Clamp(_cooldownTimer + Time.deltaTime, 0, reactionCooldown);
        if (_cooldownTimer >= reactionCooldown)
        {
            _canReact = true;
            _cooldownTimer = 0;
        }
    }
}
