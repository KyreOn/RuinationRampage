using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EffectSystem : MonoBehaviour
{
    private List<Effect> _effects         = new();
    private List<Effect> _markedForDelete = new();
    
    void Update()
    {
        foreach (var effect in _effects)
        {
            effect.UpdateEffect();
            if (effect.CheckForEnd()) 
                _markedForDelete.Add(effect);
        }

        foreach (var effect in _markedForDelete) 
            _effects.Remove(effect);
        _markedForDelete.Clear();
    }

    public void AddEffect(Effect effect, bool stackable = true)
    {
        if (!stackable)
            if (CheckForEffectById(effect.effectId))
                _effects.Remove(GetEffectById(effect.effectId));
        _effects.Add(effect);
    }

    public float CalculateSpeedModifiers()
    {
        return _effects.Where(effect => effect.effectType == EffectType.SPEED)
            .Aggregate(1f, (current, effect) => 
                current * (CheckForSlowImmune() || CheckForDisableImmune() ? Math.Clamp(effect.ApplyEffect(), 1, float.MaxValue) : effect.ApplyEffect()));
    }

    public bool CheckIfStunned()
    {
        return !CheckForDisableImmune() && _effects.Any(effect => effect.effectType == EffectType.STUN);
    }
    
    public bool CheckIfRooted()
    {
        return !CheckForDisableImmune() && _effects.Any(effect => effect.effectType == EffectType.ROOT);
    }

    public float CalculateDOT()
    {
        return _effects.Where(effect => effect.effectType == EffectType.DOT).Sum(effect => effect.ApplyEffect());
    }

    public bool CheckForDisplacementEffect()
    {
        return _effects.Any(effect => effect.effectType == EffectType.DISPLACEMENT);
    }

    public Vector3 GetDisplacementDirection()
    {
        foreach (var effect in _effects)
        {
            if (effect.effectType == EffectType.DISPLACEMENT)
                return ((DisplacementEffect)effect).direction.normalized;
        }
        return Vector3.zero;
    }
    
    public float GetDisplacementSpeed()
    {
        foreach (var effect in _effects)
        {
            if (effect.effectType == EffectType.DISPLACEMENT)
                return ((DisplacementEffect)effect).speed;
        }
        return 0;
    }

    public bool CheckForInvincibility()
    {
        return _effects.Any(effect => effect.effectType == EffectType.INVINCIBILITY);
    }

    public bool CheckForSlowImmune()
    {
        return _effects.Any(effect => effect.effectType == EffectType.SLOW_IMMUNE);
    }
    
    public bool CheckForDisableImmune()
    {
        return _effects.Any(effect => effect.effectType == EffectType.DISABLE_IMMUNE);
    }
    
    public float CalculateIncomeDamage()
    {
        return _effects.Where(effect => effect.effectType == EffectType.INCOME_DAMAGE)
            .Aggregate(1f, (current, effect) => 
                current * effect.ApplyEffect());
    }
    
    public float CalculateOutcomeDamage()
    {
        return _effects.Where(effect => effect.effectType == EffectType.OUTCOME_DAMAGE)
            .Aggregate(1f, (current, effect) => 
                current * effect.ApplyEffect());
    }
    
    private Effect GetEffectById(int id)
    {
        foreach (var effect in _effects)
        {
            if (effect.effectId == id)
                return effect;
        }

        return null;
    }
    
    public bool CheckForEffectById(int id)
    {
        foreach (var effect in _effects)
        {
            if (effect.effectId == id)
                return true;
        }

        return false;
    }
}
