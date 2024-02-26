using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EffectType
{
    SPEED,
    DISABLE,
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
        _durationTimer += Time.deltaTime;
    }

    public virtual float ApplyEffect()
    {
        return 1;
    }

    public bool CheckForEnd() => _durationTimer >= _duration;
}
