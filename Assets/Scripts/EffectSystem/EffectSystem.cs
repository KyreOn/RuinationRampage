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
        return !CheckForDisableImmune() && _effects.Any(effect => effect.effectType is EffectType.ROOT or EffectType.STUN);
    }

    public float CalculateDOT()
    {
        return _effects.Where(effect => effect.effectType == EffectType.DOT).Sum(effect => effect.ApplyEffect());
    }

    public bool CheckForDisplacementEffect()
    {
        return _effects.Any(effect => effect.effectType == EffectType.DISPLACEMENT) && !CheckForDisableImmune();
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

    public Effect CheckForPulled()
    {
        return _effects.FirstOrDefault(effect => effect.effectType == EffectType.PULLING);
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

    public Effect CheckForParry()
    {
        return _effects.FirstOrDefault(effect => effect.effectType == EffectType.PARRY);
    }

    public float CalculateTempHealth()
    {
        var result = 0f;
        foreach (var effect in _effects)
        {
            if (effect.effectType != EffectType.TEMPORARY_HEALTH) continue;
            result += effect.ApplyEffect();
        }

        return result;
    }
    
    public float DamageTempHealth(float damage)
    {
        foreach (var effect in _effects)
        {
            if (damage            <= 0) break;
            if (effect.effectType != EffectType.TEMPORARY_HEALTH) continue;
            var hp = effect.ApplyEffect();
            if (hp >= damage)
            {
                ((TemporaryHealthEffect)effect).ApplyDamage(damage);
                damage = 0;
            }
            else
            {
                damage -= hp;
                ((TemporaryHealthEffect)effect).ApplyDamage(hp);
            }
        }

        return damage;
    }

    public bool CanAssassinDodge()
    {
        return _effects.Any(effect => effect.effectType == EffectType.ASSASSIN_DODGE);
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

    public void RemoveEffectById(int id)
    {
        foreach (var effect in _effects)
        {
            if (effect.effectId == id)
                _markedForDelete.Add(effect);
        }
    }
}
