using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EffectType
{
    SPEED,
    DISABLE,
    DOT,
    DISPLACEMENT,
    INVINCIBILITY,
    UNSTOPPABLE,
    INCOMEDAMAGE,
    OUTCOMEDAMAGE,
}
public class Effect
{
    private float _duration;
    private float _durationTimer;

    public int        effectId;
    public EffectType effectType;
    
    public Effect(float duration)
    {
        _duration = duration;
    }
    public void UpdateEffect()
    {
        if (_duration == -1) return;
        _durationTimer += Time.deltaTime;
    }

    public virtual float ApplyEffect()
    {
        return 1;
    }

    public bool CheckForEnd()
    {
        if (_duration         == -1) return false;
        return _durationTimer >= _duration;
    }
}
